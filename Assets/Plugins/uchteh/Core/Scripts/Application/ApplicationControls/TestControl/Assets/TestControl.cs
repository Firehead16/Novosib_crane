using System;
using System.Collections.Generic;
using System.Linq;
using Core.Global.Authorization;
using Core.Extensions;
using Core.Global.Network;
using Core.Settings;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;
using Message = Core.Settings.Message;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum Testing
		{
			ShowTesting,
			StartTestResult,
			FinishTestResult,
			ShowChangeTest,
		}
	}
}


namespace Core.Testing
{
	/// <summary>
	/// Контроллер, для прохождения тестов
	/// </summary>
	public class TestControl : CanvasControl<ITestPanel>, ITestControl
	{
		public bool IsInitialized { get; set; }

		private ChooseTestPanel chooseTestPanel;
		private StartTestPanel startTestPanel;
		private TestingPanel testingPanel;
		private FinishTestPanel finishTestPanel;

		private ResultTestPanel resultTestPanel;
		private ResultTestExtPanel resultTestExtPanel;

		private TestAdministrationPanel testAdministrationPanel;

		private TestChangePanel testChangePanel;
		private TestCopyPanel testCopyPanel;
		private QuestionChangePanel questionChangePanel;

		private Test currentTest;
		private PassedTest passedTest;
		private bool isState;

		private static ClientNetworkControl networkControl;

		protected override void OnItemsInitialised()
		{
			networkControl = GlobalLinkStorage.Application.GetControl<ClientNetworkControl>();
		
			#region Панели прохождения тестов

			chooseTestPanel = GetControl<ChooseTestPanel>();
			startTestPanel = GetControl<StartTestPanel>();
			testingPanel = GetControl<TestingPanel>();
			finishTestPanel = GetControl<FinishTestPanel>();

			#region Подписки панели выбора теста

			chooseTestPanel.OnChooseTestingButtonClick += (isProbe, test) =>
			{
				if (test.Questions.Any())
				{
					currentTest = test;
					isState = isProbe;

					DisableControls();
					startTestPanel.Show(currentTest);
				}
				else
				{
					Debug.LogError("Нет вопросов в тесте");
				}
			};
			chooseTestPanel.OnExitClick += DisableControls;

			#endregion

			#region Подписки панели начала прохождения теста

			startTestPanel.OnStartButtonClick += () =>
			{
				DisableControls();

				passedTest = StartTest(AuthorizationControl.Session.Person.PersonId, currentTest);
				testingPanel.Show(passedTest, isState);
			};

			#endregion

			#region Подписки панели прохождение теста
			testingPanel.OnSetProgress += (passedQuestions, allQuestions) =>
			{
				string progress = $"Завершено {passedQuestions} из {allQuestions}";
				Send(null, new Message(Messages.Autorization.SetProgress, progress));
			};
			testingPanel.OnTestResult += () =>
			{
				DisableControls();

				EndPassedTest(passedTest, isState);
				finishTestPanel.Show(passedTest);
			};

			#endregion

			#region Подписки панели окончания теста

			finishTestPanel.OnDeleteProgress += () =>
			{
				Send(null, new Message(Messages.Autorization.DeleteProgress));
			};
			finishTestPanel.OnExitButtonClick += DisableControls;

			#endregion

			#endregion

			#region Панели отображения результатов тестов

			resultTestPanel = GetControl<ResultTestPanel>();
			resultTestExtPanel = GetControl<ResultTestExtPanel>();

			#region Подписки панели результатов

			resultTestPanel.OnLogExtButtonClick += ShowExtResult;
			resultTestPanel.OnFinishTestResultButtonClick += FinishTestResult;
			resultTestPanel.OnPrintButtonClick += PrintResult;

			#endregion

			#region Подписки дополнительной панели результатов

			resultTestExtPanel.OnResultTextExtButtonClick += HideExtResult;

			#endregion

			#endregion

			#region Панели администрирования тестов

			testAdministrationPanel = GetControl<TestAdministrationPanel>();
			testChangePanel = GetControl<TestChangePanel>();
			testCopyPanel = GetControl<TestCopyPanel>();
			questionChangePanel = GetControl<QuestionChangePanel>();

			#region Подписки панели администрирования тестов

			testAdministrationPanel.OnChangeTestButtonClick += (test, isNew) =>
			{
				currentTest = test;
				isState = isNew;

				if (isNew)
				{
					AddTest(test);
				}
				else
				{
					test.Questions = GetQuestions(test.TestId);
				}

				testChangePanel.Show(test, isNew);
			};
			testAdministrationPanel.OnDeleteTestButtonClick += DeleteTest;
			testAdministrationPanel.OnCopyTestButtonClick += (test) =>
			{
				testAdministrationPanel.Hide();

				List<Complication> complications = GetComplications();
				testCopyPanel.Show(test, complications);
			};

			#endregion

			#region Подписки панели копирования тестов

			testCopyPanel.OnCopyTestButtonClick += CopyTest;
			testCopyPanel.OnMenuHide += () => testAdministrationPanel.Show();
			
			#endregion

			#region Подписки панели изменения теста

			testChangePanel.OnDeleteTest += DeleteTest;
			testChangePanel.OnMenuHide += () => testAdministrationPanel.Show(); 
			testChangePanel.OnSaveButtonClick += EditTest;
			testChangePanel.OnChangeQuestionButtonClick += (question, isNew) =>
			{
				questionChangePanel.Show(question, isNew);
			};
			testChangePanel.OnDeleteQuestionButtonClick += question =>
			{
				DeleteQuestion(question);
				currentTest.Questions.Remove(question);
				testChangePanel.Show(currentTest,isState);
			};
			

			#endregion

			#region Подписки панели изменения вопроса

			questionChangePanel.OnMenuHide += () =>
			{
				currentTest.Questions = GetQuestions(currentTest.TestId);
				testChangePanel.Show(currentTest, isState);
			};

			#endregion

			#endregion
		}
		
