using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Settings;
using MainMenu;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
	#region Панели

	[SerializeField]
	private MainPanel mainPanel = null;

	[SerializeField]
	private CraneSelectPanel craneSelectPanel = null;

	[SerializeField]
	private CraneTaskPanel bridgeTaskPanel = null, castingTaskPanel = null;

	[SerializeField]
	private SettingsPanel settingsPanel = null;

	[SerializeField]
	private LedPanel ledPanel = null;

	[SerializeField]
	private QualificationSelectPanel qualificationSelectPanel = null;

	[SerializeField]
	private DocumentaryPanel documentaryPanel = null;

	[SerializeField]
	private QualificationPanel qual1Panel = null, qual2Panel = null, qual3Panel = null, qual4Panel = null;

	[SerializeField]
	private HelpControllersPanel helpPanel = null;

	[SerializeField]
	private ToggleDescriptionPanel toggleDescriptionPanel = null;

	[SerializeField] 
	private DocumentaryPanel docsPanel = null; // TheoryStudyPanel theoryStudyPanel = null;

	[SerializeField]
	private EmergenceSituationsPanel emergenceSituationsPanel = null;

	[SerializeField]
	private DemoPanel demoPanel = null;

	[SerializeField]
	private HelpControllersPanel helpControllersPanel = null;

	[SerializeField] 
	private QuestPanel questPanel;
	
	[SerializeField]
	private RecordPanel recordPanel;
	
	[SerializeField]
	private LoginPanel loginPanel;

	private List<GameObject> allPanels = new List<GameObject>();

	#endregion

	private void Start()
	{
		Screen.fullScreen = true;
		Time.timeScale = 1;

		Add();
		Subscribe();
	}
	
	/// <summary>
	/// Добавить панели в общий список
	/// </summary>
	private void Add()
	{
		allPanels.Add(mainPanel.gameObject);
		allPanels.Add(craneSelectPanel.gameObject);
		allPanels.Add(settingsPanel.gameObject);
		allPanels.Add(qualificationSelectPanel.gameObject);
		allPanels.Add(documentaryPanel.gameObject);
		allPanels.Add(helpPanel.gameObject);
		allPanels.Add(qual1Panel.gameObject);
		allPanels.Add(qual2Panel.gameObject);
		allPanels.Add(qual3Panel.gameObject);
		allPanels.Add(qual4Panel.gameObject);
		allPanels.Add(toggleDescriptionPanel.gameObject);
		allPanels.Add(docsPanel.gameObject);
		allPanels.Add(emergenceSituationsPanel.gameObject);
		//allPanels.Add(demoPanel.gameObject);
		allPanels.Add(ledPanel.gameObject);
		allPanels.Add(bridgeTaskPanel.gameObject);
		allPanels.Add(castingTaskPanel.gameObject);
		allPanels.Add(questPanel.gameObject);
		allPanels.Add(recordPanel.gameObject);
		allPanels.Add(loginPanel.gameObject);
	}

	/// <summary>
	/// Подписка панелей на события панелей
	/// </summary>
	private void Subscribe()
	{
		mainPanel.OnLearnButtonClick += CraneSelect;
		mainPanel.OnExamButtonClick += SelectQual;
		mainPanel.OnQuestButtonClick += SelectQuest;
		mainPanel.OnSettingButtonClick += Settings;
		mainPanel.OnTheoryStudyButtonClick += TheoryStudy;
		mainPanel.OnHelpButtonClick += Help;
		mainPanel.OnQuitButtonClick += QuitApp;
		mainPanel.OnVideoPlayerClick += VideoPlayer;
		mainPanel.OnLoginButtonClick += LoginButton;

		craneSelectPanel.OnStartedQuestsButtonClick += StartCraneQualifications;
		craneSelectPanel.OnBridgeClick += () =>
		{
			CloseAllExceptThis(bridgeTaskPanel.gameObject);
			bridgeTaskPanel.gameObject.SetActive(true);
		};
		craneSelectPanel.OnCastingClick += () =>
		{
			CloseAllExceptThis(castingTaskPanel.gameObject);
			castingTaskPanel.gameObject.SetActive(true);
		};
		craneSelectPanel.OnBackToMainMenuButtonClick += BackToMain;

		bridgeTaskPanel.OnBackToQualButtonClick += CraneSelect;
		//bridgeTaskPanel.OnToggleChanged += (scene, state) => toggleDescriptionPanel.ChangeDescription(scene, state);

		castingTaskPanel.OnBackToQualButtonClick += CraneSelect;
		//castingTaskPanel.OnToggleChanged += (scene, state) => toggleDescriptionPanel.ChangeDescription(scene, state);

		settingsPanel.OnControlButtonClick += () => ControlExtention.ApplicationSend(typeof(IInputBindingControl), new Message(Messages.RebindControl.ShowRebind));
		//settingsPanel.OnLampButtonClick += Lamp;
		settingsPanel.OnBackToMainButtonClick += BackToMain;

		ledPanel.OnExitButtonClick += Settings;
		ledPanel.OnSaveButtonClick += Settings;
		docsPanel.OnBackToMainMenuButtonClick += BackToMain;
		documentaryPanel.OnBackToMainMenuButtonClick += BackToMain;
		emergenceSituationsPanel.OnBackToMainMenuButtonClick += TheoryStudy;
		helpControllersPanel.OnBackToMainMenuButtonClick += BackToMain;
		questPanel.OnBackToMainButtonClick += BackToMain;
		recordPanel.OnBackToMainMenuButtonClick += BackToMain;
		loginPanel.OnApplyName += ApplyName;
	}

	// Вернуться на главную панель
	private void BackToMain() => CloseAllExceptThis(mainPanel.gameObject);

	private void ApplyName()
	{
		if(loginPanel.InputName.text.Length > 1) QuestStorage.Instance.UserName = loginPanel.InputName.text;
		else QuestStorage.Instance.UserName = String.Empty;
		BackToMain();
	}

	// Закрыть все панели кроме выбранной
	/// <param name="panel"></param>
	private void CloseAllExceptThis(GameObject panel)
	{
		foreach (var closePanel in allPanels.Except(new List<GameObject> { panel }))
		{
			closePanel.SetActive(false);
		}
		panel.SetActive(true);
	}
	
	
	// Открыть панель выбора крана
	private void CraneSelect()
	{
		craneSelectPanel.ClearToggles();

		CloseAllExceptThis(craneSelectPanel.gameObject);
		//craneSelectPanel.gameObject.SetActive(true);
		//QuestStorage.Instance.QuestLog.Mode = Mode.Learn;
	}
	
	// Открыть панель выбора квалификации
	private void SelectQual() => CloseAllExceptThis(qualificationSelectPanel.gameObject);

	private void SelectQuest() => CloseAllExceptThis(questPanel.gameObject);

	/// <summary>
	/// Начало выполнения заданий
	/// </summary>
	/// <param name="toggleScenes"></param>
	private void StartCraneQualifications(List<ToggleScene> toggleScenes)
	{
		//QuestStorage.Instance.QuestLog.SceneList.Clear();

		foreach (var t in toggleScenes)
		{
			if (t.Toggle.isOn)
			{
				//QuestStorage.Instance.QuestLog.SceneList.Enqueue("Scenes/" + t.SceneName);
				Debug.Log("add_quest " + (t.SceneName));
			}
		}

		//SceneManager.LoadScene(QuestStorage.Instance.QuestLog.SceneList.Dequeue());
	}

	// Открыть панель с настройками
	private void Settings() => CloseAllExceptThis(settingsPanel.gameObject);

	private void VideoPlayer() => CloseAllExceptThis(recordPanel.gameObject);
	
	private void LoginButton() => CloseAllExceptThis(loginPanel.gameObject);
	
	/*// Настройка ламп
	private void Lamp()
	{
		CloseAllExceptThis(ledPanel.gameObject);
		ledPanel.Show();
	}*/
	
	// Открыть панель с теоретическим обучением
	private void TheoryStudy() => CloseAllExceptThis(docsPanel.gameObject);
	
	// Открыть панель с документацией
	//private void Docs() => CloseAllExceptThis(documentaryPanel.gameObject);
	
	// Открыть панель с помошью
	private void Help() => CloseAllExceptThis(helpControllersPanel.gameObject);
	
	// Открыть панель аварийные ситуации
	private void EmergencySituations() => CloseAllExceptThis(emergenceSituationsPanel.gameObject);
	
	// Выход из приложения
	private void QuitApp() => Application.Quit();
}
