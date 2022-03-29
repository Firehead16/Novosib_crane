using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Core.Global.Network;
using Core.Testing;
using UnityEngine;

public sealed class SqliteDatabaseRequest : BaseControlMethods, IAuthorizationDatabaseRequest, ITestDatabaseRequest, IHostRequest
{
	public Database Database { get; set; }

	public override void Load()
	{
		Database = GetComponent<Database>();
	}

	public override void Unload()
	{
	}

	#region Работа с компьютерами

	public void AddComp(Computer computer)
	{
		computer.ComputerId = Database.SendExecuteScalarWithLastId($"INSERT INTO Computers (Name, IPAddress) VALUES ('{computer.Name}','{computer.IpAddress}');");
	}

	public void DeleteComp(Computer computer)
	{
		if (IsCompExist(computer.Name))
		{
			Database.SendExecuteNonQuery($"DELETE FROM Computers WHERE ComputerId = '{computer.ComputerId}'");
		}
	}

	public bool IsCompExist(string compName)
	{
		bool response = false;

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Computers WHERE Name = '{compName}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				response = reader.HasRows;
				reader.Close();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return response;
	}

	public Computer GetComputerByComputerName(string compName)
	{
		Computer computer = new Computer();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Computers WHERE Name = '{compName}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						computer.ComputerId = reader.GetInt32(0);
						computer.Name = reader["Name"].ToString();
						computer.IpAddress = reader["IPAddress"].ToString();
					}
				}
				reader.Close();
				transaction.Commit();

			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return computer;
	}

	public Computer GetComputerByComputerId(int computerId)
	{
		Computer computer = new Computer();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Computers WHERE ComputerId = '{computerId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						computer.ComputerId = reader.GetInt32(0);
						computer.Name = reader["Name"].ToString();
						computer.IpAddress = reader["IPAddress"].ToString();
					}
				}
				reader.Close();
				transaction.Commit();

			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return computer;
	}

	public List<Computer> GetComputers()
	{
		List<Computer> computers = new List<Computer>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM Computers";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						computers.Add(new Computer()
						{
							ComputerId = reader.GetInt32(0),
							Name = reader["Name"].ToString(),
							IpAddress = reader["IPAddress"].ToString(),
						});
					}
				}
				reader.Close();
				transaction.Commit();

			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return computers;
	}

	#endregion

	#region Работа с сессиями

	public void SetTask(string taskName, int sessionId)
	{
		Database.SendExecuteNonQuery($"UPDATE Sessions SET Task = '{taskName}' WHERE SessionId = '{sessionId}'");
	}

	public void DeleteTask(int sessionId)
	{
		Database.SendExecuteNonQuery($"UPDATE Sessions SET Task = NULL WHERE SessionId = '{sessionId}'");
	}

	public void SetProgress(string progressName, int sessionId)
	{
		Database.SendExecuteNonQuery($"UPDATE Sessions SET Progress = '{progressName}' WHERE SessionId = '{sessionId}'");
	}

	public void DeleteProgress(int sessionId)
	{
		Database.SendExecuteNonQuery($"UPDATE Sessions SET Progress = NULL WHERE SessionId = '{sessionId}'");
	}

	public Session OpenSession(Person person, Computer computer)
	{
		Session session = new Session
		{
			SessionId = Database.SendExecuteScalarWithLastId($"INSERT INTO Sessions (PersonId, ComputerId) VALUES ('{person.PersonId}', '{computer.ComputerId}');"),
			Person = person,
			Computer = computer,
			Task = "",
			Progress = "",
			PersonId = person.PersonId,
			ComputerId = computer.ComputerId
		};

		return session;
	}

	public void CloseSession(Session session)
	{
		Database.SendExecuteNonQuery($"DELETE FROM Sessions WHERE SessionId = '{session.SessionId}'");
	}

	public void CloseSession(Computer computer)
	{
		Database.SendExecuteNonQuery($"DELETE FROM Sessions WHERE ComputerId = '{computer.ComputerId}'");
	}

	public List<Session> GetSessions()
	{
		List<Session> sessions = new List<Session>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM Sessions";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						sessions.Add(new Session()
						{
							SessionId = reader.GetInt32(0),
							Task = reader["Task"].ToString(),
							Progress = reader["Progress"].ToString(),
							PersonId = reader.GetInt32(3),
							ComputerId = reader.GetInt32(4),
						});
					}
				}

				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var session in sessions)
		{
			session.Person = GetPersonByPersonId(session.PersonId);
			session.Computer = GetComputerByComputerId(session.ComputerId);
		}

		return sessions;
	}

	public Session GetSession(Computer computer)
	{
		Session session = new Session();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Sessions WHERE ComputerId = '{computer.ComputerId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{

						session.SessionId = reader.GetInt32(0);
						session.Task = reader["Task"].ToString();
						session.Progress = reader["Progress"].ToString();
						session.PersonId = reader.GetInt32(3);
						session.Computer = computer;
						session.ComputerId = computer.ComputerId;
					}
				}

				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		session.Person = GetPersonByPersonId(session.PersonId);


		return session;
	}

	public Session GetSession(Person person)
	{
		Session session = new Session();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Sessions WHERE PersonId = '{person.PersonId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{

						session.SessionId = reader.GetInt32(0);
						session.Task = reader["Task"].ToString();
						session.Progress = reader["Progress"].ToString();
						session.Person = person;
						session.PersonId = person.PersonId;
						session.ComputerId = reader.GetInt32(4);
					}
				}

				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		session.Computer = GetComputerByComputerId(session.ComputerId);

		return session;
	}

	public bool IsSessionOpen(Computer computer)
	{
		bool response = false;

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Sessions WHERE ComputerId = '{computer.ComputerId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				response = reader.HasRows;
				reader.Close();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return response;
	}

	public bool IsSessionOpen(Person person)
	{
		bool response = false;

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Sessions WHERE PersonId = '{person.PersonId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				response = reader.HasRows;
				reader.Close();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return response;
	}

	#endregion

	#region Работа с пользователями

	public void AddPerson(Person person)
	{
		person.PersonId = Database.SendExecuteScalarWithLastId($"INSERT INTO Persons (Login, Password, Name, Surname, Patronymic, RoleId, IsActive) VALUES ('{person.Login}','{person.Password}','{person.Name}','{person.Surname}','{person.Patronymic}','{(int)person.Role}', '{Convert.ToInt32(person.IsActive)}');");
	}

	public void EditPerson(Person person)
	{
		Database.SendExecuteNonQuery($"UPDATE Persons SET Login = '{person.Login}', Password = '{person.Password}', Surname = '{person.Surname}', Name = '{person.Name}', Patronymic = '{person.Patronymic}', IsActive = '{Convert.ToInt32(person.IsActive)}'  WHERE PersonId = '{person.PersonId}'");
	}

	public void DeletePerson(Person person)
	{
		Database.SendExecuteNonQuery($"DELETE FROM Persons WHERE PersonId = '{person.PersonId}'");
	}

	public Person GetPersonByPersonId(int personId)
	{
		List<Person> users = new List<Person>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Persons WHERE PersonId = '{personId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				users = GetPersonsFromReader(reader);
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return users.FirstOrDefault();
	}

	public Person GetPerson(string login, string password)
	{
		Person person = new Person();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteCommand cmd = connection.CreateCommand();
			SQLiteTransaction transaction = connection.BeginTransaction();

			cmd.CommandText = $"SELECT * FROM Persons WHERE Login = '{login}' AND Password = '{password}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				person = GetPersonsFromReader(reader).FirstOrDefault();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();

			return person;
		}
	}

	public List<Person> GetPersons()
	{
		List<Person> persons = new List<Person>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM Persons";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				persons = GetPersonsFromReader(reader);

				transaction.Commit();

			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return persons;
	}

	private List<Person> GetPersonsFromReader(SQLiteDataReader reader)
	{
		List<Person> users = new List<Person>();

		if (reader.HasRows)
		{
			while (reader.Read())
			{

				try
				{
					users.Add(new Person()
					{
						PersonId = reader.GetInt32(0),
						Login = reader["Login"].ToString(),
						Password = reader["Password"].ToString(),
						Name = reader["Name"].ToString(),
						Surname = reader["Surname"].ToString(),
						Patronymic = reader["Patronymic"].ToString(),
						Role = (Role)reader.GetInt32(6),
						IsActive = Convert.ToBoolean(reader.GetValue(7)),
					});
				}
				catch (Exception exc)
				{
					Debug.LogError(exc.Message);
				}

			}
		}
		reader.Close();

		return users;
	}

	public bool IsPersonExist(string login, string password)
	{
		bool response = false;

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Persons WHERE Login = '{login}' AND Password = '{password}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				response = reader.HasRows;
				reader.Close();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return response;
	}

	public bool IsPersonExist(string login)
	{
		bool response = false;

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Persons WHERE Login = '{login}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				response = reader.HasRows;
				reader.Close();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return response;
	}

	public bool IsPersonOpenSession(int personId)
	{
		bool response = false;
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Sessions WHERE PersonId = '{personId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				response = reader.HasRows;
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}
		}
		return response;
	}

	#endregion

	#region Работа с администраторами 

	public void AddAdministrator(Administrator admin)
	{
		AddPerson(admin.Person);
		admin.AdminId = Database.SendExecuteScalarWithLastId($"INSERT INTO Administrators (PersonId) VALUES ('{admin.Person.PersonId}');");
	}

	public void EditAdministrator(Administrator administrator)
	{
		EditPerson(administrator.Person);
	}

	public void DeleteAdministrator(Administrator admin)
	{
		DeletePerson(admin.Person);
	}

	public Administrator GetAdministratorByAdminId(int adminId)
	{
		List<Administrator> admins = new List<Administrator>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Administrators WHERE AdministratorId = '{adminId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						admins.Add(new Administrator()
						{
							AdminId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var admin in admins)
		{
			admin.Person = GetPersonByPersonId(admin.PersonId);
		}

		return admins.FirstOrDefault();
	}

	public Administrator GetAdministratorByPersonId(int personId)
	{
		List<Administrator> admins = new List<Administrator>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Administrators WHERE PersonId = '{personId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						admins.Add(new Administrator()
						{
							AdminId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var admin in admins)
		{
			admin.Person = GetPersonByPersonId(admin.PersonId);
		}

		return admins.FirstOrDefault();
	}

	public List<Administrator> GetAdministrators()
	{
		List<Administrator> admins = new List<Administrator>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Administrators";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						admins.Add(new Administrator()
						{
							AdminId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var admin in admins)
		{
			admin.Person = GetPersonByPersonId(admin.PersonId);
		}

		return admins;
	}

	#endregion

	#region Работа с преподавателями

	public void AddTeacher(Teacher teacher)
	{
		AddPerson(teacher.Person);
		teacher.TeacherId = Database.SendExecuteScalarWithLastId($"INSERT INTO Teachers (PersonId) VALUES ('{teacher.Person.PersonId}');");
	}

	public void EditTeacher(Teacher teacher)
	{
		EditPerson(teacher.Person);
	}

	public void DeleteTeacher(Teacher teacher)
	{
		DeletePerson(teacher.Person);
	}

	public Teacher GetTeacherByTeacherId(int teacherId)
	{
		List<Teacher> teachers = new List<Teacher>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Teachers WHERE TeacherId = '{teacherId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						teachers.Add(new Teacher()
						{
							TeacherId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var teacher in teachers)
		{
			teacher.Person = GetPersonByPersonId(teacher.PersonId);
		}

		return teachers.FirstOrDefault();
	}

	public Teacher GetTeacherByPersonId(int personId)
	{
		List<Teacher> teachers = new List<Teacher>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Teachers WHERE PersonId = '{personId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						teachers.Add(new Teacher()
						{
							TeacherId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var teacher in teachers)
		{
			teacher.Person = GetPersonByPersonId(teacher.PersonId);
		}

		return teachers.FirstOrDefault();
	}

	public List<Teacher> GetTeachers()
	{
		List<Teacher> teachers = new List<Teacher>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Teachers";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						teachers.Add(new Teacher()
						{
							TeacherId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var teacher in teachers)
		{
			teacher.Person = GetPersonByPersonId(teacher.PersonId);
		}

		return teachers;
	}

	#endregion

	#region Работа со студентами

	public void AddStudent(Student student)
	{
		AddPerson(student.Person);
		student.StudentId = Database.SendExecuteScalarWithLastId($"INSERT INTO Students (PersonId) VALUES ('{student.Person.PersonId}');");
	}

	public void EditStudent(Student student)
	{
		EditPerson(student.Person);
	}

	public void DeleteStudent(Student student)
	{
		DeletePerson(student.Person);
	}

	public Student GetStudentByStudentId(int studentId)
	{
		List<Student> students = new List<Student>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Students WHERE StudentId = '{studentId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						students.Add(new Student()
						{
							StudentId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var student in students)
		{
			student.Person = GetPersonByPersonId(student.PersonId);
		}

		return students.FirstOrDefault();
	}

	public Student GetStudentByPersonId(int personId)
	{
		List<Student> students = new List<Student>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Students WHERE PersonId = '{personId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						students.Add(new Student()
						{
							StudentId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var student in students)
		{
			student.Person = GetPersonByPersonId(student.PersonId);
		}

		return students.FirstOrDefault();
	}

	public List<Student> GetStudentsByGroup(int groupId)
	{
		List<Student> students = new List<Student>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Students WHERE GroupId = '{groupId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						students.Add(new Student()
						{
							StudentId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var student in students)
		{
			student.Person = GetPersonByPersonId(student.PersonId);
		}

		return students;
	}

	public List<Student> GetStudents()
	{
		List<Student> students = new List<Student>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Students";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{

						students.Add(new Student()
						{
							StudentId = reader.GetInt32(0),
							PersonId = reader.GetInt32(1),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var student in students)
		{
			student.Person = GetPersonByPersonId(student.PersonId);
		}

		return students;
	}

	#endregion

	#region Работа с группами

	public void AddGroup(Group group)
	{
		group.GroupId = Database.SendExecuteScalarWithLastId($"INSERT INTO StudentGroups (Name, SpecialtyId) VALUES ('{group.Name}','{group.Speciality.SpecialityId}');");
	}

	public void EditGroup(Group group)
	{
		Database.SendExecuteNonQuery($"UPDATE StudentGroups SET Name = '{group.Name}', SpecialtyId = '{group.Speciality.SpecialityId}'  WHERE GroupId = '{group.GroupId}'");
	}

	public void DeleteGroup(Group group)
	{
		Database.SendExecuteNonQuery($"DELETE FROM StudentGroups WHERE GroupId = '{group.GroupId}'");
	}

	public Group GetGroupByGroupId(int? groupId)
	{
		if (groupId == null)
		{
			return null;
		}

		List<Group> groups = new List<Group>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM StudentGroups WHERE GroupId = '{groupId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						groups.Add(new Group()
						{
							GroupId = reader.GetInt32(0),
							Name = reader["Name"].ToString(),
							SpecialityId = reader.GetInt32(2),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var group in groups)
		{
			group.Speciality = GetSpecialityBySpecialityId(group.SpecialityId);
		}

		return groups.FirstOrDefault();
	}

	public List<Group> GetGroupsBySpeciality(Speciality speciality)
	{
		List<Group> groups = new List<Group>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM StudentGroups WHERE SpecialtyId = '{speciality.SpecialityId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						groups.Add(new Group()
						{
							GroupId = reader.GetInt32(0),
							Name = reader["Name"].ToString(),
							Speciality = speciality,
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return groups;
	}

	public List<Group> GetGroups()
	{
		List<Group> groups = new List<Group>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM StudentGroups";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						groups.Add(new Group()
						{
							GroupId = reader.GetInt32(0),
							Name = reader["Name"].ToString(),
							SpecialityId = reader.GetInt32(2),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var group in groups)
		{
			group.Speciality = GetSpecialityBySpecialityId(group.SpecialityId);
		}

		return groups;
	}

	#endregion

	#region Работа со специальностями

	public void AddSpeciality(Speciality speciality)
	{
		speciality.SpecialityId = Database.SendExecuteScalarWithLastId($"INSERT INTO Specialities (Name) VALUES ('{speciality.Name}');");
	}

	public void EditSpeciality(Speciality speciality)
	{
		Database.SendExecuteNonQuery($"UPDATE Specialities SET Name = '{speciality.Name}' WHERE SpecialityId = '{speciality.SpecialityId}'");
	}

	public void DeleteSpeciality(Speciality speciality)
	{
		Database.SendExecuteNonQuery($"DELETE FROM Specialities WHERE SpecialityId = '{speciality.SpecialityId}'");
	}

	public Speciality GetSpecialityBySpecialityId(int specialityId)
	{
		List<Speciality> specialities = new List<Speciality>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Specialities WHERE SpecialityId = '{specialityId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						specialities.Add(new Speciality()
						{
							SpecialityId = reader.GetInt32(0),
							Name = reader["Name"].ToString(),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return specialities.FirstOrDefault();
	}

	public List<Speciality> GetSpecialities()
	{
		List<Speciality> specialities = new List<Speciality>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM Specialities";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						specialities.Add(new Speciality()
						{
							SpecialityId = reader.GetInt32(0),
							Name = reader["Name"].ToString(),
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return specialities;
	}

	#endregion

	#region Работа со сложностями

	public List<Complication> GetComplications()
	{
		List<Complication> complications = new List<Complication>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM Complications";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						complications.Add(new Complication()
						{
							ComplicationId = reader.GetInt32(0),
							Name = reader.GetString(1)
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return complications;
	}

	public Complication GetComplicationById(int complicationId)
	{
		return GetComplications().Single(c => c.ComplicationId == complicationId);
	}

	#endregion

	#region Работа со списком тестов

	public int AddTest(Test test)
	{
		test.TestId = Database.SendExecuteScalarWithLastId($"INSERT INTO Tests (Name, Description, ComplicationId) VALUES ('{test.Name}','{test.Description}','{test.Complication.ComplicationId}'); ");
		return test.TestId;
	}

	public void EditTest(Test test)
	{
		Database.SendExecuteNonQuery($"UPDATE Tests SET Name = '{test.Name}', Description = '{test.Description}', ComplicationId = '{test.Complication.ComplicationId}' WHERE TestId = '{test.TestId}'");
	}

	public void DeleteTest(Test test)
	{
		Database.SendExecuteNonQuery($"DELETE FROM Tests WHERE TestId = {test.TestId}");
	}

	public Test GetTest(int testId)
	{
		Test test = GetTests().Single(t => t.TestId == testId);
		return test;
	}

	public List<Test> GetTests(Complication complication)
	{
		List<Test> test = GetTests().Where(t => t.Complication.ComplicationId == complication.ComplicationId).ToList();
		return test;
	}

	public List<Test> GetTests()
	{
		List<Test> tests = new List<Test>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM Tests";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						tests.Add(new Test()
						{
							TestId = reader.GetInt32(0),
							Name = reader.GetString(1),
							Description = reader.GetString(2),
							ComplicationId = reader.GetInt32(3)
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var test in tests)
		{
			test.Complication = GetComplicationById(test.ComplicationId);
			test.Questions = GetQuestions(test.TestId);
		}

		return tests;
	}

	public void CopyTest(Test test, int complicationId)
	{
		Complication complication = GetComplicationById(complicationId);
		test.Complication = complication;

		// сохраняем старый Id
		var oldTestId = test.TestId;
		AddTest(test);

		// получаем новый Id
		var newTestId = test.TestId;

		List<Question> questions = GetQuestions(oldTestId);

		// записываем вопросы
		var oldQuestionIdList = new List<int>();

		foreach (var question in questions)
		{
			oldQuestionIdList.Add(question.QuestionId);
			question.TestId = newTestId;
			AddQuestion(question);
		}

		// записываем ответы
		var newQuestionIdList = GetQuestionsIdList(newTestId);

		if (oldQuestionIdList.Count == newQuestionIdList.Count)
		{
			for (int i = 0; i < oldQuestionIdList.Count; i++)
			{
				var type = GetQuestionById(oldQuestionIdList[i]).TypeQuestion;

				switch (type)
				{
					case Question.Type.Image:
						var imageAnswers = GetImageList(oldQuestionIdList[i]);
						foreach (var imageAnswer in imageAnswers)
						{
							imageAnswer.QuestionId = newQuestionIdList[i];
							AddAnswerImage(imageAnswer);
						}
						break;
					case Question.Type.OrderList:
						var orderListAnswers = GetOrderList(oldQuestionIdList[i]);
						foreach (var orderListAnswer in orderListAnswers)
						{
							orderListAnswer.QuestionId = newQuestionIdList[i];
							AddAnswerOrderList(orderListAnswer);
						}
						break;
					case Question.Type.SpaceFill:
						var spaceFillAnswers = GetDragNDropList(oldQuestionIdList[i]);
						foreach (var spaceFillAnswer in spaceFillAnswers)
						{
							spaceFillAnswer.QuestionId = newQuestionIdList[i];
							AddAnswerDragNDrop(spaceFillAnswer);
						}
						break;
					default:
						var textAnswers = GetAnswersText(oldQuestionIdList[i]);
						foreach (var answerOfText in textAnswers)
						{
							answerOfText.QuestionId = newQuestionIdList[i];
							AddAnswerText(answerOfText);
						}
						break;
				}
			}
		}
	}

	#endregion

	#region Работа с пройденными тестами

	/// <summary>
	/// Начать прохождение теста
	/// </summary>
	/// <param name="personId"></param>
	/// <param name="test"></param>
	/// <returns></returns>
	public PassedTest StartTest(int personId, Test test)
	{
		PassedTest passedTest = new PassedTest()
		{
			TestId = test.TestId,
			Name = test.Name,
			Description = test.Description,
			Questions = test.Questions,
			Complication = test.Complication,
			ComplicationId = test.Complication.ComplicationId,
			PersonId = personId,
		};
		return passedTest;
	}

	/// <summary>
	/// Окончание прохождения теста
	/// </summary>
	/// <param name="test"></param>
	/// <param name="isProbe"></param>
	public void EndTest(PassedTest test, bool isProbe)
	{
		if (!isProbe)
		{
			AddPassedTests(test);
		}
	}

	/// <summary>
	/// Добавить пройденный тест
	/// </summary>
	/// <param name="test">Пройденный тест</param>
	public void AddPassedTests(PassedTest test)
	{
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			int logId = Database.SendExecuteScalarWithLastId($"INSERT INTO TestLog (Date, TestId, Grade, PersonId) VALUES ('{test.Date}', '{test.TestId}', '{test.Grade}', '{test.PersonId}'); ");

			foreach (var query in test.PassedQuestions)
			{
				query.LogId = logId;
				Database.SendExecuteNonQuery($"INSERT INTO TestLogList (LogId, QuestionId, Answer, IsRight) VALUES ('{query.LogId}','{query.QuestionId}','{query.Answer}','{Convert.ToInt32(query.IsRight)}');");
			}
		}
	}

	/// <summary>
	/// Получить пройденные тесты пользователя
	/// </summary>
	/// <param name="personId">Индекс пользователя</param>
	/// <returns></returns>
	public List<PassedTest> GetPassedTests(int personId)
	{
		List<PassedTest> tests = new List<PassedTest>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();
			SQLiteCommand command = connection.CreateCommand();

			command.CommandText = $"SELECT l.LogId,t.TestId,t.Name, t.Description, l.Date, l.Grade FROM TestLog as l INNER JOIN Tests as t ON l.TestId = t.TestId WHERE l.StudentId = '{personId}'";
			command.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						tests.Add(new PassedTest()
						{
							LogId = reader.GetInt32(0),
							TestId = reader.GetInt32(1),
							Name = reader.GetString(2),
							Description = reader.GetString(3),
							Date = DateTime.Parse(reader.GetString(4)),
							Grade = reader.GetInt32(5),
							PersonId = personId,

						});
					}
				}
				reader.Close();

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		foreach (var passedTest in tests)
		{
			passedTest.PassedQuestions = GetPassedTestLists(passedTest.LogId);
		}

		return tests;
	}

	/// <summary>
	/// Получить пройденные тесты лога
	/// </summary>
	/// <param name="logId">Индекс лога</param>
	/// <returns></returns>
	public List<PassedQuestion> GetPassedTestLists(int logId)
	{
		List<PassedQuestion> passedList = new List<PassedQuestion>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();
			SQLiteTransaction transaction = connection.BeginTransaction();
			SQLiteCommand command = connection.CreateCommand();
			command.CommandText = $"SELECT * FROM TestLogList WHERE LogId = {logId}";

			try
			{
				SQLiteDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						passedList.Add(new PassedQuestion()
						{
							LogId = reader.GetInt32(0),
							QuestionId = reader.GetInt32(1),
							Answer = reader.GetString(2),
							IsRight = Convert.ToBoolean(reader.GetValue(3))
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return passedList;
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
		question.QuestionId = Database.SendExecuteScalarWithLastId($"INSERT INTO TestQuestions (Name, TestId, Type, Advice) VALUES ('{question.Name}', '{question.TestId}', '{(int)question.TypeQuestion}','{question.Advice}'); ");
		return question.QuestionId;
	}

	/// <summary>
	/// Редактировать вопрос
	/// </summary>
	/// <param name="question">Вопрос</param>
	public void EditQuestion(Question question)
	{
		Database.SendExecuteNonQuery($"UPDATE TestQuestions SET Name = '{question.Name}', Type = '{(int)question.TypeQuestion}', Advice = '{question.Advice}' WHERE QuestionId = '{question.QuestionId}'");
	}

	/// <summary>
	/// Удалить вопрос
	/// </summary>
	/// <param name="question">Вопрос</param>
	public void DeleteQuestion(Question question)
	{
		Database.SendExecuteNonQuery($"DELETE FROM TestQuestions WHERE QuestionId = {question.QuestionId}");
	}

	/// <summary>
	/// Получить вопрос по индексу
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public Question GetQuestionById(int questionId)
	{
		Question question = new Question();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();
			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM TestQuestions WHERE QuestionId = '{questionId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						question.QuestionId = reader.GetInt32(0);
						question.Name = reader.GetString(1);
						question.TestId = reader.GetInt32(2);
						question.TypeQuestion = (Question.Type)reader.GetInt32(3);
						question.Advice = reader["Advice"].ToString();
					}
				}
				reader.Close();

				transaction.Commit();

			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return question;
	}

	/// <summary>
	/// Получить список вопросов теста 
	/// </summary>
	/// <param name="testId">Индекс теста</param>
	/// <returns></returns>
	public List<Question> GetQuestions(int testId)
	{
		List<Question> questions = new List<Question>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();
			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM TestQuestions WHERE TestId = {testId}";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						questions.Add(new Question()
						{
							QuestionId = reader.GetInt32(0),
							Name = reader.GetString(1),
							TestId = reader.GetInt32(2),
							TypeQuestion = (Question.Type)reader.GetInt32(3),
							Advice = reader["Advice"].ToString()
						});
					}
				}
				reader.Close();
				transaction.Commit();

			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return questions;
	}

	/// <summary>
	/// Получить список индексов вопросов теста
	/// </summary>
	/// <param name="testId">Индекс теста</param>
	/// <returns></returns>
	public List<int> GetQuestionsIdList(int testId)
	{
		List<int> questionList = new List<int>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();
			SQLiteCommand command = connection.CreateCommand();
			SQLiteTransaction transaction = connection.BeginTransaction();
			command.CommandText = $"SELECT * FROM TestQuestions WHERE TestId = {testId}";
			command.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var currentId = reader.GetInt32(0);
						questionList.Add(currentId);
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return questionList;
	}

	/// <summary>
	/// Получить типы вопросов теста
	/// </summary>
	/// <param name="testId">Индекс теста</param>
	/// <returns></returns>
	public List<int> GetQuestionsTypeList(int testId)
	{
		List<int> questionList = new List<int>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();
			SQLiteCommand command = connection.CreateCommand();
			SQLiteTransaction transaction = connection.BeginTransaction();
			command.CommandText = $"SELECT * FROM TestQuestions WHERE TestId = {testId}";
			command.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						questionList.Add(reader.GetInt32(3));
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return questionList;
	}

	/// <summary>
	/// Получить изображение для вопроса
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public byte[] GetQuestionImage(int questionId)
	{
		byte[] currentImage = { };

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteCommand cmd = connection.CreateCommand();
			SQLiteTransaction transaction = connection.BeginTransaction();
			cmd.CommandText = $"SELECT * FROM AnswerImage WHERE QuestionId = {questionId}";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						currentImage = (byte[])reader.GetValue(1);
					}
				}
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return currentImage;
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
		int id = Database.SendExecuteScalarWithLastId($"INSERT INTO AnswerText (Name, IsPhraseAnswer, IsCorrect, QuestionId) VALUES('{answer.Name}','{Convert.ToInt32(answer.IsPhraseAnswer)}','{Convert.ToInt32(answer.IsCorrect)}','{answer.QuestionId}')");
		return id;
	}

	/// <summary>
	/// Редактировать ответ с выбором варианта
	/// </summary>
	/// <param name="answer"></param>
	public void EditAnswerText(AnswerOfText answer)
	{
		Database.SendExecuteNonQuery($"UPDATE AnswerText SET Name = '{answer.Name}', IsPhraseAnswer = '{Convert.ToInt32(answer.IsPhraseAnswer)}', IsCorrect = '{Convert.ToInt32(answer.IsCorrect)}' WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Удалить ответ с выбором варианта
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswer(AnswerOfText answer)
	{
		Database.SendExecuteNonQuery($"DELETE FROM AnswerText WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Получить ответы с выбором варианта у вопроса
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public List<AnswerOfText> GetAnswersText(int questionId)
	{
		List<AnswerOfText> answers = new List<AnswerOfText>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM AnswerText WHERE QuestionId = '{questionId}'";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						answers.Add(new AnswerOfText()
						{
							AnswerId = reader.GetInt32(0),
							Name = reader.GetString(1),
							IsPhraseAnswer = Convert.ToBoolean(reader.GetValue(2)),
							IsCorrect = Convert.ToBoolean(reader.GetValue(3)),
							QuestionId = reader.GetInt32(4)
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return answers;
	}

	/// <summary>
	/// Получить правильный ответ с выбором варианта у вопроса
	/// </summary>
	/// <param name="questionId">Индекс вопроса</param>
	/// <returns></returns>
	public AnswerOfText GetRightAnswerText(int questionId)
	{
		AnswerOfText answer = new AnswerOfText();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteTransaction transaction = connection.BeginTransaction();

			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT * FROM AnswerText WHERE QuestionId = '{questionId}' AND IsCorrect = 1";
			cmd.Transaction = transaction;

			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						answer = new AnswerOfText()
						{
							AnswerId = reader.GetInt32(0),
							Name = reader.GetString(1),
							IsPhraseAnswer = Convert.ToBoolean(reader.GetValue(2)),
							IsCorrect = Convert.ToBoolean(reader.GetValue(3)),
							QuestionId = reader.GetInt32(4)
						};
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return answer;
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
		int id = Database.SendExecuteScalarWithLastId($"INSERT INTO AnswerOrder (stringValue, TrueOrder, QuestionId) VALUES('{answer.StringValue}','{answer.StringOrder}','{answer.QuestionId}')");
		return id;
	}

	/// <summary>
	/// Редактировать ответ с выбором порядка
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void EditAnswerOrderList(AnswerOrderList answer)
	{
		Database.SendExecuteNonQuery($"UPDATE AnswerOrder SET stringValue = '{answer.StringValue}', TrueOrder = '{answer.StringOrder}' WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Удалить ответ с выбором порядка
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswerOrderList(AnswerOrderList answer)
	{
		Database.SendExecuteNonQuery($"DELETE FROM AnswerOrder WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Получить список ответов с выбором порядка у вопроса
	/// </summary>
	/// <param name="pQuestionId">Индекс вопроса</param>
	/// <returns></returns>
	public List<AnswerOrderList> GetOrderList(int pQuestionId)
	{
		List<AnswerOrderList> answerOrderList = new List<AnswerOrderList>();

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();
			SQLiteCommand command = connection.CreateCommand();
			SQLiteTransaction transaction = connection.BeginTransaction();
			command.CommandText = String.Format("SELECT * FROM AnswerOrder WHERE QuestionId = " + pQuestionId.ToString());
			command.Transaction = transaction;
			try
			{
				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							answerOrderList.Add(new AnswerOrderList()
							{
								AnswerId = reader.GetInt32(0),
								StringValue = reader["stringValue"].ToString(),
								StringOrder = reader.GetInt32(2),
								QuestionId = reader.GetInt32(3)
							});
						}
					}
				}

				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}
		return answerOrderList;
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
		var imageString = Convert.ToBase64String(answer.ImageData);

		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();
			SQLiteCommand command = new SQLiteCommand()
			{
				Connection = connection,
				CommandText = $"INSERT INTO AnswerImage (FileName, Data, IsCorrect, QuestionId) VALUES ('{answer.ImageName}', '{imageString}', '{Convert.ToInt32(answer.IsCorrect)}','{answer.QuestionId}')"
			};

			int id = Database.SendExecuteScalarWithLastId(command.CommandText);
			return id;
		}
	}

	/// <summary>
	/// Редактировать ответ с выбором изображения
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void EditAnswerImage(AnswerImage answer)
	{
		var imageString = Convert.ToBase64String(answer.ImageData);
		Database.SendExecuteNonQuery($"UPDATE AnswerImage SET FileName = '{answer.ImageName}', Data = '{imageString}', IsCorrect= '{Convert.ToInt32(answer.IsCorrect)}' WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Удалить ответ с выбором изображения
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswerImage(AnswerImage answer)
	{
		Database.SendExecuteNonQuery($"DELETE FROM AnswerImage WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Получить список ответов с выбором изображения у вопроса
	/// </summary>
	/// <param name="pQuestionId">Индекс вопроса</param>
	/// <returns></returns>
	public List<AnswerImage> GetImageList(int pQuestionId)
	{
		List<AnswerImage> answers = new List<AnswerImage>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();

			SQLiteCommand cmd = connection.CreateCommand();
			SQLiteTransaction transaction = connection.BeginTransaction();
			cmd.Transaction = transaction;
			cmd.CommandText = $"SELECT * FROM AnswerImage WHERE QuestionId = {pQuestionId}";
			try
			{
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var imageString = (string)reader["Data"];
						byte[] imageData = Convert.FromBase64String(imageString);

						answers.Add(new AnswerImage()
						{
							AnswerId = reader.GetInt32(0),
							ImageName = reader["FileName"].ToString(),
							ImageData = imageData,
							QuestionId = reader.GetInt32(3),
							IsCorrect = Convert.ToBoolean(reader.GetValue(4))
						});
					}
				}
				reader.Close();
				transaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); transaction.Rollback();
			}

			connection.Close();
		}

		return answers;
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
		int id = Database.SendExecuteScalarWithLastId($"INSERT INTO AnswerExercise (myText, IsAnswerText, indexNumber, QuestionId, Paragraph) VALUES('{answer.TextInfo}','{Convert.ToInt32(answer.IsAnswerText)}','{answer.TextIndex}','{answer.QuestionId}','{Convert.ToInt32(answer.Paragraph)}')");
		return id;
	}

	/// <summary>
	/// Редактировать ответ с выбором позиции в тексте
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void EditAnswerDragNDrop(AnswerDragNDrop answer)
	{
		Database.SendExecuteNonQuery($"UPDATE AnswerExercise SET myText = '{answer.TextInfo}', IsAnswerText = '{Convert.ToInt32(answer.IsAnswerText)}', indexNumber= '{answer.TextIndex}', Paragraph = '{Convert.ToInt32(answer.Paragraph)}' WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Удалить ответ с выбором позиции в тексте
	/// </summary>
	/// <param name="answer">Ответ</param>
	public void DeleteAnswerDragNDrop(AnswerDragNDrop answer)
	{
		Database.SendExecuteNonQuery($"DELETE FROM AnswerExercise WHERE AnswerId = '{answer.AnswerId}'");
	}

	/// <summary>
	/// Получить список ответов с выбором позиции в тексте у вопроса
	/// </summary>
	/// <param name="pQuestionId">Идекс вопроса</param>
	/// <returns></returns>
	public List<AnswerDragNDrop> GetDragNDropList(int pQuestionId)
	{
		List<AnswerDragNDrop> returnList = new List<AnswerDragNDrop>();
		using (SQLiteConnection connection = new SQLiteConnection(Database.ConnectionString))
		{
			connection.Open();
			SQLiteCommand cmd = connection.CreateCommand();
			SQLiteTransaction checkTransaction = connection.BeginTransaction();
			cmd.CommandText = $"SELECT * FROM AnswerExercise WHERE QuestionId = {pQuestionId}";
			cmd.Transaction = checkTransaction;
			try
			{
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							returnList.Add(new AnswerDragNDrop()
							{
								AnswerId = reader.GetInt32(0),
								TextInfo = reader["myText"].ToString(),
								IsAnswerText = Convert.ToBoolean(reader.GetValue(2)),
								TextIndex = reader.GetInt32(3),
								QuestionId = reader.GetInt32(4),
								Paragraph = Convert.ToBoolean(reader.GetValue(5))
							});
						}
					}
				}

				checkTransaction.Commit();
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message); checkTransaction.Rollback();
			}

			connection.Close();
		}
		return returnList;
	}

	#endregion
}