		public override void Notify(Message message)
		{
			switch (message.Type)
			{
				case Messages.Testing.ShowTesting:
					List<Test> tests = GetTests();
					chooseTestPanel.Show(tests);
					break;
				case Messages.Testing.StartTestResult:
					StartTestResult();
					break;
				case Messages.Testing.FinishTestResult:
					break;

				case Messages.Testing.ShowChangeTest:
					testAdministrationPanel.Show();
					break;
			}
		}
		
		#region Методы для отображения результатов тестов

		/// <summary>
		/// Начато отображение результатов
		/// </summary>
		private void StartTestResult()
		{
			DisableControls();
			resultTestPanel.Show();
		}

		/// <summary>
		/// Закончено отображение результатов
		/// </summary>
		private void FinishTestResult()
		{
			DisableControls();
			Send(null, new Message(Messages.Testing.FinishTestResult));
		}

		/// <summary>
		/// Отобразить дополнительную информацию результата теста
		/// </summary>
		/// <param name="resultPassedTest"></param>
		private void ShowExtResult(PassedTest resultPassedTest)
		{
			DisableControls();
			resultTestExtPanel.Show(resultPassedTest);
		}

		/// <summary>
		/// Спрятать дополнительную информацию результата теста
		/// </summary>
		private void HideExtResult()
		{
			DisableControls();
			resultTestPanel.Show();
		}
		
		/// <summary>
		/// Печать результата теста
		/// </summary>
		/// <param name="printPassedTest"></param>
		private void PrintResult(PassedTest printPassedTest)
		{
			string documentName = "Report" + DateTime.Now;

			Person person = AuthorizationControl.Session.Person;
			string title = "Отчет о тестировании:\n\n"
						   + "Номер лога: " + printPassedTest.LogId + "\n"
						   + "Пользователь: " + person.Surname + " " + person.Name + " " + person.Patronymic + " " + "\n"
						   + "Проходимый тест: " + printPassedTest.Name + "\n"
						   + "Дата прохождения тестирования: " + printPassedTest.Date + "\n"
						   + "Полученная оценка: " + printPassedTest.Grade + "\n";

			string text = "";
			foreach (var question in printPassedTest.PassedQuestions)
			{
				text += "\n"
							 + "Номер вопроса: " + question.QuestionId + "\n"
							 + "Название вопроса:" + printPassedTest.Questions.Single(q => q.QuestionId == question.QuestionId).Name + "\n"
							 + "Ответ пользователя: " + question.Answer + "\n"
							 + "Правильный ответ:" + (question.IsRight ? "Да" : "Нет");
			}

			//Printer.Print(documentName, title, text);
		}
		
		#endregion

		#region Статические методы


		/// <summary>
		/// Перемешивает лист 
		/// </summary>
		/// <param name="tempList"></param>
		public static List<AnswerOrderList> ShuffleOrderList(List<AnswerOrderList> tempList)
		{
			for (int i = 0; i < tempList.Count; i++)
			{
				AnswerOrderList temp = tempList[i];
				int randomIndex = UnityEngine.Random.Range(i, tempList.Count);
				tempList[i] = tempList[randomIndex];
				tempList[randomIndex] = temp;
			}
			return tempList;
		}

