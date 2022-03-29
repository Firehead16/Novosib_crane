using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core.Settings
{
	public static partial class Messages
	{
		public enum Application
		{
			Quit,
			DebugStartScene
		}
	}
}

namespace Core.Global
{
	/// <summary>
	/// Имеет файл настроек ApplicationControlSettings, в котором хранится список контролов, которые нужно загрузить. После загрузки контролов сам не инициализируется.
	/// </summary>
	public sealed class ApplicationManager : BaseManagerWithSettins<IApplicationControl, ApplicationManagerSettings>, IApplication
	{
		public ICollection<IApplicationControl> ApplicationControls => Controls.Values;

		[SerializeField, ShowInInspector]
		private SettingsData settingsData;

		[SerializeField]
		private bool isLoadNextScene = false;

		private void Start()
		{
			GlobalLinkStorage.Application = this;
			SettingsStorage.Settings = settingsData;
			IsRoot = true;
			Load();
		}

		protected override void OnControlsLoaded()
		{
			foreach (var applicationControl in Controls.Values.Where(x => x is IManager))
			{
				var manager = (IManager)applicationControl;
				manager.IsRoot = true;
			}

			foreach (var applicationControl in Controls.Values.Where(x => x is IInitialize))
			{
				var initableControl = (IInitialize)applicationControl;
				initableControl.Initialize();
			}

			IsLoaded = true;

			ApplicationStart();
		}

		private void ApplicationStart()
		{
			if (isLoadNextScene)
			{
				Debug.Log("Приложение запускается");
				DontDestroyOnLoad(this);
				SceneManager.LoadScene(1);
			}
		}

		public override void Send(Type typeControlMessageFor, Message message)
		{
			Notify(message);

			if (typeControlMessageFor == null)
			{
				ApplicationControls.ForEach(c => c.Notify(message));
			}
			else
			{
				ApplicationControls.Where(c => c.GetType().GetInterface(typeControlMessageFor.ToString()) != null).ForEach(c => c.Notify(message));
			}
		}

		public override void Notify(Message message)
		{
			switch (message.Type)
			{
				case Messages.Application.Quit:
					Unload();
					Application.Quit();
					break;
			}
		}
	}
}







