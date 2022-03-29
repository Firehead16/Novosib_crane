using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CraneSelectPanel : MonoBehaviour
{
	public event Action<List<ToggleScene>> OnStartedQuestsButtonClick;
	public event Action OnBridgeClick;
    public event Action OnCastingClick;
    public event Action OnBackToMainMenuButtonClick;

    [SerializeField]
    private Button bridgeButtonStart = null;

    [SerializeField]
    private Button castingButtonStart = null;

    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "SceneName")]
    public List<ToggleScene> BridgeToggles = new List<ToggleScene>();

    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "SceneName")]
    public List<ToggleScene> CastingToggles = new List<ToggleScene>();

    private List<ToggleScene> allToggleScenes = new List<ToggleScene>();

    [SerializeField]
    private Button bridgeButton = null;

    [SerializeField]
    private Button castingButton = null;

    [SerializeField]
    private Button backButton = null;

    void Start()
    {
	    allToggleScenes.AddRange(BridgeToggles);
	    allToggleScenes.AddRange(CastingToggles);

	    bridgeButtonStart?.onClick.AddListener(() => OnStartedQuestsButtonClick?.Invoke(BridgeToggles));
	    castingButtonStart?.onClick.AddListener(() => OnStartedQuestsButtonClick?.Invoke(CastingToggles));

        bridgeButton.onClick.AddListener(() => OnBridgeClick?.Invoke());
        castingButton.onClick.AddListener(() => OnCastingClick?.Invoke());
        backButton.onClick.AddListener(() => OnBackToMainMenuButtonClick?.Invoke());
    }


    public void ClearToggles()
    {
	    foreach (ToggleScene toggleScene in allToggleScenes)
	    {
		    toggleScene.Toggle.gameObject.SetActive(true);
		    toggleScene.Toggle.isOn = false;
	    }
    }
}
