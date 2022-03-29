using System;
using Core.Settings;
using Core.Ui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class RegisterPanel : Panel, IAutorizationPanel
	{
		public event Action<Person> OnCheckLoginButtonClick;

		public event Action OnQuitApplicationButtonClick;

		[SerializeField]
		private Button registerLoginButton = null;

		[SerializeField]
		private Button quitRegistrationButton = null;

		[SerializeField]
		private TMP_InputField loginInputField = null;

		[SerializeField]
		private TMP_InputField passwordInputField = null;

		[SerializeField]
		private TMP_InputField lastNameInputField = null; //фамилия

		[SerializeField]
		private TMP_InputField firstNameInputField = null; //имя

		[SerializeField]
		private TMP_InputField middleNameInputField = null; //отчество

		[SerializeField]
		private Text errorText = null;

		public override void Initialize()
		{
			base.Initialize();

			registerLoginButton.onClick.AddListener(() =>
			{
				if (ValidateInput())
				{
					Person person = new Person()
					{
						Login = loginInputField.text,
						Password = passwordInputField.text,
						Surname = lastNameInputField.text,
						Name = firstNameInputField.text,
						Patronymic = middleNameInputField.text,
						Role = Role.Student,
						IsActive = true
					};
					OnCheckLoginButtonClick?.Invoke(person);
				}
			});
			quitRegistrationButton.onClick.AddListener(() =>
			{
				OnQuitApplicationButtonClick?.Invoke();
			});
		}

		public override void Show(bool isTimeShow = false)
		{
			loginInputField.text = "";
			passwordInputField.text = "";
			lastNameInputField.text = "";
			firstNameInputField.text = "";
			middleNameInputField.text = "";

			base.Show(isTimeShow);
		}

		public void SetError(string text)
		{
			errorText.text = text;
		}

		[Button]
		private bool ValidateInput()
		{
			errorText.text = "";

			if (loginInputField.text == "")
			{
				errorText.text += "Введите логин \n";
				Debug.Log("Введите логин");
				return false;
			}
			if (passwordInputField.text == "")
			{
				errorText.text += "Введите пароль \n";
				Debug.Log("Введите пароль");
				return false;
			}

			if (lastNameInputField.text == "")
			{
				errorText.text += "Введите фамилию \n";
				Debug.Log("Введите фамилию");
				return false;
			}
			if (firstNameInputField.text == "")
			{
				errorText.text += "Введите имя \n";
				Debug.Log("Введите имя");
				return false;
			}
			if (middleNameInputField.text == "")
			{
				errorText.text += "Введите отчество \n";
				Debug.Log("Введите отчество");
				return false;
			}

			if (loginInputField.text == passwordInputField.text)
			{
				errorText.text += "Одинаковый логин и пароль \n";
				Debug.Log("Одинаковый логин и пароль");
				return false;
			}
			if (passwordInputField.text.Length < AutorizationSettings.Default().MinPasswordChar)
			{
				errorText.text += "Слишком короткий пароль \n";
				Debug.Log("Слишком короткий пароль");
				return false;
			}
			if (passwordInputField.text.Length > AutorizationSettings.Default().MaxPasswordChar)
			{
				errorText.text += "Слишком длинный пароль \n";
				Debug.Log("Слишком длинный пароль");
				return false;
			}

			if (loginInputField.text.Length < AutorizationSettings.Default().MinLoginChar)
			{
				errorText.text = "Слишком короткий логин \n";
				Debug.Log("Слишком короткий логин");
				return false;
			}

			if (loginInputField.text.Length > AutorizationSettings.Default().MaxLoginChar)
			{
				errorText.text = "Слишком длинный логин \n";
				Debug.Log("Слишком длинный логин");
				return false;
			}
			return true;
		}
	}
}