using System;
using UnityEngine;
using UnityEngine.UI;

public enum DemoType
{
    Destruction,
    Fire,
    DropOfCargo
}


public class EmergenceSituationsPanel : MonoBehaviour
{
    public event Action OnBackToMainMenuButtonClick;

    public event Action<DemoType> OnStartDemoButtonClick;


    [SerializeField]
    private Button backButton = null;

    [SerializeField]
    private Button destructionButton = null;

    [SerializeField]
    private Button fireButton = null;

    [SerializeField]
    private Button dropOfCargoButton = null;


    private void Start()
    {
        backButton.onClick.AddListener(() => OnBackToMainMenuButtonClick?.Invoke());
        destructionButton.onClick.AddListener(() => OnStartDemoButtonClick?.Invoke(DemoType.Destruction));
        fireButton.onClick.AddListener(() => OnStartDemoButtonClick?.Invoke(DemoType.Fire));
        dropOfCargoButton.onClick.AddListener(() => OnStartDemoButtonClick?.Invoke(DemoType.DropOfCargo));
    }
}
