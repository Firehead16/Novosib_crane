using Core.Global.Network;
using Core.Settings;
using Core.Ui;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum Autorization
		{
			StartSession,
			CloseSession,
			SetProgress,
			DeleteProgress,
			ShowAutorization,
			HideAutorization,
			ShowChangePassword,
			ShowChangeStudent,
			ShowChangeTeacher,
			ShowChangeAdministrator
		}
	}
}

namespace Core.Global.Authorization
{
	/// <summary>
	/// В файле настроек AuthorizationSettings хранятся ссылки на дефолтные панели
	/// </summary>
	public sealed class AuthorizationControl : CanvasControl<IAutorizationPanel>, IAuthorization
	{
		public bool IsInitialized { get; set; }

		/// <summary>
		/// Текущая сессия
		/// </summary>
		[ShowInInspector, ReadOnly]
		public static Session Session { get; private set; }

		private AccessPanel accessPanel;

		private RegisterPanel registerPanel;

		private PasswordPanel passwordPanel;

		private StudentsPanel studentsPanel;
		private StudentChangePanel studentChangePanel;

		private TeachersPanel teachersPanel;
		private TeacherChangePanel teacherChangePanel;

		private AdministratorsPanel administratorsPanel;
		private AdministratorChangePanel administratorChangePanel;

		private ClientNetworkControl networkControl;

		protected override void OnItemsInitialised()
		{
			networkControl = GlobalLinkStorage.Application.GetControl<ClientNetworkControl>();

			accessPanel = GetControl<AccessPanel>();
			registerPanel = GetControl<RegisterPanel>();

			passwordPanel = GetControl<PasswordPanel>();

			studentsPanel = GetControl<StudentsPanel>();
			studentChangePanel = GetControl<StudentChangePanel>();

			teachersPanel = GetControl<TeachersPanel>();
			teacherChangePanel = GetControl<TeacherChangePanel>();

			administratorsPanel = GetControl<AdministratorsPanel>();
			administratorChangePanel = GetControl<AdministratorChangePanel>();

			#region Подписки панели входа

			accessPanel.OnCheckLoginButtonClick += (person) =>
			{
				StartCoroutine(StartSessionAsync(person.Login, person.Password));
			};
			accessPanel.OnOpenRegisterPanelButtonClick += () =>
			{
				accessPanel.Hide();
				registerPanel.Show();
			};
			accessPanel.OnQuitApplicationButtonClick += () =>
			{
				Send(typeof(IApplication), new Message(Messages.Application.Quit));
			};

			#endregion

			#region Подписки панели регистрации

			registerPanel.OnCheckLoginButtonClick += (person) => { StartCoroutine(RegisterPersonAsync(person)); };
			registerPanel.OnQuitApplicationButtonClick += () =>
			{
				accessPanel.Show();
				registerPanel.Hide();
			};

			#endregion

			#region Подписки панели с паролями

			passwordPanel.OnSavePassword += SavePassword;

			#endregion

			#region Подписки панели со студентами

			studentsPanel.OnAddOrEditStudentButtonClick += ShowChangeStudentButtonClick;
			studentsPanel.OnDeleteStudentButtonClick += (student) => StartCoroutine(DeleteStudentInPanelAsync(student));
			studentsPanel.OnChangeActiveStudentButtonClick +=
				(student) => StartCoroutine(SwitchActiveStudentInPanelAsync(student));

			studentChangePanel.OnSaveStudentButtonClick +=
				(student, isNew) => StartCoroutine(ChangeStudentAsync(student, isNew));
			studentChangePanel.OnMenuHide += () => StartCoroutine(ShowStudentPanel());

			#endregion

			#region Подписки панели с преподавателями

			teachersPanel.OnAddOrEditTeacherButtonClick += ShowChangeTeacherButtonClick;
			teachersPanel.OnDeleteTeacherButtonClick += (teacher) => StartCoroutine(DeleteTeacherInPanelAsync(teacher));
			teachersPanel.OnChangeActiveTeacherButtonClick +=
				(teacher) => StartCoroutine(SwitchActiveTeacherInPanelAsync(teacher));

			teacherChangePanel.OnSaveTeacherButtonClick +=
				(teacher, isNew) => StartCoroutine(ChangeTeacherAsync(teacher, isNew));
			teacherChangePanel.OnMenuHide += () => StartCoroutine(ShowTeacherPanel());

			#endregion

			#region Подписки панели с администраторами

			administratorsPanel.OnAddOrEditAdministratorsButtonClick += ShowChangeAdministratorButtonClick;
			administratorsPanel.OnDeleteAdministratorsButtonClick += (administrator) =>
				StartCoroutine(DeleteAdministratorInPanelAsync(administrator));
			administratorsPanel.OnChangeActiveAdministratorsButtonClick += (administrator) =>
				StartCoroutine(SwitchActiveAdministratorInPanelAsync(administrator));

			administratorChangePanel.OnSaveAdministratorButtonClick += (administrator, isNew) =>
				StartCoroutine(ChangeAdministratorAsync(administrator, isNew));
			administratorChangePanel.OnMenuHide += () => StartCoroutine(ShowAdministratorPanel());

			#endregion
		}

