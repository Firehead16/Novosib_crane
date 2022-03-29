using System;
using System.Collections.Generic;
using Core.Global.Network;
using Core.Testing;

public class TestLocalServerRequestHandler : LocalServerRequestHandler
{
	/// <summary>
	/// Сформировать ответ от базы данных на сервере
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public override Request GetHandlerResponse(Request request)
	{
		Request response = new Request(request.RequestType, new List<RequestParameter>())
		{
			Id = request.Id
		};

		switch (Enum.Parse(typeof(TestRequestType), request.RequestType))
		{
			case TestRequestType.AddComp:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddComp((Computer)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteComp:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteComp((Computer)request.Parameters[0].Value);
				break;
			case TestRequestType.IsCompExist:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().IsCompExist((string)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetComputerByComputerName:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetComputerByComputerName((string)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetComputerByComputerId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetComputerByComputerId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetComputers:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetComputers()) };
				break;
			case TestRequestType.SetTask:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().SetTask((string)request.Parameters[0].Value, (int)request.Parameters[1].Value);
				break;
			case TestRequestType.DeleteTask:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteTask((int)request.Parameters[0].Value);
				break;
			case TestRequestType.SetProgress:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().SetProgress((string)request.Parameters[0].Value, (int)request.Parameters[1].Value);
				break;
			case TestRequestType.DeleteProgress:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteProgress((int)request.Parameters[0].Value);
				break;
			case TestRequestType.OpenSession:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().OpenSession((Person)request.Parameters[0].Value, (Computer)request.Parameters[1].Value)) };
				break;
			case TestRequestType.CloseSessionBySession:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().CloseSession((Session)request.Parameters[0].Value);
				break;
			case TestRequestType.CloseSessionByComputer:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().CloseSession((Computer)request.Parameters[0].Value);
				break;
			case TestRequestType.GetSessions:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetSessions()) };
				break;
			case TestRequestType.GetSessionByComp:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetSession((Computer)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetSessionByPerson:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetSession((Person)request.Parameters[0].Value)) };
				break;
			case TestRequestType.IsSessionOpenByComp:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().IsSessionOpen((Computer)request.Parameters[0].Value)) };
				break;
			case TestRequestType.IsSessionOpenByPerson:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().IsSessionOpen((Person)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddPerson:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddPerson((Person)request.Parameters[0].Value);
				break;
			case TestRequestType.EditPerson:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().EditPerson((Person)request.Parameters[0].Value);
				break;
			case TestRequestType.DeletePerson:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeletePerson((Person)request.Parameters[0].Value);
				break;
			case TestRequestType.GetPersonByPersonId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetPersonByPersonId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetPerson:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetPerson((string)request.Parameters[0].Value, (string)request.Parameters[1].Value)) };
				break;
			case TestRequestType.GetPersons:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetPersons()) };
				break;
			case TestRequestType.IsPersonExistWithLoginAndPassword:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().IsPersonExist((string)request.Parameters[0].Value, (string)request.Parameters[1].Value)) };
				break;
			case TestRequestType.IsPersonExistWithLogin:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().IsPersonExist((string)request.Parameters[0].Value)) };
				break;
			case TestRequestType.IsPersonOpenSession:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().IsPersonOpenSession((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddAdministrator:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddAdministrator((Administrator)request.Parameters[0].Value);
				break;
			case TestRequestType.EditAdministrator:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().EditAdministrator((Administrator)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteAdministrator:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteAdministrator((Administrator)request.Parameters[0].Value);
				break;
			case TestRequestType.GetAdministratorByAdminId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetAdministratorByAdminId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetAdministratorByPersonId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetAdministratorByPersonId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetaAdministrators:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetAdministrators()) };
				break;
			case TestRequestType.AddTeacher:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddTeacher((Teacher)request.Parameters[0].Value);
				break;
			case TestRequestType.EditTeacher:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().EditTeacher((Teacher)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteTeacher:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteTeacher((Teacher)request.Parameters[0].Value);
				break;
			case TestRequestType.GetTeacherByTeacherId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetTeacherByTeacherId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetTeacherByPersonId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetTeacherByPersonId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetTeachers:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetTeachers()) };
				break;
			case TestRequestType.AddStudent:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddStudent((Student)request.Parameters[0].Value);
				break;
			case TestRequestType.EditStudent:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().EditStudent((Student)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteStudent:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteStudent((Student)request.Parameters[0].Value);
				break;
			case TestRequestType.GetStudentByStudentId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetStudentByStudentId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetStudentByPersonId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetStudentByPersonId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetStudentsByGroup:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetStudentsByGroup((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetStudents:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetStudents()) };
				break;
			case TestRequestType.AddGroup:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddGroup((Group)request.Parameters[0].Value);
				break;
			case TestRequestType.EditGroup:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().EditGroup((Group)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteGroup:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteGroup((Group)request.Parameters[0].Value);
				break;
			case TestRequestType.GetGroupByGroupId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetGroupByGroupId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetGroupsBySpeciality:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetGroupsBySpeciality((Speciality)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetGroups:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetGroups()) };
				break;
			case TestRequestType.AddSpeciality:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().AddSpeciality((Speciality)request.Parameters[0].Value);
				break;
			case TestRequestType.EditSpeciality:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().EditSpeciality((Speciality)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteSpeciality:
				LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteSpeciality((Speciality)request.Parameters[0].Value);
				break;
			case TestRequestType.GetSpecialityBySpecialityId:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetSpecialityBySpecialityId((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetSpecialities:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<IAuthorizationDatabaseRequest>().GetSpecialities()) };
				break;
			case TestRequestType.AddTest:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().AddTest((Test)request.Parameters[0].Value)) };
				break;
			case TestRequestType.EditTest:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EditTest((Test)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteTest:
				LocalConnection.GetRequest<ITestDatabaseRequest>().DeleteTest((Test)request.Parameters[0].Value);
				break;
			case TestRequestType.GetTest:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetTest((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetTests:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetTests()) };
				break;
			case TestRequestType.GetTestsByComplication:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetTests((Complication)request.Parameters[0].Value)) };
				break;
			case TestRequestType.CopyTest:
				LocalConnection.GetRequest<ITestDatabaseRequest>().CopyTest((Test)request.Parameters[0].Value, (int)request.Parameters[1].Value);
				break;
			case TestRequestType.StartTest:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().StartTest((int)request.Parameters[0].Value, (Test)request.Parameters[1].Value)) };
				break;
			case TestRequestType.EndTest:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EndTest((PassedTest)request.Parameters[0].Value, (bool)request.Parameters[1].Value);
				break;
			case TestRequestType.AddPassedTests:
				LocalConnection.GetRequest<ITestDatabaseRequest>().AddPassedTests((PassedTest)request.Parameters[0].Value);
				break;
			case TestRequestType.GetPassedTests:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetPassedTests((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetPassedTestLists:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetPassedTestLists((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetComplications:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetComplications()) };
				break;
			case TestRequestType.GetComplicationById:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetComplicationById((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddQuestion:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().AddQuestion((Question)request.Parameters[0].Value)) };
				break;
			case TestRequestType.EditQuestion:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EditQuestion((Question)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteQuestion:
				LocalConnection.GetRequest<ITestDatabaseRequest>().DeleteQuestion((Question)request.Parameters[0].Value);
				break;
			case TestRequestType.GetQuestionById:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetQuestionById((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetQuestions:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetQuestions((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetQuestionsIdList:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetQuestionsIdList((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetQuestionsTypeList:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetQuestionsTypeList((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetQuestionImage:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetQuestionImage((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddAnswerText:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().AddAnswerText((AnswerOfText)request.Parameters[0].Value)) };
				break;
			case TestRequestType.EditAnswerText:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EditAnswerText((AnswerOfText)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteAnswer:
				LocalConnection.GetRequest<ITestDatabaseRequest>().DeleteAnswer((AnswerOfText)request.Parameters[0].Value);
				break;
			case TestRequestType.GetAnswersText:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetAnswersText((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.GetRightAnswerText:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetRightAnswerText((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddAnswerOrderList:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().AddAnswerOrderList((AnswerOrderList)request.Parameters[0].Value)) };
				break;
			case TestRequestType.EditAnswerOrderList:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EditAnswerOrderList((AnswerOrderList)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteAnswerOrderList:
				LocalConnection.GetRequest<ITestDatabaseRequest>().DeleteAnswerOrderList((AnswerOrderList)request.Parameters[0].Value);
				break;
			case TestRequestType.GetOrderList:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetOrderList((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddAnswerImage:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().AddAnswerImage((AnswerImage)request.Parameters[0].Value)) };
				break;
			case TestRequestType.EditAnswerImage:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EditAnswerImage((AnswerImage)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteAnswerImage:
				LocalConnection.GetRequest<ITestDatabaseRequest>().DeleteAnswerImage((AnswerImage)request.Parameters[0].Value);
				break;
			case TestRequestType.GetImageList:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetImageList((int)request.Parameters[0].Value)) };
				break;
			case TestRequestType.AddAnswerDragNDrop:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().AddAnswerDragNDrop((AnswerDragNDrop)request.Parameters[0].Value)) };
				break;
			case TestRequestType.EditAnswerDragNDrop:
				LocalConnection.GetRequest<ITestDatabaseRequest>().EditAnswerDragNDrop((AnswerDragNDrop)request.Parameters[0].Value);
				break;
			case TestRequestType.DeleteAnswerDragNDrop:
				LocalConnection.GetRequest<ITestDatabaseRequest>().DeleteAnswerDragNDrop((AnswerDragNDrop)request.Parameters[0].Value);
				break;
			case TestRequestType.GetDragNDropList:
				response.Parameters = new List<RequestParameter> { new RequestParameter(LocalConnection.GetRequest<ITestDatabaseRequest>().GetDragNDropList((int)request.Parameters[0].Value)) };
				break;
			default:
				throw new Exception("Не найдена команда");
		}

		return response;
	}
}
