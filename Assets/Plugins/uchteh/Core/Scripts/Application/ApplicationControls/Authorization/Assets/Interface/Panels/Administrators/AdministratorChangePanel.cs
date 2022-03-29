using System;
using Core.Settings;
using Core.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class AdministratorChangePanel : Panel, IAutorizationPanel
	{
		public event Action<Administrator, bool> OnSaveAdministratorButtonClick;

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

		private Administrator curAdmin;

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
		/// <param name="administrator">Пользователь</param>
		/// <param name="isNewCount">Новая запись</param>
		public void Show(Administrator administrator, bool isNewCount)
		{
			curAdmin = administrator;
			isNew = isNewCount;

			warningText.text = "";

			title.text = isNew ? "Добавить администратора" : "Редактировать администратора";
			login.text = curAdmin.Person.Login;
			password.text = curAdmin.Person.Password;
			userName.text = curAdmin.Person.Name;
			userSurname.text = curAdmin.Person.Surname;
			userPatronymic.text = curAdmin.Person.Patronymic;

			base.Show();
		}

		/// <summary>
		/// Сохранить администратора
		/// </summary>
		private void Save()
		{
			if (IsValid())
			{
				OnSaveAdministratorButtonClick?.Invoke(curAdmin, isNew);
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

			if (string.IsNullOrEmpty(curAdmin.Person.Login))
			{
				warningText.text += "Необходимо ввести логин\n";
				response = false;
			}

			if (string.IsNullOrEmpty(curAdmin.Person.Password))
			{
				warningText.text += "Необходимо ввести пароль\n";
				response = false;
			}

			if (curAdmin.Person.Login == curAdmin.Person.Password)
			{
				warningText.text += "Одинаковый логин и пароль \n";
				return false;
			}

			if (curAdmin.Person.Password.Length < AutorizationSettings.Default().MinPasswordChar)
			{
				warningText.text += "Слишком короткий пароль \n";
				return false;
			}

			if (curAdmin.Person.Password.Length > AutorizationSettings.Default().MaxPasswordChar)
			{
				warningText.text += "Слишком длинный пароль \n";
				return false;
			}

			if (curAdmin.Person.Login.Length < AutorizationSettings.Default().MinLoginChar)
			{
				warningText.text += "Слишком короткий логин \n";
				return false;
			}

			if (curAdmin.Person.Login.Length > AutorizationSettings.Default().MaxLoginChar)
			{
				warningText.text += "Слишком длинный логин \n";
				return false;
			}

			if (string.IsNullOrEmpty(curAdmin.Person.Name))
			{
				warningText.text += "Необходимо ввести имя\n";
				response = false;
			}

			if (string.IsNullOrEmpty(curAdmin.Person.Surname))
			{
				warningText.text += "Необходимо ввести фамилию\n";
				response = false;
			}

			if (string.IsNullOrEmpty(curAdmin.Person.Surname))
			{
				warningText.text += "Необходимо ввести отчество\n";
				response = false;
			}

			return response;
		}

		/// <summary>
		/// Изменить имя администратора
		/// </summary>
		/// <param name="newName"></param>
		private void ChangeName(string newName)
		{
			curAdmin.Person.Name = newName;
		}

		/// <summary>
		/// Изменить фамилию администратора
		/// </summary>
		/// <param name="newSurname"></param>
		private void ChangeSurname(string newSurname)
		{
			curAdmin.Person.Surname = newSurname;
		}

		/// <summary>
		/// Изменить отчество администратора
		/// </summary>
		/// <param name="newPatronymic"></param>
		private void ChangePatronymic(string newPatronymic)
		{
			curAdmin.Person.Patronymic = newPatronymic;
		}

		/// <summary>
		/// Изменить логин администратора
		/// </summary>
		/// <param name="newLogin"></param>
		private void ChangeLogin(string newLogin)
		{
			curAdmin.Person.Login = newLogin;
		}

		/// <summary>
		/// Изменить пароль администратора
		/// </summary>
		/// <param name="newPassword"></param>
		private void ChangePassword(string newPassword)
		{
			curAdmin.Person.Password = newPassword;
		}
	}
}
