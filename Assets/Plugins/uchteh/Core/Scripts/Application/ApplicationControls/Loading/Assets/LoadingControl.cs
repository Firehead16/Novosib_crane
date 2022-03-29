using System.Collections.Generic;
using Core.Gameplay.Questing;
using Core.Settings;
using Core.Ui;
using UnityEngine.SceneManagement;


namespace Core.Settings
{
	public static partial class Messages
	{
		public enum Load
		{
			FirstScene,
			SecondScene,
			Student,
			Teacher,
			Administrator,
			LoadingStringUpdate,
			RebindScene,
			ReloadScene,
			LoadQuests
		}
	}
}

public class LoadingControl : CanvasControl<InputActionAssetsPreviewPanel>, ILoading
{
	protected override void OnItemsInitialised()
	{
		
	}

	public override void Notify(Message message)
	{
		switch (message.Type)
		{
			case Messages.Load.LoadQuests:
				SettingsStorage.ScenePrepareMessages.Clear();
				SettingsStorage.ScenePrepareMessages.Add(typeof(IQuestControl),new Message(Messages.QuestControl.StartWork, (List<QuestSettings>)message.Content[0]));
				ReloadScene(Mode.Exam);
				break;
			case Messages.Load.RebindScene:
				break;
			case Messages.Load.ReloadScene:
				ReloadScene((Mode)message.Content[0]);
				break;
		}
	}


	/// <summary>
	/// Перезапуск сцены
	/// </summary>
	/// <param name="mode"></param>
	private void ReloadScene(Mode mode)
	{
		SettingsStorage.Mode = mode;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
