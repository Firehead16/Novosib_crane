using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private Button ControlsButton = null;
    [SerializeField] private Button ResetButton = null;
    [SerializeField] private Button ExitButton = null;
    
    // Start is called before the first frame update
    void Start()
    {
        ControlsButton.onClick.AddListener(ShowControls);
        ResetButton.onClick.AddListener(Restart);
        ExitButton.onClick.AddListener(Exit);
    }

    void ShowControls()
    {
        //TODO подсказки по управлению
    }

    //TODO останавливать запись
    void Restart()
    {
        QuestControl.Instance.ResetOrExit(1);
        //SceneManager.LoadScene(1);
    }

    void Exit()
    {
        QuestControl.Instance.ResetOrExit(0);
        //SceneManager.LoadScene(0); // Загрузить главное меню
    }

    private void OnEnable() => Time.timeScale = 0;

    private void OnDisable() => Time.timeScale = 1;
}
