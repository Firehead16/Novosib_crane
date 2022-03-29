using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraneTaskPanel : MonoBehaviour
{
	public event Action OnBackToQualButtonClick;
	public event Action<ToggleScene, bool> OnToggleChanged;

	[SerializeField]
	private CraneSelectPanel craneSelectPanel = null;

	[SerializeField]
	private int QualificationNumber = 0;

	[SerializeField]
	private Button backToQualButton = null;

	[SerializeField]
	private Button StartExamButton = null;

	private void Start()
	{
		backToQualButton.onClick.AddListener(() => OnBackToQualButtonClick?.Invoke());

		switch (QualificationNumber)
		{
			case 1:
				{
					craneSelectPanel.BridgeToggles.ForEach(t => t.Toggle.onValueChanged.AddListener(state => OnToggleChanged?.Invoke(t, state)));
				}
				break;
			case 2:
				{
					craneSelectPanel.CastingToggles.ForEach(t => t.Toggle.onValueChanged.AddListener(state => OnToggleChanged?.Invoke(t, state)));
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
					StartExamButton.gameObject.SetActive(craneSelectPanel.BridgeToggles.Any(x => x.Toggle.isOn));
				}
				break;
			case 2:
				{
					StartExamButton.gameObject.SetActive(craneSelectPanel.CastingToggles.Any(x => x.Toggle.isOn));
				}
				break;
		}
	}
}