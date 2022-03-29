using System;
using Core.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class PasswordPanel : Panel, IAutorizationPanel
	{
		public event Action<string> OnSavePassword;

		[SerializeField]
		private TMP_InputField newPassword = null;

		[SerializeField]
		private TMP_InputField repeatePassword = null;

		[SerializeField]
		private Text textWarning = null;

		[SerializeField]
		private Button saveButton = null;

		[SerializeField]
		private Button closeButton = null;


		public override void Initialize()
		{
			base.Initialize();

			newPassword.onValueChanged.AddListener(GetNewPassword);
			repeatePassword.onValueChanged.AddListener(GetRepeatePassword);

			saveButton.onClick.AddListener(Save);
			closeButton.onClick.AddListener(Hide);
		}

		public override void Hide()
		{
			newPassword.text = " ";
			repeatePassword.text = " ";
			textWarning.text = " ";

			base.Hide();
		}

		/// <summary>
		/// Ввод нового пароля
		/// </summary>
		/// <param name="changeNewPassword"></param>
		private void GetNewPassword(string changeNewPassword)
		{
			newPassword.text = changeNewPassword;
		}

		/// <summary>
		/// Повтор ввода пароля
		/// </summary>
		/// <param name="changeRepeatePassword"></param>
		private void GetRepeatePassword(string changeRepeatePassword)
		{
			repeatePassword.text = changeRepeatePassword;
		}

		/// <summary>
		/// Сохранение изменений
		/// </summary>
		private void Save()
		{
			if (IsValid())
			{
				OnSavePassword?.Invoke(newPassword.text);
				Hide();
			}
		}

		/// <summary>
		/// Валидация формы
		/// </summary>
		/// <returns></returns>
		private bool IsValid()
		{
			bool response = true;
			textWarning.text = "";

			if (newPassword.text != repeatePassword.text)
			{
				textWarning.text += "Пароли не совпадают \n";
				response = false;
			}

			if (string.IsNullOrEmpty(repeatePassword.text))
			{
				textWarning.text += "Введите повторно пароль \n";
				response = false;
			}

			if (string.IsNullOrEmpty(newPassword.text))
			{
				textWarning.text += "Введите новый пароль \n";
				response = false;
			}

			return response;
		}
	}
}

