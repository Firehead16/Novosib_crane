using System;
using Core.Settings;
using Core.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class AccessPanel : Panel, IAutorizationPanel
	{

		public event Action<Person> OnCheckLoginButtonClick;
		public event Action OnOpenRegisterPanelButtonClick;
		public event Action OnQuitApplicationButtonClick;

		[SerializeField]
		private Button checkLoginButton = null;

		[SerializeField]
		private Button openRegisterPanelButton = null;

		[SerializeField]
		private Button quitApplicationButton = null;

		[SerializeField]
		private TMP_InputField loginInputField = null;

		[SerializeField]
		private TMP_InputField passwordInputField = null;

		[SerializeField]
		private Text errorText = null;

		public override void Initialize()
		{
			base.Initialize();

			checkLoginButton.onClick.AddListener(() =>
			{
				if (ValidateInput())
				{
					Person person = new Person()
					{
						Login = loginInputField.text,
						Password = passwordInputField.text
					};
					OnCheckLoginButtonClick?.Invoke(person);
				}
			});
			openRegisterPanelButton.onClick.AddListener(() =>
			{
				OnOpenRegisterPanelButtonClick?.Invoke();
			});
			quitApplicationButton.onClick.AddListener(() =>
			{
				OnQuitApplicationButtonClick?.Invoke();
			});

		}

		public override void Show(bool isTimeShow = false)
		{
			loginInputField.text = "";
			passwordInputField.text = "";

			base.Show(isTimeShow);
		}

		public void SetError(string text)
		{
			errorText.text = text;
		}

		private bool ValidateInput()
		{
			errorText.text = "";

			if (loginInputField.text == "")
			{
				errorText.text += "Введите логин \n";
				return false;
			}
			if (passwordInputField.text == "")
			{
				errorText.text += "Введите пароль \n";
				return false;
			}

			if (loginInputField.text == passwordInputField.text)
			{
				errorText.text += "Одинаковый логин и пароль \n";
				return false;
			}

			if (passwordInputField.text.Length < AutorizationSettings.Default().MinPasswordChar)
			{
				errorText.text += "Слишком короткий пароль \n";
				return false;
			}
			if (passwordInputField.text.Length > AutorizationSettings.Default().MaxPasswordChar)
			{
				errorText.text += "Слишком длинный пароль \n";
				return false;
			}

			if (loginInputField.text.Length < AutorizationSettings.Default().MinLoginChar)
			{
				errorText.text += "Слишком короткий логин \n";
				return false;
			}

			if (loginInputField.text.Length > AutorizationSettings.Default().MaxLoginChar)
			{
				errorText.text += "Слишком длинный логин \n";
				return false;
			}

			return true;
		}
	}
}