using System.Collections.Generic;
using Core.Global.Network;
using Core.Testing;

public enum TestRequestType
{
	AddComp,
	DeleteComp,
	IsCompExist,
	GetComputerByComputerName,
	GetComputerByComputerId,
	GetComputers,
	SetTask,
	DeleteTask,
	SetProgress,
	DeleteProgress,
	OpenSession,
	GetSessions,
	GetSessionByComp,
	GetSessionByPerson,
	IsSessionOpenByComp,
	IsSessionOpenByPerson,
	AddPerson,
	EditPerson,
	DeletePerson,
	GetPerson,
	IsPersonExistWithLoginAndPassword,
	IsPersonExistWithLogin,
	IsPersonOpenSession,
	AddAdministrator,
	EditAdministrator,
	DeleteAdministrator,
	GetAdministratorByAdminId,
	GetAdministratorByPersonId,
	GetaAdministrators,
	AddTeacher,
	EditTeacher,
	DeleteTeacher,
	GetTeacherByTeacherId,
	GetTeacherByPersonId,
	GetTeachers,
	AddStudent,
	EditStudent,
	DeleteStudent,
	GetStudentByStudentId,
	GetStudentByPersonId,
	GetStudentsByGroup,
	GetStudents,
	AddGroup,
	EditGroup,
	DeleteGroup,
	GetGroupByGroupId,
	GetGroupsBySpeciality,
	GetGroups,
	AddSpeciality,
	EditSpeciality,
	DeleteSpeciality,
	GetSpecialityBySpecialityId,
	GetSpecialities,
	AddTest,
	EditTest,
	DeleteTest,
	GetTest,
	GetTests,
	StartTest,
	EndTest,
	AddPassedTests,
	GetPassedTests,
	GetPassedTestLists,
	AddQuestion,
	EditQuestion,
	DeleteQuestion,
	GetQuestionById,
	GetQuestions,
	GetQuestionsIdList,
	GetQuestionsTypeList,
	GetQuestionImage,
	AddAnswerText,
	EditAnswerText,
	DeleteAnswer,
	GetAnswersText,
	GetRightAnswerText,
	AddAnswerOrderList,
	EditAnswerOrderList,
	DeleteAnswerOrderList,
	GetOrderList,
	AddAnswerImage,
	EditAnswerImage,
	GetImageList,
	AddAnswerDragNDrop,
	EditAnswerDragNDrop,
	DeleteAnswerDragNDrop,
	GetDragNDropList,
	CloseSessionBySession,
	CloseSessionByComputer,
	GetComplications,
	GetComplicationById,
	GetTestsByComplication,
	CopyTest,
	GetPersonByPersonId,
	GetPersons,
	DeleteAnswerImage,
}

public sealed class TestLocalClientRequestHandler : LocalClientRequestHandler, IAuthorizationDatabaseRequest, ITestDatabaseRequest
{
	#region Работа с компьютерами

