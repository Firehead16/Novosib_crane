using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneControl : MonoBehaviour
{
    public static CraneControl Instance;
    
    public struct CRANESTATE
    {
        public bool CraneControlEnabled; //G ключ вкл управл. выкл
        public bool CraneReadyEnabled; //G управление включено
        public bool CraneBrakeFail; //R отказ тормозов
        public bool CraneOverWeight; //Y перегруз 80
        public bool CraneOverWeight2; //R перегруз 100
        public bool CraneMoveEnabled; //G Производится передвижение
        public bool CraneTowerEnabled; //G Включение поворота башни
        public bool CraneArrowEnabled; //G Включение вылета стрелы
        public bool CraneRopeEnabled; //G Включение контроля лебёдки
        public bool CraneRailEnabled; //G Включение перемещения по рельсам
        public bool CraneControllerFail; //R неполадки с контроллером

        public bool CraneRopeFail; //R отказ системы лебёдок
        public bool CraneArrowFail; //R отказ системы вылета стрелы
        public bool CraneTowerFail; //R отказ системы поворота башни
        public bool CraneRailFail; //R отказ системы передвижения по рельсам

        public bool CraneSlab1Green; //G Слабина лебёдок
        public bool CraneSlab2Green; //G
        public bool CraneSlab1Red; //R
        public bool CraneSlab2Red; //R
    };
    
    public static CRANESTATE craneState;

    //Аниматор стрелы для отображения на ОГП вылета
    public Animator Arrow = null;

    public GrabControl CurrentGrabControl = null;

    [SerializeField] private List<GrabControl> _grabControls = new List<GrabControl>();

    public Transform Base;
    
    //private bool controlsStarted = false;

    private Coroutine startControlsCoroutine = null;
    public Coroutine testLEDsCoroutine = null;
    public Coroutine fqtestLEDsCoroutine = null;

    public InputActionAsset inputActionAsset;
    
    public bool isEmegencyStop;
    
    public enum RopeSpeed
    {
        FirstSpeed,
        SecondSpeed,
        ThirdSpeed
    }

    public RopeSpeed CurrentRopeSpeed = RopeSpeed.FirstSpeed;
    
    IniReader aIniReader;
    IniWriter aIniWriter;
    string AppPath = "";
    
    public static int JoystickA, JoystickB, JoystickC; // A=1 B=0 C=2

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() => inputActionAsset.Enable();

    private void OnDisable() => inputActionAsset.Disable();
    
    private IEnumerator EnableControls()
    {
        #region обнуляем стартовые значения
        craneState.CraneReadyEnabled = false;
        craneState.CraneBrakeFail = false;
        craneState.CraneMoveEnabled = false;
        craneState.CraneOverWeight = false;
        craneState.CraneOverWeight2 = false;
        craneState.CraneTowerEnabled = false;
        craneState.CraneRopeEnabled = false;
        craneState.CraneArrowEnabled = false;
        craneState.CraneRailEnabled = false;
        craneState.CraneControllerFail = false;
        craneState.CraneSlab1Green = false;
        craneState.CraneSlab2Green = false;
        craneState.CraneSlab1Red = false;
        craneState.CraneSlab2Red = false;
        #endregion

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Поворот крана включен");
        craneState.CraneTowerEnabled = true;
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Вылет стрелы включен");
        craneState.CraneArrowEnabled = true;
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Рельсы и лебёдка включены");
        craneState.CraneRopeEnabled = true;
        craneState.CraneRailEnabled = true;

        craneState.CraneArrowEnabled = true;

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Управление включено");
        craneState.CraneReadyEnabled = true;

        startControlsCoroutine = null;
    }
    
    void ChangeAllSystemState(bool state)
    {
        craneState.CraneControlEnabled = state;
        craneState.CraneReadyEnabled = state;
        craneState.CraneBrakeFail = state;
        craneState.CraneOverWeight = state;
        craneState.CraneOverWeight2 = state;
        craneState.CraneMoveEnabled = state;
        craneState.CraneTowerEnabled = state;
        craneState.CraneArrowEnabled = state;
        craneState.CraneRopeEnabled = state;
        craneState.CraneRailEnabled = state;
        craneState.CraneControllerFail = state;
        craneState.CraneRopeFail = state;
        craneState.CraneArrowFail = state;
        craneState.CraneTowerFail = state;
        craneState.CraneRailFail = state;

        craneState.CraneSlab1Green = state;
        craneState.CraneSlab2Green = state;
        craneState.CraneSlab1Red = state;
        craneState.CraneSlab2Red = state;
    }

    private void Start()
    {
        AppPath = Path.GetDirectoryName(Application.dataPath);
        aIniReader = new IniReader(AppPath + "\\Config\\crane.ini");
        aIniWriter = new IniWriter(AppPath + "\\Config\\crane.ini");

        string JoySettings = "";
        if (File.Exists(AppPath + "\\Config\\sticks.ini"))
        {
            JoySettings = File.ReadLines(AppPath + "\\Config\\sticks.ini").First();
            Debug.Log("From File " + JoySettings);
            string[] subStrings = JoySettings.Split(' ');
            JoystickA = Convert.ToInt32(subStrings[0]);
            JoystickB = Convert.ToInt32(subStrings[1]);
            JoystickC = Convert.ToInt32(subStrings[2]);
            Debug.Log("JoyA " + JoystickA + " JoyB " + JoystickB + " JoyC " + JoystickC);
        }
        
        
        /*
        Индексы

        Greifer,            // Грейфер
        Traverse,           // Траверса
        Autotraverse,		// Автотраверса
        Spreder,            // Спредер
        TextileSlings,      // Текстильные стропы
        LineSlings,			// Линейные стропы
        RopeSlings,         // Канатные стропы 
        */
        
        //Включаем выбранный грузозахват
        if(QuestStorage.Instance != null)
            CurrentGrabControl = _grabControls[(int) QuestStorage.Instance.LiftingDevice];
        else if(CurrentGrabControl.SafeIsUnityNull()) CurrentGrabControl = _grabControls[6]; //если отсутствует QuestsStorage включать грейфер
        CurrentGrabControl.gameObject.SetActive(true);
    }

    private void Update()
    {
        #region Ключ и кнопки включения / выключения

        if (!inputActionAsset.SafeIsUnityNull() && inputActionAsset.FindAction("KeyOn").ReadValue<float>() > 0 &&
            !craneState.CraneControlEnabled) craneState.CraneControlEnabled = true;
        
        if (!inputActionAsset.SafeIsUnityNull() && inputActionAsset.FindAction("WorkOn").ReadValue<float>() > 0 &&
            craneState.CraneControlEnabled && !craneState.CraneReadyEnabled &&
            startControlsCoroutine == null) startControlsCoroutine = StartCoroutine(EnableControls());

        if(Joystick.all.Count > 1 && !inputActionAsset.SafeIsUnityNull() && 
        inputActionAsset.FindAction("KeyOn").ReadValue<float>() < 0.1f && craneState.CraneControlEnabled){
            ChangeAllSystemState(false);
        }

        if (Joystick.all.Count == 0 && !inputActionAsset.SafeIsUnityNull() && inputActionAsset.FindAction("KeyOff").ReadValue<float>() > 0)
        {
            ChangeAllSystemState(false);
        }
        
        if (!inputActionAsset.SafeIsUnityNull() && inputActionAsset.FindAction("WorkOff").ReadValue<float>() > 0)
        {
            ChangeAllSystemState(false);
            craneState.CraneControlEnabled = true;
        }

        #endregion

        #region Аварийный стоп

        if (!inputActionAsset.SafeIsUnityNull() && inputActionAsset.FindAction("EmergencyStop").ReadValue<float>() > 0)
        {
            ChangeAllSystemState(false);
            craneState.CraneControlEnabled = true;
            isEmegencyStop = true;
        }
        if (isEmegencyStop && !inputActionAsset.SafeIsUnityNull() && inputActionAsset.FindAction("EmergencyStop").ReadValue<float>() == 0)
            isEmegencyStop = false;

        #endregion

        #region Индикатор движения крана

        if (fqtestLEDsCoroutine == null && Hardware.Instance.IsConnected())
        {
            craneState.CraneMoveEnabled = craneState.CraneReadyEnabled &&
                  (Mathf.Abs(inputActionAsset.FindAction("RailsMovement").ReadValue<float>()) > 0.05f ||
                   Mathf.Abs(inputActionAsset.FindAction("ArrowMovement").ReadValue<float>()) > 0.05f ||
                   Mathf.Abs(inputActionAsset.FindAction("RotationMovement").ReadValue<float>()) > 0.05f ||
                   Mathf.Abs(inputActionAsset.FindAction("RopeLengthChanging").ReadValue<float>()) > 0.05f ||
                   Mathf.Abs(Joystick.all[CraneControl.JoystickA].stick.x.ReadValue()) > 0.05f ||
                   Mathf.Abs(Joystick.all[CraneControl.JoystickA].stick.y.ReadValue()) > 0.05f ||
                   Mathf.Abs(Joystick.all[CraneControl.JoystickB].stick.x.ReadValue()) > 0.05f ||
                   Mathf.Abs(Joystick.all[CraneControl.JoystickB].stick.y.ReadValue()) > 0.05f);
        }

        #endregion
        
        //скорости лебёдки и тест индикации
        if (craneState.CraneControlEnabled && !inputActionAsset.SafeIsUnityNull())
        {
            if (inputActionAsset.FindAction("FirstSpeed").ReadValue<float>() > 0)
                CurrentRopeSpeed = RopeSpeed.FirstSpeed;
            if (inputActionAsset.FindAction("SecondSpeed").ReadValue<float>() > 0)
                CurrentRopeSpeed = RopeSpeed.SecondSpeed;
            if (inputActionAsset.FindAction("ThirdSpeed").ReadValue<float>() > 0)
                CurrentRopeSpeed = RopeSpeed.ThirdSpeed;
            if (testLEDsCoroutine == null && inputActionAsset.FindAction("TestLEDs").ReadValue<float>() > 0)
                testLEDsCoroutine = StartCoroutine(Hardware.Instance.Test());
            // if (testLEDsCoroutine != null && fqtestLEDsCoroutine == null)
            //     fqtestLEDsCoroutine = StartCoroutine(fq_testLEDs());
        }
    }

    /*IEnumerator fq_testLEDs()
    {
        ChangeAllSystemState(true);
        yield return new WaitForSeconds(2);
        craneState.CraneBrakeFail = false;
        craneState.CraneOverWeight = false;
        craneState.CraneOverWeight2 = false;
        craneState.CraneControllerFail = false;
        craneState.CraneRopeFail = false;
        craneState.CraneArrowFail = false;
        craneState.CraneTowerFail = false;
        craneState.CraneRailFail = false;

        fqtestLEDsCoroutine = null;
    }*/

    private void OnDestroy() => ChangeAllSystemState(false);
}
