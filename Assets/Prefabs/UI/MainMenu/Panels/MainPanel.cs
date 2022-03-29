using System;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
	public event Action OnSettingButtonClick;
	public event Action OnTheoryStudyButtonClick;
	public event Action OnQuitButtonClick;
	public event Action OnVideoPlayerClick;
    public event Action OnExamButtonClick;
    public event Action OnLearnButtonClick;
    public event Action OnQuestButtonClick;
	public event Action OnHelpButtonClick;
	
	public event Action OnLoginButtonClick;

	[SerializeField]
    private Button learnButton = null;

    [SerializeField]
    private Button examButton = null;

    [SerializeField] 
    private Button questButton = null;

	[SerializeField]
	private Button settingsButton = null;

	[SerializeField]
	private Button documentaryButton = null;

	[SerializeField]
	private Button helpButton = null;
	
	[SerializeField]
	private Button loginButton = null;

	[SerializeField]
	private Button quitButton = null;
	
	[SerializeField]
	private Button videoPlayerButton = null;

	[SerializeField] 
	private TMP_Text nameText = null;


	private void Start()
	{
		learnButton.onClick.AddListener(() => OnLearnButtonClick?.Invoke());
        examButton.onClick.AddListener(() => OnExamButtonClick?.Invoke());
        questButton.onClick.AddListener(() => OnQuestButtonClick?.Invoke());
		settingsButton.onClick.AddListener(() => OnSettingButtonClick?.Invoke());
		documentaryButton.onClick.AddListener(() => OnTheoryStudyButtonClick?.Invoke());
		helpButton.onClick.AddListener(() => OnHelpButtonClick?.Invoke());
		quitButton.onClick.AddListener(() => OnQuitButtonClick?.Invoke());
		videoPlayerButton.onClick.AddListener(() => OnVideoPlayerClick?.Invoke());
		loginButton.onClick.AddListener(() => OnLoginButtonClick?.Invoke());
	}

	private void OnEnable()
	{
		if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.UserName.Length > 1)
			nameText.text = QuestStorage.Instance.UserName;
		else nameText.text = "Введите имя";
	}
}
