using System;
using System.Collections;
using System.Collections.Generic;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class TestingPanel : Panel, ITestPanel
	{
		public event Action<int, int> OnSetProgress;
		public event Action OnTestResult;

		[Header("Спрайты для вопросов")]

		[SerializeField]
		private Sprite fillSprite = null;

		[SerializeField]
		private Sprite rightSprite = null;

		[SerializeField]
		private Sprite falseSprite = null;

		[Header("Панель с вопросами")]

		[SerializeField]
		private Dropdown changeQuestionDropMenu = null;

		[SerializeField]
		private RectTransform questionArea = null;

		[SerializeField]
		private Button nextButton = null, prevButton = null, firstButton = null;

		[SerializeField]
		private Button completeButton = null;

		[SerializeField]
		private Text time = null;
		
		/// <summary>
		/// Проходимый тест
		/// </summary>
		private PassedTest curPassedTest;

		/// <summary>
		/// Проба или зачет
		/// </summary>
		private bool isProbe;

		/// <summary>
		/// Количество пройденных вопросов
		/// </summary>
		private int passedQuestionCounter;

		/// <summary>
		/// Возможные варианты ответа на вопрос (неверно, верно, не ответил)
		/// </summary>
		private enum AnswerResults
		{
			False,
			True,
			None
		}

		private class QuestionInfo
		{
			public int QuestionId;
			public int QuestionType;
			public AnswerResults Results;
			public IQuestionForm QuestionForm;

			public QuestionInfo(int questionId, int questionType, AnswerResults results)
			{
				QuestionId = questionId;
				QuestionType = questionType;
				Results = results;
			}
		}
		private readonly List<QuestionInfo> questionInfo = new List<QuestionInfo>();

		private int questionIndex;

		private readonly Random rng = new Random();

		public override void Initialize()
		{
			base.Initialize();

			nextButton.onClick.AddListener(NextQuestion);
			prevButton.onClick.AddListener(PreviosQuestion);
			firstButton.onClick.AddListener(FirstQuestion);
			changeQuestionDropMenu.onValueChanged.AddListener(ChangeQuestion);
			completeButton.onClick.AddListener(CompleteTest);
		}

		/// <summary>
		/// Показать меню
		/// </summary>
		/// <param name="passedTest">Запись проходимого теста</param>
		/// <param name="isGoProbe">Проба или зачет</param>
		public void Show(PassedTest passedTest, bool isGoProbe)
		{
			base.Show();

			curPassedTest = passedTest;
			isProbe = isGoProbe;
			questionIndex = 0;

			StartCoroutine(StartTimer());
			InitQuestions();
			FillQuestions();

		}

		/// <summary>
		/// Спрятать меню
		/// </summary>
		public override void Hide()
		{
			ClearQuestionInfo();
			base.Hide();
		}

		/// <summary>
		/// Очистить информацию о прохождении
		/// </summary>
		private void ClearQuestionInfo()
		{
			foreach (var info in questionInfo)
			{
				if (info.QuestionForm != null)
				{
					Destroy((UnityEngine.Object)info.QuestionForm);
				}
			}

			questionInfo.Clear();
		}

		/// <summary>
		/// Запустить таймер
		/// </summary>
		private IEnumerator StartTimer()
		{
			int minutes = TestControlsSettings.Default().Time;
			int seconds = 0;

			while (minutes != 0 || seconds != 0)
			{
				if (seconds == 0)
				{
					minutes--;
					seconds = 59;
				}
				else
				{
					seconds--;
				}

				time.text = minutes.ToString("00") + ":" + seconds.ToString("00");
				yield return new WaitForSeconds(1f);
			}

			CompleteTest();
		}

		/// <summary>
		/// Получить список вопросов
		/// </summary>
		private void InitQuestions()
		{
			passedQuestionCounter = 0;
			ClearQuestionInfo();

			foreach (var question in curPassedTest.Questions)
			{
				questionInfo.Add(new QuestionInfo(question.QuestionId, (int)question.TypeQuestion, AnswerResults.None));
			}

			if (TestControlsSettings.Default().NeedShuffle) Shuffle(questionInfo);
		}

		/// <summary>
		/// Заполнить меню выбора вопроса
		/// </summary>
		private void FillQuestions()
		{
			changeQuestionDropMenu.ClearOptions();

			if (questionInfo.Count != 0)
			{
				List<Dropdown.OptionData> buttonNames = new List<Dropdown.OptionData>();
				changeQuestionDropMenu.itemImage.sprite = fillSprite;


				for (int i = 0; i < questionInfo.Count; i++)
				{
					QuestionInfo question = questionInfo[i];
					buttonNames.Add(new Dropdown.OptionData(string.Format("Вопрос №" + question.QuestionId + " ({0} из {1})", (i + 1), questionInfo.Count)));
				}
				changeQuestionDropMenu.AddOptions(buttonNames);

				ShowQuestion(0);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(changeQuestionDropMenu.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Отобразить вопрос на экране
		/// </summary>
		/// <param name="newQuestionIndex"></param>
		private void ShowQuestion(int newQuestionIndex)
		{
			CheckQuestionResult();

			questionIndex = newQuestionIndex;

			CreateQuestion(questionInfo[questionIndex].QuestionType);

			PrepareNavigationButtons();

			OnSetProgress?.Invoke(passedQuestionCounter, questionInfo.Count);
		}

		/// <summary>
		/// Изменить состояние кнопок навигации
		/// </summary>
		private void PrepareNavigationButtons()
		{
			nextButton.interactable = !IsFinalQuestion();
			prevButton.interactable = questionIndex > 0;
		}

		/// <summary>
		/// Создаем вопрос
		/// </summary>
		/// <param name="pType"></param>
		private void CreateQuestion(int pType)
		{
			if (questionInfo[questionIndex].QuestionForm == null)
			{
				GameObject questionObject = TestControlsSettings.Default().GetQuestionObject(pType);
				if (questionObject != null)
				{

					var rectTransform = questionObject.GetComponent<RectTransform>();
					rectTransform.SetParent(questionArea.transform);
					rectTransform.anchoredPosition = Vector2.zero;
					rectTransform.sizeDelta = Vector2.zero;
					rectTransform.localScale = Vector3.one;

					rectTransform.localPosition = Vector3.ProjectOnPlane(rectTransform.localPosition, Vector3.forward);
					rectTransform.localRotation = Quaternion.identity;


					questionObject.transform.SetAsFirstSibling();

					questionInfo[questionIndex].QuestionForm = questionObject.GetComponent<IQuestionForm>();
					//Берем из базы данных все необходимые значения
					questionInfo[questionIndex].QuestionForm.QuestionId = questionInfo[questionIndex].QuestionId;
					questionInfo[questionIndex].QuestionForm.Load();
				}
			}
			else
			{
				questionInfo[questionIndex].QuestionForm.Show();
			}

		}

		/// <summary>
		/// Проверить результат 
		/// </summary>
		private void CheckQuestionResult()
		{
			if (questionInfo[questionIndex].QuestionForm != null)
			{
				IQuestionForm checkedForm = questionInfo[questionIndex].QuestionForm;

				var enteredAnswer = checkedForm.SetInLog().Answer.Replace(" ", "");
				if (enteredAnswer.Equals(""))
				{
					questionInfo[questionIndex].Results = AnswerResults.False;
				}
				else
				{
					questionInfo[questionIndex].Results = checkedForm.CheckIfRight() ? AnswerResults.True : AnswerResults.False;
				}

				if (isProbe)
				{
					switch (questionInfo[questionIndex].Results)
					{
						case AnswerResults.False:
							changeQuestionDropMenu.options[questionIndex].image = falseSprite;
							break;
						case AnswerResults.True:
							changeQuestionDropMenu.options[questionIndex].image = rightSprite;
							break;
					}
				}

				if (curPassedTest.PassedQuestions.Exists(a => a.QuestionId == checkedForm.QuestionId))
				{

					for (int i = 0; i < curPassedTest.PassedQuestions.Count; i++)
					{
						if (checkedForm.QuestionId == curPassedTest.PassedQuestions[i].QuestionId)
						{
							curPassedTest.PassedQuestions.RemoveAt(i);
							curPassedTest.PassedQuestions.Insert(i, checkedForm.SetInLog());

							break;
						}
					}
				}
				else
				{
					curPassedTest.PassedQuestions.Add(questionInfo[questionIndex].QuestionForm.SetInLog());
					passedQuestionCounter++;
				}

				checkedForm.Hide();
			}
		}

		/// <summary>
		/// Переключить на следующий вопрос
		/// </summary>
		private void NextQuestion()
		{
			if (!IsFinalQuestion())
			{
				ChangeQuestion(questionIndex + 1);
			}
		}

		/// <summary>
		/// Переключить на предыдущий вопрос
		/// </summary>
		private void PreviosQuestion()
		{
			if (questionIndex > 0)
			{
				ChangeQuestion(questionIndex - 1);
			}
		}

		/// <summary>
		/// Переключить на первый вопрос
		/// </summary>
		private void FirstQuestion()
		{
			ChangeQuestion(0);
		}

		/// <summary>
		/// Изменить вопрос
		/// </summary>
		/// <param name="id"></param>
		private void ChangeQuestion(int id)
		{
			changeQuestionDropMenu.value = id;
			ShowQuestion(id);
		}

		/// <summary>
		/// Нажать кнопку закончить тест
		/// </summary>
		private void CompleteTest()
		{
			CheckQuestionResult();
			OnTestResult?.Invoke();
		}

		#region Вспомогательные методы

		/// <summary>
		/// Функция сортировки вопросов в случайном порядке
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		private void Shuffle<T>(List<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		/// <summary>
		/// Нет ли больше вопросов в тесте
		/// </summary>
		/// <returns></returns>
		private bool IsFinalQuestion()
		{
			return questionIndex + 1 >= questionInfo.Count;
		}

		/// <summary>
		/// Закончит ли тест
		/// </summary>
		/// <returns></returns>
		private bool IsCompleteTest()
		{
			return passedQuestionCounter >= questionInfo.Count - 1;
		}

		#endregion
	}
}