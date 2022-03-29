using System;
using System.Diagnostics;
using Core.Global.Authorization;
using Core.Extensions;
using Core.Settings;
using Core.Testing;
using Core.Ui;
using UnityEngine;

namespace Core.Global.Administration
{
	public class AdminControl : CanvasControl<IAdminPanel>, IAdministration
	{
		public bool IsInitialized { get; set; }

		private ButtonPanel buttonPanel;

		private HelpPanel helpPanel;
		private AboutPanel aboutPanel;

		private DropMenu dropMenu;

		protected override void OnItemsInitialised()
		{
			gameObject.SetActive(false);

			buttonPanel = GetControl<ButtonPanel>();
			helpPanel = GetControl<HelpPanel>();
			aboutPanel = GetControl<AboutPanel>();
			
			#region Подписки панели с кнопками

			buttonPanel.OnAboutButtonClick += ShowAbout;
			buttonPanel.OnHelpButtonClick += ShowHelp;
			buttonPanel.OnDocumentaryButtonClick += ShowDocumentary;
			buttonPanel.OnInputButtonClick += ShowInput;
			buttonPanel.OnSettingsButtonClick += ShowSettings;
			buttonPanel.OnTestingButtonClick += ShowTesting;
			buttonPanel.OnQuitButtonClick += ShowQuit;

			#endregion
		}
		
		public override void Notify(Message message)
		{
			switch (message.Type)
			{
				case Messages.Load.Administrator:
					Show();
					break;
			}
		}

		#region Методы панели с кнопками

		/// <summary>
		/// Показать о программе
		/// </summary>
		private void ShowAbout()
		{
			aboutPanel.Show();
		}

		/// <summary>
		/// Показать панель помощи
		/// </summary>
		private void ShowHelp()
		{
			helpPanel.Show();
		}

		/// <summary>
		/// Показать документацию
		/// </summary>
		private void ShowDocumentary()
		{
			Process.Start("explorer.exe", Environment.CurrentDirectory + @"\" + SettingsStorage.Settings.DocumentryFolderPath);
		}

		/// <summary>
		/// Показать панель настройки ввода
		/// </summary>
		private void ShowInput()
		{
			Send(typeof(IInputBindingControl), new Message(Messages.RebindControl.ShowRebind));
		}

		/// <summary>
		/// Показать настройки
		/// </summary>
		private void ShowSettings()
		{
			if (dropMenu == null)
			{
				CreateDropMenu();
			}
			else
			{
				Destroy(dropMenu.gameObject);
			}
		}

		/// <summary>
		/// Тестирование
		/// </summary>
		private void ShowTesting()
		{
			Send(typeof(ITestControl), new Message(Messages.Testing.ShowTesting));
		}

		/// <summary>
		/// Выйти из админки
		/// </summary>
		private void ShowQuit()
		{
			Send(typeof(IAuthorization), new Message(Messages.Autorization.CloseSession));
			Hide();
		}


		private void Show()
		{
			gameObject.SetActive(true);
		}

		private void Hide()
		{
			gameObject.SetActive(false);
		}

		private void CreateDropMenu()
		{
			dropMenu = UiBuilder.CreateDropMenu(buttonPanel.transform, new Vector3((buttonPanel.transform.position.x + 230f), buttonPanel.transform.position.y + 140f));
			dropMenu.GetComponent<RectTransform>().SetWidth(300);
			dropMenu.IsNeedDestroyOnExit = true;

			var passwordBtn = UiBuilder.CreateButton(dropMenu.transform, Vector3.one, "Сменить пароль", TextAnchor.MiddleCenter, null, Color.black);
			passwordBtn.onClick.AddListener(() =>
			{
				Send(null, new Message(Messages.Autorization.ShowChangePassword));
				dropMenu.Destroy();
			});

			var adminBtn = UiBuilder.CreateButton(dropMenu.transform, Vector3.one, "Администраторы", TextAnchor.MiddleCenter, null, Color.black);
			adminBtn.onClick.AddListener(() =>
			{
				Send(null, new Message(Messages.Autorization.ShowChangeAdministrator));
				dropMenu.Destroy();
			});

			var teacherBtn = UiBuilder.CreateButton(dropMenu.transform, Vector3.one, "Преподаватели", TextAnchor.MiddleCenter, null, Color.black);
			teacherBtn.onClick.AddListener(() =>
			{
				Send(null, new Message(Messages.Autorization.ShowChangeTeacher));
				dropMenu.Destroy();
			});

			var studentBtn = UiBuilder.CreateButton(dropMenu.transform, Vector3.one, "Студенты", TextAnchor.MiddleCenter, null, Color.black);
			studentBtn.onClick.AddListener(() =>
			{
				Send(null, new Message(Messages.Autorization.ShowChangeStudent));
				dropMenu.Destroy();
			});

			var testBtn = UiBuilder.CreateButton(dropMenu.transform, Vector3.one, "Тесты", TextAnchor.MiddleCenter, null, Color.black);
			testBtn.onClick.AddListener(() =>
			{
				Send(null, new Message(Messages.Testing.ShowChangeTest));
				dropMenu.Destroy();
			});
		}
		
		#endregion
		
	}
}
