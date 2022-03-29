using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public event Action OnApplyName;
    
    [SerializeField]
    private Button applyButton = null;
    
    public TMP_Text InputName;

    private void Start()
    {
        applyButton.onClick.AddListener(() => OnApplyName?.Invoke());
    }
}
