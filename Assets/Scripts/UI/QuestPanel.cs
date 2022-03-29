using System.Collections;
using System.Collections.Generic;
using System;
using Core.Gameplay.Questing;
using Core.Ui;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private DropdownPlaceListChanger placeListChanger;
    [SerializeField] private DropdownGrabChanger grabChanger;

    [SerializeField] private CustomDropdown CargoType2Quest;
    
    [SerializeField] private Button startButton = null; 

    [SerializeField] private ToggleDescriptionPanel descriptionPanel;

    /*[SerializeField] private Image ScreenshotImage;

    [SerializeField] private Text QuestDescriptionText;
    
    [SerializeField] private List<QuestDescription> DiscriptionList = new List<QuestDescription>();*/

    [SerializeField] private Button backToMainMenuButton = null;

    [SerializeField] private TMP_InputField timer = null;

    [SerializeField] private HorizontalSelector mode = null;

    [SerializeField] private Slider fog, wind, waves;

    [SerializeField] private Toggle isRecord = null;

    private bool buttonsState = true;

    private int lastSelector;

    private QuestDescription currentDescription;

    public event Action OnBackToMainButtonClick;
    public event Action OnStartQuestButtonClick;

    void Start()
    {
        backToMainMenuButton.onClick.AddListener(() => OnBackToMainButtonClick?.Invoke());
        startButton.onClick.AddListener(() => OnStartQuestButtonClick?.Invoke());
        OnStartQuestButtonClick += StartQuest;

        lastSelector = mode.index;
    }

    void HideButtonsUnselect(bool state)
    {
        startButton.gameObject.SetActive(state);
        descriptionPanel.gameObject.SetActive(state);
        mode.gameObject.SetActive(state);
        isRecord.gameObject.SetActive(state);
    }

    /*void UpdateDescription(Toggle i)
    {
        for (int j = 0; j < 4; j++)
        {
            if (i == placeListChanger.questToggles[j] && currentDescription != DiscriptionList[j])
            {
                currentDescription = DiscriptionList[j];
                descriptionPanel. // = currentDescription.QuestScreenshot;
                
            }
        }
    }*/
    
    void Update()
    {
        if (buttonsState != !placeListChanger._activeToggle.SafeIsUnityNull())
        {
            HideButtonsUnselect(!placeListChanger._activeToggle.SafeIsUnityNull());
            buttonsState = !placeListChanger._activeToggle.SafeIsUnityNull();
        }
        
        timer.gameObject.SetActive(!placeListChanger._activeToggle.SafeIsUnityNull() && mode.index == 1);

        if (mode.index != lastSelector)
        {
            if (!isRecord.isOn && mode.index == 1) isRecord.isOn = true; //экзамен
            if (isRecord.isOn && mode.index == 0) isRecord.isOn = false; //обучение
            lastSelector = mode.index;
        }

        /*if (!placeListChanger._activeToggle.SafeIsUnityNull())
        {
            UpdateDescription(placeListChanger._activeToggle);
        }*/

        //Debug.Log("Mode " + mode.index); 1 - Экзамен, 0 - Обучение
    }

    void StartQuest()
    {
        #region Первое задание

        if (placeListChanger._activeToggle == placeListChanger.questToggles[0])
        {
            //UpdateDescription(0);
            QuestStorage.Instance.QuestType = Quest.Pipes;
            //записываем место разгрузки
            switch (placeListChanger.GetComponent<CustomDropdown>().selectedItemIndex)
            {
                case 0:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.RailwayPlatform;
                    break;
                case 1:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.Auto;
                    break;
                case 2:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.Pontoon;
                    break;
                case 3:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.SecondVessel;
                    break;
            }
            //записываем грузозахват
            switch (grabChanger.GetComponent<CustomDropdown>().selectedItemIndex)
            {
                case 0:
                    QuestStorage.Instance.LiftingDevice = LiftingDevice.Traverse;
                    break;
                case 1:
                    QuestStorage.Instance.LiftingDevice = LiftingDevice.LineSlings;
                    break;
            }
        }

        #endregion

        #region Второе задание

        else if (placeListChanger._activeToggle == placeListChanger.questToggles[1])
        {
            //UpdateDescription(1);
            QuestStorage.Instance.QuestType = Quest.Containers;
            

            if (CargoType2Quest.index == 0) //выбраны ген. грузы
            {
                QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.SecondVessel;
                QuestStorage.Instance.LiftingDevice = LiftingDevice.RopeSlings;
            }
            else
            {
                //записываем место разгрузки
                switch (placeListChanger.GetComponent<CustomDropdown>().selectedItemIndex)
                {
                    case 0:
                        QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.RailwayPlatform;
                        break;
                    case 1:
                        QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.Auto;
                        break;
                    case 2:
                        QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.Pontoon;
                        break;
                    case 3:
                        QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.AnotherContainer;
                        break;
                }
                
                //записываем грузозахват
                switch (grabChanger.GetComponent<CustomDropdown>().selectedItemIndex)
                {
                    case 0:
                        QuestStorage.Instance.LiftingDevice = LiftingDevice.Spreader;
                        break;
                    case 1:
                        QuestStorage.Instance.LiftingDevice = LiftingDevice.RopeSlings;
                        break;
                }
            }

            //записываем тип груза
            switch (CargoType2Quest.selectedItemIndex)
            {
                case 0:
                    QuestStorage.Instance.CargoType = CargoType.GenCargo;
                    break;
                case 1:
                    QuestStorage.Instance.CargoType = CargoType.Container20Ft;
                    break;
                case 2:
                    QuestStorage.Instance.CargoType = CargoType.Container40Ft;
                    break;
            }
        }

        #endregion

        #region Третье задание

        else if (placeListChanger._activeToggle == placeListChanger.questToggles[2])
        {
            //UpdateDescription(2);
            QuestStorage.Instance.QuestType = Quest.Auto;
            //записываем место разгрузки
            switch (placeListChanger.GetComponent<CustomDropdown>().selectedItemIndex)
            {
                case 0:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.HoldToPontoon;
                    break;
                case 1:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.PontoonToHold;
                    break;
            }
            //записываем грузозахват
            QuestStorage.Instance.LiftingDevice = LiftingDevice.AutoTraverse;
        }

        #endregion

        #region Четвёртое задание

        else if (placeListChanger._activeToggle == placeListChanger.questToggles[3])
        {
            //UpdateDescription(3);
            QuestStorage.Instance.QuestType = Quest.Bulk;
            //записываем место разгрузки
            switch (placeListChanger.GetComponent<CustomDropdown>().selectedItemIndex)
            {
                case 0:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.PontoonToHold;
                    break;
                case 1:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.HoldToPontoon;
                    break;
                case 2:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.RailwayPlatform;
                    break;
                case 3:
                    QuestStorage.Instance.QuestCargoPlace = QuestCargoPlace.SecondVessel;
                    break;
            }
            //записываем грузозахват
            QuestStorage.Instance.LiftingDevice = LiftingDevice.Greifer;
        }

        #endregion

        //Устанавливаем время (если задано)
        if (timer.text.Length > 0) QuestStorage.Instance.Timer = Convert.ToInt32(timer.text);

        QuestStorage.Instance.questMode = mode.index == 1 ? QuestMode.Exam : QuestMode.Learn;

        #region Погодные условия

        QuestStorage.Instance.fog = fog.value * 0.05f;
        QuestStorage.Instance.wind = wind.value;
        QuestStorage.Instance.waves = waves.value;

        #endregion
        
        //Запись прохождения
        QuestStorage.Instance.RecordThis = isRecord.isOn;
        
        //QuestStorage.Instance.QuestLog.SceneList.Enqueue("Scenes/QuestScene");
        SceneManager.LoadScene("Scenes/QuestScene");
    }
}