	public void AddComp(Computer computer)
	{
		computer.ComputerId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddComp.ToString(), new List<RequestParameter> { new RequestParameter(computer) }));
	}

	public void DeleteComp(Computer computer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteComp.ToString(), new List<RequestParameter> { new RequestParameter(computer) }));
	}

	public bool IsCompExist(string compName)
	{
		return LocalConnection.SendServerMessage<bool>(new Request(TestRequestType.IsCompExist.ToString(), new List<RequestParameter> { new RequestParameter(compName) }));
	}

	public Computer GetComputerByComputerName(string compName)
	{
		return LocalConnection.SendServerMessage<Computer>(new Request(TestRequestType.GetComputerByComputerName.ToString(), new List<RequestParameter> { new RequestParameter(compName) }));
	}

	public Computer GetComputerByComputerId(int computerId)
	{
		return LocalConnection.SendServerMessage<Computer>(new Request(TestRequestType.GetComputerByComputerId.ToString(), new List<RequestParameter> { new RequestParameter(computerId) }));
	}

	public List<Computer> GetComputers()
	{
		return LocalConnection.SendServerMessage<List<Computer>>(new Request(TestRequestType.GetComputers.ToString(), new List<RequestParameter>()));
	}

	#endregion

	#region Работа с сессиями

	public void SetTask(string taskName, int sessionId)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.SetTask.ToString(), new List<RequestParameter> { new RequestParameter(taskName), new RequestParameter(sessionId) }));
	}

	public void DeleteTask(int sessionId)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteTask.ToString(), new List<RequestParameter> { new RequestParameter(sessionId) }));
	}

	public void SetProgress(string progressName, int sessionId)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.SetProgress.ToString(), new List<RequestParameter> { new RequestParameter(progressName), new RequestParameter(sessionId) }));
	}

	public void DeleteProgress(int sessionId)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteProgress.ToString(), new List<RequestParameter> { new RequestParameter(sessionId) }));
	}

	public Session OpenSession(Person person, Computer computer)
	{
		return LocalConnection.SendServerMessage<Session>(new Request(TestRequestType.OpenSession.ToString(), new List<RequestParameter> { new RequestParameter(person), new RequestParameter(computer) }));
	}

	public void CloseSession(Session session)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.CloseSessionBySession.ToString(), new List<RequestParameter> { new RequestParameter(session) }));
	}

	public void CloseSession(Computer computer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.CloseSessionByComputer.ToString(), new List<RequestParameter> { new RequestParameter(computer) }));
	}

	public List<Session> GetSessions()
	{
		return LocalConnection.SendServerMessage<List<Session>>(new Request(TestRequestType.GetSessions.ToString(), new List<RequestParameter>()));
	}

	public Session GetSession(Computer computer)
	{
		return LocalConnection.SendServerMessage<Session>(new Request(TestRequestType.GetSessionByComp.ToString(), new List<RequestParameter> { new RequestParameter(computer) }));
	}

	public Session GetSession(Person person)
	{
		return LocalConnection.SendServerMessage<Session>(new Request(TestRequestType.GetSessionByPerson.ToString(), new List<RequestParameter> { new RequestParameter(person) }));
	}

	public bool IsSessionOpen(Computer computer)
	{
		return LocalConnection.SendServerMessage<bool>(new Request(TestRequestType.IsSessionOpenByComp.ToString(), new List<RequestParameter> { new RequestParameter(computer) }));
	}

	public bool IsSessionOpen(Person person)
	{
		return LocalConnection.SendServerMessage<bool>(new Request(TestRequestType.IsSessionOpenByPerson.ToString(), new List<RequestParameter> { new RequestParameter(person) }));
	}

	#endregion

	#region Работа с пользователями

	public void AddPerson(Person person)
	{
		person.PersonId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddPerson.ToString(), new List<RequestParameter> { new RequestParameter(person) }));
	}

	public void EditPerson(Person person)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditPerson.ToString(), new List<RequestParameter> { new RequestParameter(person) }));
	}

	public void DeletePerson(Person person)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeletePerson.ToString(), new List<RequestParameter> { new RequestParameter(person) }));
	}

	public Person GetPersonByPersonId(int personId)
	{
		return LocalConnection.SendServerMessage<Person>(new Request(TestRequestType.GetPersonByPersonId.ToString(), new List<RequestParameter> { new RequestParameter(personId) }));
	}

	public Person GetPerson(string login, string password)
	{
		return LocalConnection.SendServerMessage<Person>(new Request(TestRequestType.GetPerson.ToString(), new List<RequestParameter> { new RequestParameter(login), new RequestParameter(password) }));
	}

	public List<Person> GetPersons()
	{
		return LocalConnection.SendServerMessage<List<Person>>(new Request(TestRequestType.GetPersons.ToString(), new List<RequestParameter>()));
	}

	public bool IsPersonExist(string login, string password)
	{
		return LocalConnection.SendServerMessage<bool>(new Request(TestRequestType.IsPersonExistWithLoginAndPassword.ToString(), new List<RequestParameter> { new RequestParameter(login), new RequestParameter(password) }));
	}

	public bool IsPersonExist(string login)
	{
		return LocalConnection.SendServerMessage<bool>(new Request(TestRequestType.IsPersonExistWithLogin.ToString(), new List<RequestParameter> { new RequestParameter(login) }));
	}

	public bool IsPersonOpenSession(int personId)
	{
		return LocalConnection.SendServerMessage<bool>(new Request(TestRequestType.IsPersonOpenSession.ToString(), new List<RequestParameter> { new RequestParameter(personId) }));
	}

	#endregion

	#region Работа с администраторами 

	public void AddAdministrator(Administrator admin)
	{
		admin.AdminId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddAdministrator.ToString(), new List<RequestParameter> { new RequestParameter(admin) }));
	}

	public void EditAdministrator(Administrator administrator)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditAdministrator.ToString(), new List<RequestParameter> { new RequestParameter(administrator) }));
	}

	public void DeleteAdministrator(Administrator admin)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteAdministrator.ToString(), new List<RequestParameter> { new RequestParameter(admin) }));
	}

	public Administrator GetAdministratorByAdminId(int adminId)
	{
		return LocalConnection.SendServerMessage<Administrator>(new Request(TestRequestType.GetAdministratorByAdminId.ToString(), new List<RequestParameter> { new RequestParameter(adminId) }));
	}

	public Administrator GetAdministratorByPersonId(int personId)
	{
		return LocalConnection.SendServerMessage<Administrator>(new Request(TestRequestType.GetAdministratorByPersonId.ToString(), new List<RequestParameter> { new RequestParameter(personId) }));
	}

	public List<Administrator> GetAdministrators()
	{
		return LocalConnection.SendServerMessage<List<Administrator>>(new Request(TestRequestType.GetaAdministrators.ToString(), new List<RequestParameter>()));
	}

	#endregion

	#region Работа с преподавателями

	public void AddTeacher(Teacher teacher)
	{
		teacher.TeacherId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddTeacher.ToString(), new List<RequestParameter> { new RequestParameter(teacher) }));
	}

	public void EditTeacher(Teacher teacher)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditTeacher.ToString(), new List<RequestParameter> { new RequestParameter(teacher) }));
	}

	public void DeleteTeacher(Teacher teacher)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteTeacher.ToString(), new List<RequestParameter> { new RequestParameter(teacher) }));
	}

	public Teacher GetTeacherByTeacherId(int teacherId)
	{
		return LocalConnection.SendServerMessage<Teacher>(new Request(TestRequestType.GetTeacherByTeacherId.ToString(), new List<RequestParameter> { new RequestParameter(teacherId) }));
	}

	public Teacher GetTeacherByPersonId(int personId)
	{
		return LocalConnection.SendServerMessage<Teacher>(new Request(TestRequestType.GetTeacherByPersonId.ToString(), new List<RequestParameter> { new RequestParameter(personId) }));
	}

	public List<Teacher> GetTeachers()
	{
		return LocalConnection.SendServerMessage<List<Teacher>>(new Request(TestRequestType.GetTeachers.ToString(), new List<RequestParameter> ()));
	}

	#endregion

	#region Работа со студентами

	public void AddStudent(Student student)
	{
		student.StudentId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddStudent.ToString(), new List<RequestParameter> { new RequestParameter(student) }));
	}

	public void EditStudent(Student student)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditStudent.ToString(), new List<RequestParameter> { new RequestParameter(student) }));
	}

	public void DeleteStudent(Student student)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteStudent.ToString(), new List<RequestParameter> { new RequestParameter(student) }));
	}

	public Student GetStudentByStudentId(int studentId)
	{
		return LocalConnection.SendServerMessage<Student>(new Request(TestRequestType.GetStudentByStudentId.ToString(), new List<RequestParameter> { new RequestParameter(studentId) }));
	}

	public Student GetStudentByPersonId(int personId)
	{
		return LocalConnection.SendServerMessage<Student>(new Request(TestRequestType.GetStudentByPersonId.ToString(), new List<RequestParameter> { new RequestParameter(personId) }));
	}

	public List<Student> GetStudentsByGroup(int groupId)
	{
		return LocalConnection.SendServerMessage<List<Student>>(new Request(TestRequestType.GetStudentsByGroup.ToString(), new List<RequestParameter> { new RequestParameter(groupId) }));
	}

	public List<Student> GetStudents()
	{
		return LocalConnection.SendServerMessage<List<Student>>(new Request(TestRequestType.GetStudents.ToString(), new List<RequestParameter>()));
	}

	#endregion

	#region Работа с группами

	public void AddGroup(Group group)
	{
		group.GroupId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddGroup.ToString(), new List<RequestParameter> { new RequestParameter(group) }));
	}

	public void EditGroup(Group group)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditGroup.ToString(), new List<RequestParameter> { new RequestParameter(group) }));
	}

	public void DeleteGroup(Group group)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteGroup.ToString(), new List<RequestParameter> { new RequestParameter(group) }));
	}

	public Group GetGroupByGroupId(int? groupId)
	{
		return LocalConnection.SendServerMessage<Group>(new Request(TestRequestType.GetGroupByGroupId.ToString(), new List<RequestParameter> { new RequestParameter(groupId) }));
	}

	public List<Group> GetGroupsBySpeciality(Speciality speciality)
	{
		return LocalConnection.SendServerMessage<List<Group>>(new Request(TestRequestType.GetGroupsBySpeciality.ToString(), new List<RequestParameter> { new RequestParameter(speciality) }));
	}

	public List<Group> GetGroups()
	{
		return LocalConnection.SendServerMessage<List<Group>>(new Request(TestRequestType.GetGroups.ToString(), new List<RequestParameter>()));
	}

	#endregion

	#region Работа со специальностями

	public void AddSpeciality(Speciality speciality)
	{
		speciality.SpecialityId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddSpeciality.ToString(), new List<RequestParameter> { new RequestParameter(speciality) }));
	}

	public void EditSpeciality(Speciality speciality)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditSpeciality.ToString(), new List<RequestParameter> { new RequestParameter(speciality) }));
	}

	public void DeleteSpeciality(Speciality speciality)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteSpeciality.ToString(), new List<RequestParameter> { new RequestParameter(speciality) }));
	}

	public Speciality GetSpecialityBySpecialityId(int specialityId)
	{
		return LocalConnection.SendServerMessage<Speciality>(new Request(TestRequestType.GetSpecialityBySpecialityId.ToString(), new List<RequestParameter> { new RequestParameter(specialityId) }));
	}

	public List<Speciality> GetSpecialities()
	{
		return LocalConnection.SendServerMessage<List<Speciality>>(new Request(TestRequestType.GetSpecialities.ToString(), new List<RequestParameter>()));
	}

	#endregion

	#region Работа со сложностями

	public List<Complication> GetComplications()
	{
		return LocalConnection.SendServerMessage<List<Complication>>(new Request(TestRequestType.GetComplications.ToString(), new List<RequestParameter>()));
	}

	public Complication GetComplicationById(int complicationId)
	{
		return LocalConnection.SendServerMessage<Complication>(new Request(TestRequestType.GetComplicationById.ToString(), new List<RequestParameter> { new RequestParameter(complicationId) }));
	}

	#endregion

	#region Работа со списком тестов

	public int AddTest(Test test)
	{
		test.TestId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddTest.ToString(), new List<RequestParameter> { new RequestParameter(test) }));
		return test.TestId;
	}

	public void EditTest(Test test)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditTest.ToString(), new List<RequestParameter> { new RequestParameter(test) }));
	}

	public void DeleteTest(Test test)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteTest.ToString(), new List<RequestParameter> { new RequestParameter(test) }));
	}

	public Test GetTest(int testId)
	{
		return LocalConnection.SendServerMessage<Test>(new Request(TestRequestType.GetTest.ToString(), new List<RequestParameter> { new RequestParameter(testId) }));
	}

	public List<Test> GetTests()
	{
		return LocalConnection.SendServerMessage<List<Test>>(new Request(TestRequestType.GetTests.ToString(), new List<RequestParameter>()));
	}

	public List<Test> GetTests(Complication complication)
	{
		return LocalConnection.SendServerMessage<List<Test>>(new Request(TestRequestType.GetTestsByComplication.ToString(), new List<RequestParameter> { new RequestParameter(complication) }));
	}

	public void CopyTest(Test test, int complicationId)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.CopyTest.ToString(), new List<RequestParameter> { new RequestParameter(test), new RequestParameter(complicationId) }));
	}

	#endregion

	#region Работа с пройденными тестами

	public PassedTest StartTest(int personId, Test test)
	{
		return LocalConnection.SendServerMessage<PassedTest>(new Request(TestRequestType.StartTest.ToString(), new List<RequestParameter> { new RequestParameter(personId), new RequestParameter(test) }));
	}

	public void EndTest(PassedTest test, bool isProbe)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EndTest.ToString(), new List<RequestParameter> { new RequestParameter(test), new RequestParameter(isProbe) }));
	}

	/// <summary>
	/// Добавить пройденный тест
	/// </summary>
	/// <param name="test">Пройденный тест</param>
	public void AddPassedTests(PassedTest test)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.AddPassedTests.ToString(), new List<RequestParameter> { new RequestParameter(test) }));
	}

	/// <summary>
	/// Получить пройденные тесты пользователя
	/// </summary>
	/// <param name="studentId">Индекс пользователя</param>
	/// <returns></returns>
	public List<PassedTest> GetPassedTests(int studentId)
	{
		return LocalConnection.SendServerMessage<List<PassedTest>>(new Request(TestRequestType.GetPassedTests.ToString(), new List<RequestParameter> { new RequestParameter(studentId) }));
	}

	/// <summary>
	/// Получить пройденные тесты лога
	/// </summary>
	/// <param name="logId">Индекс лога</param>
	/// <returns></returns>
	public List<PassedQuestion> GetPassedTestLists(int logId)
	{
		return LocalConnection.SendServerMessage<List<PassedQuestion>>(new Request(TestRequestType.GetPassedTestLists.ToString(), new List<RequestParameter> { new RequestParameter(logId) }));
	}

	#endregion

	#region Работа с вопросами

	/// <summary>
	/// Добавить вопрос
	/// </summary>
	/// <param name="question">Вопрос</param>
	/// <returns></returns>
	public int AddQuestion(Question question)
	{
		question.QuestionId = LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddQuestion.ToString(), new List<RequestParameter> { new RequestParameter(question) }));
		return question.QuestionId;
	}

	/// <summary>
	/// Редактировать вопрос
	/// </summary>
	/// <param name="question">Вопрос</param>
	public void EditQuestion(Question question)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditQuestion.ToString(), new List<RequestParameter> { new RequestParameter(question) }));
	}

	/// <summary>
	/// Удалить вопрос
	/// </summary>
	/// <param name="question">Вопрос</param>
	public void DeleteQuestion(Question question)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteQuestion.ToString(), new List<RequestParameter> { new RequestParameter(question) }));
	}

	/// <summary>
	/// Получить вопрос по индексу
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public Question GetQuestionById(int questionId)
	{
		return LocalConnection.SendServerMessage<Question>(new Request(TestRequestType.GetQuestionById.ToString(), new List<RequestParameter> { new RequestParameter(questionId) }));
	}

	/// <summary>
	/// Получить список вопросов теста 
	/// </summary>
	/// <param name="testId">Индекс теста</param>
	/// <returns></returns>
	public List<Question> GetQuestions(int testId)
	{
		return LocalConnection.SendServerMessage<List<Question>>(new Request(TestRequestType.GetQuestions.ToString(), new List<RequestParameter> { new RequestParameter(testId) }));
	}

	/// <summary>
	/// Получить список индексов вопросов теста
	/// </summary>
	/// <param name="testId">Индекс теста</param>
	/// <returns></returns>
	public List<int> GetQuestionsIdList(int testId)
	{
		return LocalConnection.SendServerMessage<List<int>>(new Request(TestRequestType.GetQuestionsIdList.ToString(), new List<RequestParameter> { new RequestParameter(testId) }));
	}

	/// <summary>
	/// Получить типы вопросов теста
	/// </summary>
	/// <param name="testId">Индекс теста</param>
	/// <returns></returns>
	public List<int> GetQuestionsTypeList(int testId)
	{
		return LocalConnection.SendServerMessage<List<int>>(new Request(TestRequestType.GetQuestionsTypeList.ToString(), new List<RequestParameter> { new RequestParameter(testId) }));
	}

	/// <summary>
	/// Получить изображение для вопроса
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public byte[] GetQuestionImage(int questionId)
	{
		return LocalConnection.SendServerMessage<byte[]>(new Request(TestRequestType.GetQuestionImage.ToString(), new List<RequestParameter> { new RequestParameter(questionId) }));
	}

	#endregion

	#region Работа над ответами с выбором варианта

	/// <summary>
	/// Добавить ответ с выбором варианта
	/// </summary>
	/// <param name="answer">Ответ</param>
	/// <returns></returns>
	public int AddAnswerText(AnswerOfText answer)
	{
		return LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddAnswerText.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Редактировать ответ с выбором варианта
	/// </summary>
	/// <param name="answer"></param>
	public void EditAnswerText(AnswerOfText answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditAnswerText.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Удалить ответ с выбором варианта
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswer(AnswerOfText answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteAnswer.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Получить ответы с выбором варианта у вопроса
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public List<AnswerOfText> GetAnswersText(int questionId)
	{
		return LocalConnection.SendServerMessage<List<AnswerOfText>>(new Request(TestRequestType.GetAnswersText.ToString(), new List<RequestParameter> { new RequestParameter(questionId) }));
	}

	/// <summary>
	/// Получить правильный ответ с выбором варианта у вопроса
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public AnswerOfText GetRightAnswerText(int questionId)
	{
		return LocalConnection.SendServerMessage<AnswerOfText>(new Request(TestRequestType.GetRightAnswerText.ToString(), new List<RequestParameter> { new RequestParameter(questionId) }));
	}

	#endregion

	#region Работа над ответами с выбором порядка

	/// <summary>
	/// Добавить ответ с выбором порядка
	/// </summary>
	/// <param name="answer">Ответ</param>
	/// <returns></returns>
	public int AddAnswerOrderList(AnswerOrderList answer)
	{
		return LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddAnswerOrderList.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Редактировать ответ с выбором порядка
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void EditAnswerOrderList(AnswerOrderList answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditAnswerOrderList.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Удалить ответ с выбором порядка
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswerOrderList(AnswerOrderList answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteAnswerOrderList.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Получить список ответов с выбором порядка у вопроса
	/// </summary>
	/// <param name="pQuestionId">Индекс вопроса</param>
	/// <returns></returns>
	public List<AnswerOrderList> GetOrderList(int pQuestionId)
	{
		return LocalConnection.SendServerMessage<List<AnswerOrderList>>(new Request(TestRequestType.GetOrderList.ToString(), new List<RequestParameter> { new RequestParameter(pQuestionId) }));
	}

	#endregion

	#region Работа над ответами с выбором изображения

	/// <summary>
	/// Добавить ответ с выбором изображения
	/// </summary>
	/// <param name="answer">Ответ</param>
	/// <returns></returns>
	public int AddAnswerImage(AnswerImage answer)
	{
		return LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddAnswerImage.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Редактировать ответ с выбором изображения
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void EditAnswerImage(AnswerImage answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditAnswerImage.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Удалить ответ с выбором изображения
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswerImage(AnswerImage answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteAnswerImage.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Получить список ответов с выбором изображения у вопроса
	/// </summary>
	/// <param name="pQuestionId">Индекс вопроса</param>
	/// <returns></returns>
	public List<AnswerImage> GetImageList(int pQuestionId)
	{
		return LocalConnection.SendServerMessage<List<AnswerImage>>(new Request(TestRequestType.GetImageList.ToString(), new List<RequestParameter> { new RequestParameter(pQuestionId) }));
	}

	#endregion

	#region Работа над ответами с выбором позиции в тексте

	/// <summary>
	/// Добавить ответ с выбором позиции в тексте
	/// </summary>
	/// <param name="answer">Ответ</param>
	/// <returns></returns>
	public int AddAnswerDragNDrop(AnswerDragNDrop answer)
	{
		return LocalConnection.SendServerMessage<int>(new Request(TestRequestType.AddAnswerDragNDrop.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Редактировать ответ с выбором позиции в тексте
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void EditAnswerDragNDrop(AnswerDragNDrop answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.EditAnswerDragNDrop.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Удалить ответ с выбором позиции в тексте
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswerDragNDrop(AnswerDragNDrop answer)
	{
		LocalConnection.SendServerMessage(new Request(TestRequestType.DeleteAnswerDragNDrop.ToString(), new List<RequestParameter> { new RequestParameter(answer) }));
	}

	/// <summary>
	/// Получить список ответов с выбором позиции в тексте у вопроса
	/// </summary>
	/// <param name="pQuestionId">Идекс вопроса</param>
	/// <returns></returns>
	public List<AnswerDragNDrop> GetDragNDropList(int pQuestionId)
	{
		return LocalConnection.SendServerMessage<List<AnswerDragNDrop>>(new Request(TestRequestType.GetDragNDropList.ToString(), new List<RequestParameter> { new RequestParameter(pQuestionId) }));
	}

	#endregion
}