		public override void Unload()
		{
			CloseSession();
		}

		public override void Notify(Message message)
		{
			base.Notify(message);
			{
				switch (message.Type)
				{
					case Messages.Autorization.ShowAutorization:
						accessPanel.Show();
						registerPanel.Hide();
						break;
					case Messages.Autorization.HideAutorization:
						accessPanel.Hide();
						registerPanel.Hide();
						break;
					case Messages.Autorization.StartSession:
						string login = message.Content[0].ToString();
						string password = message.Content[1].ToString();
						StartCoroutine(StartSessionAsync(login, password));
						break;
					case Messages.Autorization.CloseSession:
						CloseSession();
						Send(typeof(IAuthorization), new Message(Messages.Autorization.ShowAutorization));
						break;
					case Messages.Autorization.SetProgress:
						string progress = message.Content[0].ToString();
						SetProgress(progress);
						break;
					case Messages.Autorization.DeleteProgress:
						DeleteProgress();
						break;
					case Messages.Autorization.ShowChangePassword:
						passwordPanel.Show();
						break;
					case Messages.Autorization.ShowChangeStudent:
						StartCoroutine(ShowStudentPanel());
						break;
					case Messages.Autorization.ShowChangeTeacher:
						StartCoroutine(ShowTeacherPanel());
						break;
					case Messages.Autorization.ShowChangeAdministrator:
						StartCoroutine(ShowAdministratorPanel());
						break;
				}
			}
		}

		#region Регистрация пользователя

		private IEnumerator RegisterPersonAsync(Person person)
		{
			yield return AddPersonAsync(person);
			yield return StartSessionAsync(person.Login, person.Password);
		}

		private IEnumerator AddPersonAsync(Person person)
		{
			Person existPerson = null;

			var thread = new Thread(() =>
			{
				existPerson = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.GetPerson(person.Login, person.Password);
			});
			thread.Start();
			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			if (existPerson != null)
			{
				registerPanel.SetError("Пользователь уже существует");
				yield break;
			}

			thread = new Thread(() =>
			{
				networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().AddStudent(new Student()
				{
					Person = person
				});
			});
			thread.Start();
			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}
		}

		#endregion

		#region Сессия

		#region Запуск сессии

		private IEnumerator StartSessionAsync(string login, string password)
		{
			bool isPersonExist = false;

			var thread = new Thread(() =>
			{
				isPersonExist = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.IsPersonExist(login, password);
			});
			thread.Start();
			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			if (!isPersonExist)
			{
				accessPanel.SetError("Неверно введены логин или пароль");
				yield break;
			}

			thread = new Thread(() =>
			{
				Person person = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.GetPerson(login,password);

				if (networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.IsSessionOpen(networkControl.Computer))
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
						.CloseSession(networkControl.Computer);
				}

				Session = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.OpenSession(person, networkControl.Computer);
			});
			thread.Start();
			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			switch (Session.Person.Role)
			{
				case Role.Student:
					Send(null, new Message(Messages.Load.Student));
					break;
				case Role.Teacher:
					Send(null, new Message(Messages.Load.Teacher));
					break;
				case Role.Administrator:
					Send(null, new Message(Messages.Load.Administrator));
					break;
			}