		/// <summary>
		/// Создать изображение
		/// </summary>
		/// <param name="targetImage"></param>
		/// <param name="byteImage"></param>
		public static void FillImage(Image targetImage, byte[] byteImage)
		{
			var fillTexture = new Texture2D(1, 1);//создаем текстуру, которой будем заполнять спрайт изображения. В конструкторе вроде бы могут быть любые параметры (ширина и высота)
			fillTexture.LoadImage(byteImage); //заполняем текстуру
			targetImage.GetComponent<RectTransform>().SetWidth(fillTexture.width);
			targetImage.GetComponent<RectTransform>().SetHeight(fillTexture.height);
			targetImage.sprite = Sprite.Create(fillTexture, new Rect(0, 0, fillTexture.width, fillTexture.height), new Vector2());
		}

		/// <summary>
		/// Создать изображение
		/// </summary>
		/// <param name="byteImage"></param>
		/// <returns></returns>
		public static Sprite FillAnswerImageSprite(byte[] byteImage)
		{
			var fillTexture = new Texture2D(1, 1); //создаем текстуру, которой будем заполнять спрайт изображения. В конструкторе вроде бы могут быть любые параметры (ширина и высота)
			fillTexture.LoadImage(byteImage); //заполняем текстуру
			return Sprite.Create(fillTexture, new Rect(0, 0, fillTexture.width, fillTexture.height), new Vector2());
		}


		public static void AddTest(Test test)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().AddTest(test);
		}
		
		public static void EditTest(Test test)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().EditTest(test);
		}

		public static void DeleteTest(Test test)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().DeleteTest(test);
		}

		public static void CopyTest(Test test, int complicationId)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().CopyTest(test, complicationId);
		}

		public static List<Test> GetTests(Complication complication)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetTests(complication);
		}

		public static List<Test> GetTests()
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetTests();
		}


		public static PassedTest StartTest(int personId, Test test)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().StartTest(personId, test);
		}

		public static void EndPassedTest(PassedTest test, bool isProbe)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().EndTest(test, isProbe);
		}


		public static int AddQuestion(Question question)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().AddQuestion(question);
		}

		public static void DeleteQuestion(Question question)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().DeleteQuestion(question);
		}

		public static List<Question> GetQuestions(int testId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetQuestions(testId);
		}

		public static Question GetQuestionById(int questionId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetQuestionById(questionId);
		}


		public static List<PassedTest> GetPassedTests(int personId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetPassedTests(personId);
		}

		public static List<Complication> GetComplications()
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetComplications();
		}

		public static List<Person> GetPersons()
		{
			return networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().GetPersons();
		}


		public static void AddAnswerOrderList(AnswerOrderList answerOrderList)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().AddAnswerOrderList(answerOrderList);
		}

		public static void EditAnswerOrderList(AnswerOrderList answerOrderList)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().EditAnswerOrderList(answerOrderList);
		}

		public static List<AnswerOrderList> GetOrderList(int questionId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetOrderList(questionId);
		}


		public static void AddAnswerText(AnswerOfText answerOfText)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().AddAnswerText(answerOfText);
		}

		public static void EditAnswerText(AnswerOfText answerOfText)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().EditAnswerText(answerOfText);
		}

		public static List<AnswerOfText> GetAnswersText(int questionId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetAnswersText(questionId);
		}
		public static AnswerOfText GetRightAnswerText(int questionId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetRightAnswerText(questionId);
		}


		public static void AddAnswerImage(AnswerImage answerImage)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().AddAnswerImage(answerImage);
		}

		public static void EditAnswerImage(AnswerImage answerImage)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().EditAnswerImage(answerImage);
		}

		public static List<AnswerImage> GetImageList(int questionId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetImageList(questionId);
		}

		public static byte[] GetQuestionImage(int questionId)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetQuestionImage(questionId);
		}


		public static void AddAnswerDragNDrop(AnswerDragNDrop answerDragNDrop)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().AddAnswerDragNDrop(answerDragNDrop);
		}

		public static void EditAnswerDragNDrop(AnswerDragNDrop answerDragNDrop)
		{
			networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().EditAnswerDragNDrop(answerDragNDrop);
		}

		public static List<AnswerDragNDrop> GetDragNDropList(int answerDragNDrop)
		{
			return networkControl.CurrentConnection.GetRequest<ITestDatabaseRequest>().GetDragNDropList(answerDragNDrop);
		}

		#endregion

	}

}

