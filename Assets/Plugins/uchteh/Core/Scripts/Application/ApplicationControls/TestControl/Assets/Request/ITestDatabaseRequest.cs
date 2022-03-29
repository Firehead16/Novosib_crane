using System.Collections.Generic;
using Core.Global.Network;

namespace Core.Testing
{
	public interface ITestDatabaseRequest : IRequest
	{
		#region Работа со сложностями

		List<Complication> GetComplications();

		Complication GetComplicationById(int complicationId);

		#endregion

		#region Работа со списком тестов

		/// <summary>
		/// Добавить тест
		/// </summary>
		/// <param name="test">Тест</param>
		int AddTest(Test test);

		/// <summary>
		/// Редактировать тест
		/// </summary>
		/// <param name="test">Тест</param>
		 void EditTest(Test test);

		/// <summary>
		/// Удалить тест
		/// </summary>
		/// <param name="test">Тест</param>
		 void DeleteTest(Test test);

		/// <summary>
		/// Получить тест
		/// </summary>
		/// <param name="testId">Индекс теста</param>
		/// <returns></returns>
		 Test GetTest(int testId);

		/// <summary>
		/// Получить список тестов
		/// </summary>
		/// <returns></returns>
		 List<Test> GetTests();

		/// <summary>
		/// Получить список тестов по сложности
		/// </summary>
		/// <returns></returns>
		 List<Test> GetTests(Complication complication);

		/// <summary>
		/// Начало выполнения теста
		/// </summary>
		/// <param name="personId">Индекс пользователя</param>
		/// <param name="test">Индекс теста</param>
		/// <returns></returns>
		PassedTest StartTest(int personId, Test test);

		/// <summary>
		/// Окончание выполнения теста
		/// </summary>
		/// <param name="test">Пройденный тест</param>
		/// <param name="isProbe">Проба или зачет</param>
		 void EndTest(PassedTest test, bool isProbe);

		/// <summary>
		/// Копировать тест
		/// </summary>
		/// <param name="test"></param>
		/// <param name="complicationId"></param>
		void CopyTest(Test test, int complicationId);

		#endregion

		#region Работа с пройденными тестами

		/// <summary>
		/// Добавить пройденный тест
		/// </summary>
		/// <param name="test">Пройденный тест</param>
		 void AddPassedTests(PassedTest test);

		/// <summary>
		/// Получить пройденные тесты пользователя
		/// </summary>
		/// <param name="personId">Индекс пользователя</param>
		/// <returns></returns>
		 List<PassedTest> GetPassedTests(int personId);

		/// <summary>
		/// Получить пройденные тесты лога
		/// </summary>
		/// <param name="logId">Индекс лога</param>
		/// <returns></returns>
		 List<PassedQuestion> GetPassedTestLists(int logId);

		#endregion
		
		#region Работа с вопросами

		/// <summary>
		/// Добавить вопрос
		/// </summary>
		/// <param name="questionForm">Вопрос</param>
		/// <returns></returns>
		 int AddQuestion(Question questionForm);

		/// <summary>
		/// Редактировать вопрос
		/// </summary>
		/// <param name="questionForm">Вопрос</param>
		 void EditQuestion(Question questionForm);

		/// <summary>
		/// Удалить вопрос
		/// </summary>
		/// <param name="questionForm">Вопрос</param>
		 void DeleteQuestion(Question questionForm);

		/// <summary>
		/// Получить вопрос по индексу
		/// </summary>
		/// <param name="questionId">Индекс вопроса</param>
		/// <returns></returns>
		 Question GetQuestionById(int questionId);

		/// <summary>
		/// Получить список вопросов теста 
		/// </summary>
		/// <param name="testId">Индекс теста</param>
		/// <returns></returns>
		 List<Question> GetQuestions(int testId);

		/// <summary>
		///  Получить список индексов вопросов теста
		/// </summary>
		/// <param name="testId">Индекс теста</param>
		/// <returns></returns>
		 List<int> GetQuestionsIdList(int testId);

