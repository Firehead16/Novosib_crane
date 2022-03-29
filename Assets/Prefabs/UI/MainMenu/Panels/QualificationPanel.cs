using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QualificationPanel : MonoBehaviour
{
	public event Action<ToggleScene,int> OnStartedQuestsButtonClick;
	public event Action OnBackToQualButtonClick;
	public event Action<ToggleScene, bool> OnToggleChanged;

	[SerializeField]
	private Button startButton = null;

	[SerializeField]
	private QualificationSelectPanel qualPanel = null;

	[SerializeField]
	private int QualificationNumber = 0;

	[SerializeField]
	private Button backToQualButton = null;

	[SerializeField]
	private Button StartExamButton = null;

	[SerializeField]
	private Toggle toggleAdvice = null;
	public Toggle ToogleAdvice => toggleAdvice;

	private void Start()
	{
		//toggleAdvice.onValueChanged.AddListener(value => QuestStorage.Instance.QuestLog.IsNeedAdvice = value);
		backToQualButton.onClick.AddListener(() => OnBackToQualButtonClick?.Invoke());

		switch (QualificationNumber)
		{
			case 1:
				{
					qualPanel.Qual1Toggles.ForEach(t => t.Toggle.onValueChanged.AddListener(state => OnToggleChanged?.Invoke(t, state)));
					startButton.onClick.AddListener(() => OnStartedQuestsButtonClick?.Invoke(qualPanel.Qual1Toggles.Single(q => q.Toggle.isOn),1));
				}
				break;
			case 2:
				{
					qualPanel.Qual2Toggles.ForEach(t => t.Toggle.onValueChanged.AddListener(state => OnToggleChanged?.Invoke(t, state)));
					startButton.onClick.AddListener(() => OnStartedQuestsButtonClick?.Invoke(qualPanel.Qual2Toggles.Single(q => q.Toggle.isOn),2));
				}
				break;
			case 3:
				{
					qualPanel.Qual3Toggles.ForEach(t => t.Toggle.onValueChanged.AddListener(state => OnToggleChanged?.Invoke(t, state)));
					startButton.onClick.AddListener(() => OnStartedQuestsButtonClick?.Invoke(qualPanel.Qual3Toggles.Single(q => q.Toggle.isOn),3));
				}
				break;
			case 4:
				{
					qualPanel.Qual4Toggles.ForEach(t => t.Toggle.onValueChanged.AddListener(state => OnToggleChanged?.Invoke(t, state)));
					startButton.onClick.AddListener(() => OnStartedQuestsButtonClick?.Invoke(qualPanel.Qual4Toggles.Single(q => q.Toggle.isOn),4));
				}
				break;
		}
	}


	private void Update()
	{
		switch (QualificationNumber)
		{
			case 1:
				{
					StartExamButton.gameObject.SetActive(qualPanel.Qual1Toggles.Any(x => x.Toggle.isOn));
				}
				break;
			case 2:
				{
					StartExamButton.gameObject.SetActive(qualPanel.Qual2Toggles.Any(x => x.Toggle.isOn));
				}
				break;
			case 3:
				{
					StartExamButton.gameObject.SetActive(qualPanel.Qual3Toggles.Any(x => x.Toggle.isOn));
				}
				break;
			case 4:
				{
					StartExamButton.gameObject.SetActive(qualPanel.Qual4Toggles.Any(x => x.Toggle.isOn));
				}
				break;
		}
	}
}