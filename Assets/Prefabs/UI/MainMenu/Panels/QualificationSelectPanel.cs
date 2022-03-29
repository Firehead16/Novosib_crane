using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class QualificationSelectPanel : MonoBehaviour
{
	public event Action OnQual1ButtonClick;
	public event Action OnQual2ButtonClick;
	public event Action OnQual3ButtonClick;
	public event Action OnQual4ButtonClick;
	public event Action OnBackToMenu;
	
	[SerializeField]
	private Button qual1Button = null;

	[SerializeField]
	private Button qual2Button = null;

	[SerializeField]
	private Button qual3Button = null;

	[SerializeField]
	private Button qual4Button = null;

	[SerializeField]
	private Button backToCranes = null;

	[Header("1 Квалификация")]
	[ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "SceneName")]
	public List<ToggleScene> Qual1Toggles = new List<ToggleScene>();

	public ToggleScene Qual1StartToggles;

	public ToggleScene Qual1FinishToggles;

	[Header("2 Квалификация")]
	[ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "SceneName")]
	public List<ToggleScene> Qual2Toggles = new List<ToggleScene>();

	public ToggleScene Qual2StartToggles;

	public ToggleScene Qual2FinishToggles;

	[Header("3 Квалификация")]
	[ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "SceneName")]
	public List<ToggleScene> Qual3Toggles = new List<ToggleScene>();

	public ToggleScene Qual3StartToggles;

	public ToggleScene Qual3FinishToggles;

	[Header("4 Квалификация")]
	[ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "SceneName")]
	public List<ToggleScene> Qual4Toggles = new List<ToggleScene>();

	private List<ToggleScene> allToggles = new List<ToggleScene>();


	private void Start()
	{
		allToggles.AddRange(Qual1Toggles);
		allToggles.AddRange(Qual2Toggles);
		allToggles.AddRange(Qual3Toggles);
		allToggles.AddRange(Qual4Toggles);

		qual1Button?.onClick.AddListener(() => OnQual1ButtonClick?.Invoke());
		qual2Button?.onClick.AddListener(() => OnQual2ButtonClick?.Invoke());
		qual3Button?.onClick.AddListener(() => OnQual3ButtonClick?.Invoke());
		qual4Button?.onClick.AddListener(() => OnQual4ButtonClick?.Invoke());

		backToCranes.onClick.AddListener((() => OnBackToMenu?.Invoke()));
	}
	
	public void ClearToggles()
	{
		foreach (ToggleScene toggleScene in allToggles)
		{
			toggleScene.Toggle.gameObject.SetActive(true);
			toggleScene.Toggle.isOn = false;
		}
	}

}