		/// <summary>
		/// Получить типы вопросов теста
		/// </summary>
		/// <param name="testId">Индекс теста</param>
		/// <returns></returns>
		 List<int> GetQuestionsTypeList(int testId);

		/// <summary>
		/// Получить изображение для вопроса
		/// </summary>
		/// <param name="questionId">Индекс вопроса</param>
		/// <returns></returns>
		 byte[] GetQuestionImage(int questionId);

		#endregion

		#region Работа над ответами с выбором варианта

		/// <summary>
		/// Добавить ответ с выбором варианта
		/// </summary>
		/// <param name="answer">Ответ</param>
		/// <returns></returns>
		 int AddAnswerText(AnswerOfText answer);

		/// <summary>
		/// Редактировать ответ с выбором варианта
		/// </summary>
		/// <param name="answer"></param>
		 void EditAnswerText(AnswerOfText answer);

		/// <summary>
		/// Удалить ответ с выбором варианта
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void DeleteAnswer(AnswerOfText answer);

		/// <summary>
		/// Получить ответы с выбором варианта у вопроса
		/// </summary>
		/// <param name="questionId">Индекс вопроса</param>
		/// <returns></returns>
		 List<AnswerOfText> GetAnswersText(int questionId);

		/// <summary>
		/// Получить правильный ответ с выбором варианта у вопроса
		/// </summary>
		/// <param name="questionId">Индекс вопроса</param>
		/// <returns></returns>
		 AnswerOfText GetRightAnswerText(int questionId);

		#endregion

		#region Работа над ответами с выбором порядка

		/// <summary>
		/// Добавить ответ с выбором порядка
		/// </summary>
		/// <param name="answer">Ответ</param>
		/// <returns></returns>
		 int AddAnswerOrderList(AnswerOrderList answer);

		/// <summary>
		/// Редактировать ответ с выбором порядка
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void EditAnswerOrderList(AnswerOrderList answer);

		/// <summary>
		/// Удалить ответ с выбором порядка
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void DeleteAnswerOrderList(AnswerOrderList answer);

		/// <summary>
		/// Получить список ответов с выбором порядка у вопроса
		/// </summary>
		/// <param name="pQuestionId">Индекс вопроса</param>
		/// <returns></returns>
		 List<AnswerOrderList> GetOrderList(int pQuestionId);

		#endregion

		#region Работа над ответами с выбором изображения

		/// <summary>
		/// Добавить ответ с выбором изображения
		/// </summary>
		/// <param name="answer">Ответ</param>
		/// <returns></returns>
		 int AddAnswerImage(AnswerImage answer);

		/// <summary>
		/// Редактировать ответ с выбором изображения
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void EditAnswerImage(AnswerImage answer);

		/// <summary>
		/// Удалить ответ с выбором изображения
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void DeleteAnswerImage(AnswerImage answer);

		/// <summary>
		/// Получить список ответов с выбором изображения у вопроса
		/// </summary>
		/// <param name="pQuestionId">Индекс вопроса</param>
		/// <returns></returns>
		 List<AnswerImage> GetImageList(int pQuestionId);

		#endregion

		#region Работа над ответами с выбором позиции в тексте

		/// <summary>
		/// Добавить ответ с выбором позиции в тексте
		/// </summary>
		/// <param name="answer">Ответ</param>
		/// <returns></returns>
		 int AddAnswerDragNDrop(AnswerDragNDrop answer);

		/// <summary>
		/// Редактировать ответ с выбором позиции в тексте
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void EditAnswerDragNDrop(AnswerDragNDrop answer);

		/// <summary>
		/// Удалить ответ с выбором позиции в тексте
		/// </summary>
		/// <param name="answer">Ответ</param>
		 void DeleteAnswerDragNDrop(AnswerDragNDrop answer);

		/// <summary>
		/// Получить список ответов с выбором позиции в тексте у вопроса
		/// </summary>
		/// <param name="pQuestionId">Идекс вопроса</param>
		/// <returns></returns>
		 List<AnswerDragNDrop> GetDragNDropList(int pQuestionId);

		#endregion

	}
}