			Send(typeof(IAuthorization), new Message(Messages.Autorization.HideAutorization));
		}

		#endregion

		#region Закрытие сессии

		/// <summary>
		///Закрыть сессию 
		/// </summary>
		private void CloseSession()
		{
			this.DebugLog("Закрытие сессии");
			StartCoroutine(CloseSessionAsync());
		}

		private IEnumerator CloseSessionAsync()
		{
			var thread = new Thread(() =>
			{
				if (Session != null)
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().CloseSession(Session);
					Session = null;
				}
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}
		}

		#endregion


		/// <summary>
		/// Запись прогресса прохождения
		/// </summary>
		/// <param name="progress"></param>
		private void SetProgress(string progress)
		{
			Session.Progress = progress;
			networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().SetProgress(Session.Progress, Session.SessionId);
		}

		/// <summary>
		/// Удаление прогресса прохождения
		/// </summary>
		private void DeleteProgress()
		{
			Session.Progress = null;
			networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteProgress(Session.SessionId);
		}

		#endregion

		#region Методы панели с паролями

		/// <summary>
		/// Сохранение изменений пароля
		/// </summary>
		/// <returns></returns>
		private void SavePassword(string password)
		{
			StartCoroutine(SaveFinishAsync(password));
		}

		private IEnumerator SaveFinishAsync(string password)
		{
			Thread thread = new Thread(() =>
			{
				var person = Session.Person;
				person.Password = password;

				networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().EditPerson(person);
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}
		}

		#endregion

		#region Методы панелей со студентами

		/// <summary>
		/// Обновить студентов в панеле
		/// </summary>
		/// <returns></returns>
		private IEnumerator ShowStudentPanel()
		{
			List<Student> students = new List<Student>();

			Thread thread = new Thread(() =>
			{
				students = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.GetStudents();
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			studentsPanel.Show(students);
		}

		/// <summary>
		/// Показать панель изменения студента
		/// </summary>
		/// <param name="student"></param>
		/// <param name="isNew"></param>
		private void ShowChangeStudentButtonClick(Student student, bool isNew)
		{
			studentChangePanel.Show(student, isNew);
		}

		/// <summary>
		/// Сохранить студента
		/// </summary>
		/// <returns></returns>
		private IEnumerator ChangeStudentAsync(Student student, bool isNew)
		{
			if (isNew && networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
				.IsPersonExist(student.Person.Login, student.Person.Password))
			{
				studentChangePanel.SetError("Данный студент уже существует \n");
				yield break;
			}

			Thread thread = new Thread(() =>
			{
				if (isNew)
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().AddStudent(student);
				}
				else
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().EditStudent(student);
				}
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			studentChangePanel.Hide();
		}

		/// <summary>
		/// Удалить пользователя с панели
		/// </summary>
		/// <param name="student"></param>
		/// <returns></returns>
		private IEnumerator DeleteStudentInPanelAsync(Student student)
		{
			yield return StartCoroutine(DeletePersonAsync(student.Person));
			yield return ShowStudentPanel();
		}

		/// <summary>
		/// Изменить состояние активации пользователя на панели
		/// </summary>
		private IEnumerator SwitchActiveStudentInPanelAsync(Student student)
		{
			yield return StartCoroutine(SwitchActivePerson(student.Person));
			yield return StartCoroutine(ShowStudentPanel());
		}

		#endregion

		#region Методы панелей с преподавателями

		/// <summary>
		/// Обновить преподавателей в панеле
		/// </summary>
		/// <returns></returns>
		private IEnumerator ShowTeacherPanel()
		{
			List<Teacher> teachers = new List<Teacher>();

			Thread thread = new Thread(() =>
			{
				teachers = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.GetTeachers();
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			teachersPanel.Show(teachers);
		}

		/// <summary>
		/// Показать панель изменения преподавателя
		/// </summary>
		/// <param name="teacher"></param>
		/// <param name="isNew"></param>
		private void ShowChangeTeacherButtonClick(Teacher teacher, bool isNew)
		{
			teacherChangePanel.Show(teacher, isNew);
		}

		/// <summary>
		/// Сохранить преподавателя
		/// </summary>
		/// <returns></returns>
		private IEnumerator ChangeTeacherAsync(Teacher teacher, bool isNew)
		{
			if (isNew && networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
				.IsPersonExist(teacher.Person.Login, teacher.Person.Password))
			{
				teacherChangePanel.SetError("Данный преподаватель уже существует \n");
				yield break;
			}

			Thread thread = new Thread(() =>
			{
				if (isNew)
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().AddTeacher(teacher);
				}
				else
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().EditTeacher(teacher);
				}
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			teacherChangePanel.Hide();
		}

		/// <summary>
		/// Удалить преподавателя с панели
		/// </summary>
		/// <param name="teacher"></param>
		/// <returns></returns>
		private IEnumerator DeleteTeacherInPanelAsync(Teacher teacher)
		{
			yield return StartCoroutine(DeletePersonAsync(teacher.Person));
			yield return ShowTeacherPanel();
		}

		/// <summary>
		/// Активировать / Деактивировать преподавателя
		/// </summary>
		/// <param name="teacher"></param>
		/// <returns></returns>
		private IEnumerator SwitchActiveTeacherInPanelAsync(Teacher teacher)
		{
			yield return StartCoroutine(SwitchActivePerson(teacher.Person));
			yield return StartCoroutine(ShowTeacherPanel());
		}


		#endregion

		#region Методы панелей с администраторами

		/// <summary>
		/// Обновить администраторов в панеле
		/// </summary>
		/// <returns></returns>
		private IEnumerator ShowAdministratorPanel()
		{
			List<Administrator> administrators = new List<Administrator>();

			Thread thread = new Thread(() =>
			{
				administrators = networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
					.GetAdministrators();
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			administratorsPanel.Show(administrators);
		}

		/// <summary>
		/// Показать панель изменения администратора
		/// </summary>
		/// <param name="administrator"></param>
		/// <param name="isNew"></param>
		private void ShowChangeAdministratorButtonClick(Administrator administrator, bool isNew)
		{
			administratorChangePanel.Show(administrator, isNew);
		}

		/// <summary>
		/// Сохранить администратора
		/// </summary>
		/// <returns></returns>
		private IEnumerator ChangeAdministratorAsync(Administrator administrator, bool isNew)
		{
			if (isNew && networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
				.IsPersonExist(administrator.Person.Login, administrator.Person.Password))
			{
				teacherChangePanel.SetError("Данный администратор уже существует \n");
				yield break;
			}

			Thread thread = new Thread(() =>
			{
				if (isNew)
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
						.AddAdministrator(administrator);
				}
				else
				{
					networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>()
						.EditAdministrator(administrator);
				}
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}

			administratorChangePanel.Hide();
		}

		/// <summary>
		/// Удалить администратора с панели
		/// </summary>
		/// <param name="administrator"></param>
		/// <returns></returns>
		private IEnumerator DeleteAdministratorInPanelAsync(Administrator administrator)
		{
			yield return StartCoroutine(DeletePersonAsync(administrator.Person));
			yield return ShowAdministratorPanel();
		}

		/// <summary>
		/// Активировать / Деактивировать администратора
		/// </summary>
		/// <param name="administrator"></param>
		/// <returns></returns>
		private IEnumerator SwitchActiveAdministratorInPanelAsync(Administrator administrator)
		{
			yield return StartCoroutine(SwitchActivePerson(administrator.Person));
			yield return StartCoroutine(ShowAdministratorPanel());
		}

		#endregion

		#region Общие методы

		private IEnumerator DeletePersonAsync(Person person)
		{
			Thread thread = new Thread(() =>
			{
				networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().DeletePerson(person);
			});
			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}
		}

		private IEnumerator SwitchActivePerson(Person person)
		{
			Thread thread = new Thread(() =>
			{
				person.IsActive = !person.IsActive;
				networkControl.CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().EditPerson(person);
			});

			thread.Start();

			while (thread.IsAlive)
			{
				yield return new WaitForSecondsRealtime(0.1f);

			}
		}

		#endregion
	}
}
