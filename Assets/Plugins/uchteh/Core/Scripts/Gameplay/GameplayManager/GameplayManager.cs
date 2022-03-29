using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum Gameplay
		{

		}
	}
}

namespace Core.Gameplay
{
	public class GameplayManager : InitableManagerWithSettins<IGameplayControl, GameplayManagerSettings>
	{
		public ICollection<IGameplayControl> GameplayControls => Controls.Values;

		private Dictionary<Type, Message> SceneMessages { get; set; } = new Dictionary<Type, Message>();

		[SerializeField, ShowInInspector]
		private SettingsData settingsData;

		/// <summary>
		/// При старте сцены загружаются все менеджеры, настройки которых хранятся в SubSettings, после загрузки выполняется инициализация
		/// </summary>
		private void Start()
		{
			GlobalLinkStorage.Gameplay = this;
			SettingsStorage.Settings = settingsData;

			// Получаем с прошлой сцены сообщения при запуске
			SceneMessages = SettingsStorage.ScenePrepareMessages;

			Load();
		}

		protected override void OnControlsLoaded()
		{
			base.OnControlsLoaded();

			foreach (var gameplayControl in Controls.Values.Where(x => x is IManager))
			{
				var manager = (IManager)gameplayControl;
				manager.IsRoot = true;
			}

			foreach (IGameplayControl initableControl in Controls.Values)
			{
				initableControl.Initialize();
			}
			IsLoaded = true;
			OnItemsInitialised();
		}

		protected override void OnItemsInitialised()
		{
			IsInitialized = true;

			SendPreparedMessages();
			Debug.Log($"OnItemsInitialised() {GetType()}");
		}

		/// <summary>
		/// Отправка сообщений контроллерам
		/// </summary>
		/// <param name="typeControlMessageFor"></param>
		/// <param name="message"></param>
		public override void Send(Type typeControlMessageFor, Message message)
		{
			if (IsInitialized)
			{
				// Отправляем всем
				if (typeControlMessageFor == null)
				{
					GameplayControls.ForEach(c => c.Notify(message));
				}
				// Отправляем указанному контроллеру
				else
				{
					GameplayControls.Where(c => c.GetType().GetInterface(typeControlMessageFor.ToString()) != null).ForEach(c => c.Notify(message));
				}
			}
		}

		/// <summary>
		/// Отправка отложенных сообщение с предыдущей сцены
		/// </summary>
		private void SendPreparedMessages()
		{
			foreach (var message in SceneMessages)
			{
				Send(message.Key, message.Value);
			}

			SceneMessages.Clear();
		}
	} 
}