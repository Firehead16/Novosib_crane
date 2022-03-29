using System;
using UnityEngine;
using UnityEngine.UI;

public class TheoryStudyPanel : MonoBehaviour
{
    public event Action OnBackToMainMenuButtonClick;

    public event Action OnDocsButtonClick;

    public event Action OnEmergencySituationsButtonClick;

    [SerializeField]
    private Button backButton = null;

    [SerializeField]
    private Button docsButton = null;

    [SerializeField]
    private Button emergencySituationsButton = null;


    private void Start()
    {
        backButton.onClick.AddListener(() => OnBackToMainMenuButtonClick?.Invoke());
        docsButton.onClick.AddListener(() => OnDocsButtonClick?.Invoke());
        emergencySituationsButton.onClick.AddListener(() => OnEmergencySituationsButtonClick?.Invoke());
    }
}
