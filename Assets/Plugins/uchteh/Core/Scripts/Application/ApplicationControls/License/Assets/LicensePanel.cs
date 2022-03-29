using System;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
public class LicensePanel : Panel
{
    public event Action OnSaveLicenseButtonClick;
    public event Action OnCancelLicenseButtonClick;

    [SerializeField] private Button saveLicenseButton = null;
    [SerializeField] private Button cancelLicenseButton = null;
    [SerializeField] private InputField productIdInputField= null;
    [SerializeField] private InputField serialKeyInputField = null;
    [SerializeField] private Text licenseFilePath = null;

    private string productId;
    public string ProductIdText { set => productId= productIdInputField.text = value; }
    public string SerialKeyText { get => serialKeyInputField.text; set => serialKeyInputField.text = value; }
    public string LicenseFilePathText { set => licenseFilePath.text = value; }

    /// <summary>
    /// На кнопки и поля подписываются события
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        saveLicenseButton.onClick.AddListener(()=>{ OnSaveLicenseButtonClick?.Invoke();});
        cancelLicenseButton.onClick.AddListener(()=>{ OnCancelLicenseButtonClick?.Invoke();});
        productIdInputField.onEndEdit.AddListener(s => UpdateProductId());
    }


    private void UpdateProductId()
    {   
        ProductIdText = productId;
    }
}



