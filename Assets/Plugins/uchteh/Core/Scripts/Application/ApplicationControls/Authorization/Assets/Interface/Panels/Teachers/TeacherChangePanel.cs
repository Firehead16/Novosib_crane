using System;
using Core.Settings;
using UnityEngine;
using UnityEngine.UI;
using Core.Ui;
using TMPro;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class TeacherChangePanel : Panel, IAutorizationPanel
	{
		public event Action<Teacher, bool> OnSaveTeacherButtonClick;

		[SerializeField]
		private Text title = null;

		[SerializeField]
		private TMP_InputField login = null;

		[SerializeField]
		private TMP_InputField password = null;

		[SerializeField] 
		private TMP_InputField userSurname = null;

		[SerializeField]
		private TMP_InputField userName = null;

		[SerializeField] 
		private TMP_InputField userPatronymic = null;

		[SerializeField]
		private Button saveButton = null;

		[SerializeField]
		private Button closeButton = null;

		[SerializeField]
		private Text warningText = null;


		/// <summary>
		/// Текущий пользователь
		/// </summary>
		private Teacher curTeacher;

		/// <summary>
		/// Добавляем или редактируем пользователя
		/// </summary>
		private bool isNew;

		public override void Initialize()
		{
			base.Initialize();

			login.onValueChanged.AddListener(ChangeLogin);
			password.onValueChanged.AddListener(ChangePassword);
			userName.onValueChanged.AddListener(ChangeName);
			userSurname.onValueChanged.AddListener(ChangeSurname);
			userPatronymic.onValueChanged.AddListener(ChangePatronymic);
			
			saveButton.onClick.AddListener(Save);
			closeButton.onClick.AddListener(Hide);
		}
		
		/// <summary>
		/// Показать меню
		/// </summary>
		/// <param name="teacher">Пользователь</param>
		/// <param name="isNewCount">Новая запись</param>
		public void Show(Teacher teacher, bool isNewCount)
		{
			curTeacher = teacher;
			isNew = isNewCount;
			
			title.text = isNew ? "Добавить учителя" : "Редактировать учителя";
			login.text = curTeacher.Person.Login;
			password.text = curTeacher.Person.Password;
			userSurname.text = curTeacher.Person.Surname;
			userName.text = curTeacher.Person.Name;
			userPatronymic.text = curTeacher.Person.Patronymic;
			warningText.text = "";

			base.Show();
		}

		/// <summary>
		/// Добавить ошибку
		/// </summary>
		/// <param name="text"></param>
		public void SetError(string text)
		{
			warningText.text = text;
		}

		/// <summary>
		/// Сохранить учителя
		/// </summary>
		private void Save()
		{
			if (IsValid())
			{
				OnSaveTeacherButtonClick?.Invoke(curTeacher, isNew);
			}
		}

		/// <summary>
		/// Правильно введены данные
		/// </summary>
		/// <returns></returns>
		private bool IsValid()
		{
			bool response = true;
			warningText.text = "";

			if (string.IsNullOrEmpty(curTeacher.Person.Login))
			{
				warningText.text += "Необходимо ввести логин\n";
				response = false;
			}

			if (string.IsNullOrEmpty(curTeacher.Person.Password))
			{
				warningText.text += "Необходимо ввести пароль\n";
				response = false;
			}

			if (curTeacher.Person.Login == curTeacher.Person.Password)
			{
				warningText.text += "Одинаковый логин и пароль \n";
				return false;
			}

			if (curTeacher.Person.Password.Length < AutorizationSettings.Default().MinPasswordChar)
			{
				warningText.text += "Слишком короткий пароль \n";
				return false;
			}

			if (curTeacher.Person.Password.Length > AutorizationSettings.Default().MaxPasswordChar)
			{
				warningText.text += "Слишком длинный пароль \n";
				return false;
			}

			if (curTeacher.Person.Login.Length < AutorizationSettings.Default().MinLoginChar)
			{
				warningText.text += "Слишком короткий логин \n";
				return false;
			}

			if (curTeacher.Person.Login.Length > AutorizationSettings.Default().MaxLoginChar)
			{
				warningText.text += "Слишком длинный логин \n";
				return false;
			}

			if (string.IsNullOrEmpty(curTeacher.Person.Name))
			{
				warningText.text += "Необходимо ввести имя\n";
				response = false;
			}

			if (string.IsNullOrEmpty(curTeacher.Person.Surname))
			{
				warningText.text += "Необходимо ввести фамилию\n";
				response = false;
			}

			if (string.IsNullOrEmpty(curTeacher.Person.Surname))
			{
				warningText.text += "Необходимо ввести отчество\n";
				response = false;
			}

			return response;
		}

		/// <summary>
		/// Сменить имя
		/// </summary>
		/// <param name="newName"></param>
		private void ChangeName(string newName)
		{
			curTeacher.Person.Name = newName;
		}

		/// <summary>
		/// Сменить фамилию
		/// </summary>
		/// <param name="newSurname"></param>
		private void ChangeSurname(string newSurname)
		{
			curTeacher.Person.Surname = newSurname;
		}

		/// <summary>
		/// Сменить отчество
		/// </summary>
		/// <param name="newPatronymic"></param>
		private void ChangePatronymic(string newPatronymic)
		{
			curTeacher.Person.Patronymic = newPatronymic;
		}

		/// <summary>
		/// Сменить логин
		/// </summary>
		/// <param name="newLogin"></param>
		private void ChangeLogin(string newLogin)
		{
			curTeacher.Person.Login = newLogin;
		}

		/// <summary>
		/// Сменить пароль
		/// </summary>
		/// <param name="newPassword"></param>
		private void ChangePassword(string newPassword)
		{
			curTeacher.Person.Password = newPassword;
		}
	}
}
