using System;
using Core.Extensions;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class FinishTestPanel : Panel, ITestPanel
	{
		public event Action OnDeleteProgress;
		public event Action OnExitButtonClick;

		[SerializeField]
		private Text finishTitle = null;

		[SerializeField]
		private Text finishDescription = null;

		[SerializeField]
		private Button closeButton = null;

		[SerializeField]
		private Button finishButton = null;

		[SerializeField]
		private GameObject numberColumn = null, enteredAnswerColumn = null, isRightAnswerColumn = null;

		private PassedTest currentPassedTest;

		public override void Initialize()
		{
			base.Initialize();
			finishButton.onClick.AddListener(() => OnExitButtonClick?.Invoke());
			closeButton.onClick.AddListener(() => OnExitButtonClick?.Invoke());
		}

		public void Show(PassedTest passedTest)
		{
			base.Show();
			currentPassedTest = passedTest;

			// Количество правильных ответов
			int rightAnswerCount = GetRightQuestionCount();

			// Установить оценку
			SetGrade(rightAnswerCount / (double)currentPassedTest.PassedQuestions.Count);

			// Установить описание
			finishTitle.text = currentPassedTest.Name;
			finishDescription.text = $"Вы ответили на {rightAnswerCount} из {currentPassedTest.PassedQuestions.Count} вопросов. Ваша оценка: {currentPassedTest.Grade}";
			
			// Дата прохождения теста
			currentPassedTest.Date = DateTime.Now;
			currentPassedTest.Log = $"Время: {currentPassedTest.Date.ToLocalTime():dd MMMM yyyy HH:mm}\n Ошибок: {currentPassedTest.PassedQuestions.Count - rightAnswerCount}";


			CreateResultTable();

			OnDeleteProgress?.Invoke();
		}

		/// <summary>
		/// Создание таблицы с результатами
		/// </summary>
		private void CreateResultTable()
		{
			ClearResultTable();

			var count = 1;
			foreach (var passedQuestion in currentPassedTest.PassedQuestions)
			{
				var countBtn = UiBuilder.CreateButton(numberColumn.transform, Vector3.one, count.ToString());
				countBtn.transform.localPosition = Vector3.ProjectOnPlane(countBtn.transform.localPosition, Vector3.forward);
				countBtn.transform.localRotation = Quaternion.identity;
				countBtn.GetComponentInChildren<RectTransform>().SetHeight(100);


				var answeredBtn = UiBuilder.CreateButton(enteredAnswerColumn.transform, Vector3.one, passedQuestion.Answer);
				answeredBtn.transform.localPosition = Vector3.ProjectOnPlane(answeredBtn.transform.localPosition, Vector3.forward);
				answeredBtn.transform.localRotation = Quaternion.identity;
				answeredBtn.GetComponentInChildren<RectTransform>().SetHeight(100);

				var isRightBtn = UiBuilder.CreateButton(isRightAnswerColumn.transform, Vector3.one, passedQuestion.IsRight ? "Правильно" : "Неправильно");
				isRightBtn.transform.localPosition = Vector3.ProjectOnPlane(isRightBtn.transform.localPosition, Vector3.forward);
				isRightBtn.transform.localRotation = Quaternion.identity;
				isRightBtn.GetComponentInChildren<RectTransform>().SetHeight(100);

				count++;
			}
		}

		/// <summary>
		/// Очистка таблицы с результатами
		/// </summary>
		private void ClearResultTable()
		{
			for (int i = 1; i < numberColumn.transform.childCount; i++)
			{
				Destroy(numberColumn.transform.GetChild(i).gameObject);
				Destroy(enteredAnswerColumn.transform.GetChild(i).gameObject);
				Destroy(isRightAnswerColumn.transform.GetChild(i).gameObject);
			}
		}

		/// <summary>
		/// Получить количество правильных ответов
		/// </summary>
		/// <returns></returns>
		private int GetRightQuestionCount()
		{
			int rightAnswerCount = 0;

			foreach (var question in currentPassedTest.PassedQuestions)
			{
				if (question.IsRight) rightAnswerCount++;
			}

			return rightAnswerCount;
		}

		/// <summary>
		/// Установить оценку по проценту правильных ответов
		/// </summary>
		/// <param name="percent"></param>
		private void SetGrade(double percent)
		{
			if (percent >= 0.85) currentPassedTest.Grade = 5;
			else if (percent >= 0.70) currentPassedTest.Grade = 4;
			else if (percent >= 0.50) currentPassedTest.Grade = 3;
			else
			{
				currentPassedTest.Grade = 2;
			}
		}
	}
}