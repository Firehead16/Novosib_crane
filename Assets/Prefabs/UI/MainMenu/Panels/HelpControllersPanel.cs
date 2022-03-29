using System;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

public class HelpControllersPanel : MonoBehaviour
{
	public event Action OnBackToMainMenuButtonClick;

	[SerializeField]
	private WindowManager windowManager = null;

    [SerializeField]
	private Button backButton = null;

	private void Start()
    {
        backButton.onClick.AddListener(() => OnBackToMainMenuButtonClick?.Invoke());
		windowManager.OpenFirstTab();
    }

	private void OnEnable()
	{
		windowManager.OpenFirstTab();
	}
}
