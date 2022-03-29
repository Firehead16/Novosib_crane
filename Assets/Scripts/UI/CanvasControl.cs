using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CanvasControl : MonoBehaviour
{
    public static CanvasControl Instance;

    [SerializeField] private GameObject angleDiffPanel;
    [SerializeField] private TMP_Text angleDiffText;

    [SerializeField] private GameObject bulkProgressPanel;
    [SerializeField] private TMP_Text bulkProgressText;

    [SerializeField] private GameObject OGP;

    [SerializeField] private GameObject F13CameraHelp;
    [SerializeField] private GameObject F2CameraHelp;
    [SerializeField] private GameObject F4CameraHelp;

    [SerializeField] private GameObject PauseMenu;

    [SerializeField] private TMP_Text timer;
    private int examTimer;

    [SerializeField] private TMP_Text workInfo;
    [SerializeField] private TMP_Text ropeSpeedInfo;

    private Coroutine onlyCoroutine = null;

    [SerializeField] private GameObject DescriptionPanel;
    [SerializeField] private TMP_Text DescriptionText;

    [SerializeField] private GameObject F1Crosshair;
    [SerializeField] private GameObject F4Crosshair;

    private void Awake()
    {
        //������ ����� �������
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        //����� ������� ������������
        void OnOGPPerformed(InputAction.CallbackContext obj) => onlyCoroutine ??= StartCoroutine(ShowOGP());

        CraneControl.Instance.inputActionAsset.FindAction("ShowOGP").performed += OnOGPPerformed;

        //����� ������� ������������
        void OnPausePerformed(InputAction.CallbackContext context) => onlyCoroutine ??= StartCoroutine(ShowPauseMenu());

        CraneControl.Instance.inputActionAsset.FindAction("Pause").performed += OnPausePerformed;

        #region ��������� ��� ������
        CraneControl.Instance.inputActionAsset.FindAction("F1Camera").performed += context => SetCameraHelp(1);
        CraneControl.Instance.inputActionAsset.FindAction("F2Camera").performed += context => SetCameraHelp(2);
        CraneControl.Instance.inputActionAsset.FindAction("F3Camera").performed += context => SetCameraHelp(1);
        CraneControl.Instance.inputActionAsset.FindAction("F4Camera").performed += context => SetCameraHelp(4);
        SetCameraHelp(4);
        #endregion

        examTimer = QuestStorage.Instance.Timer * 60;

        F4Crosshair.SetActive(true);

    }

    public void SetCrosshair(int cameraNum)
    {
        F1Crosshair.SetActive(cameraNum == 1);
        F4Crosshair.SetActive(cameraNum == 4);
    }

    private void Update()
    {
        switch (QuestStorage.Instance.questMode)
        {
            case QuestMode.Learn:
                timer.text = QuestControl.GetTime();
                break;
            case QuestMode.Exam:
                if (examTimer > 0)
                {
                    var time = TimeSpan.FromSeconds(examTimer - Time.timeSinceLevelLoad);
                    timer.text = time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2");
                }
                else timer.text = QuestControl.GetTime();
                break;
        }

        if (QuestStorage.Instance.QuestType == Quest.Bulk)
        {
            if (!bulkProgressPanel.activeSelf) bulkProgressPanel.SetActive(true);
            else bulkProgressText.text = "Выполнено: " + QuestControl.Instance.placeTrigger.BulkProgress + "%";
        }

        ropeSpeedInfo.text = "Скорость лебедки: " + ((int)CraneControl.Instance.CurrentRopeSpeed + 1);

        if (CraneControl.craneState.CraneControlEnabled)
        {
            if (CraneControl.craneState.CraneReadyEnabled)
            {
                workInfo.text = "Кран включен";
            }
            else workInfo.text = "Кран выключен";
        }
        else workInfo.text = "Ключ выключен";

        DescriptionPanel.SetActive(DescriptionText.text != "");
    }

    //��������� ��������� �� ���������� �������
    void SetCameraHelp(int value)
    {
        switch (value)
        {
            case 1:
            case 3:
                F13CameraHelp.SetActive(true);
                F2CameraHelp.SetActive(false);
                F4CameraHelp.SetActive(false);
                break;
            case 2:
                F2CameraHelp.SetActive(true);
                F13CameraHelp.SetActive(false);
                F4CameraHelp.SetActive(false);
                break;
            case 4:
                F4CameraHelp.SetActive(true);
                F13CameraHelp.SetActive(false);
                F2CameraHelp.SetActive(false);
                break;
        }
    }

    //��������� ����������� ������� ������������
    IEnumerator ShowOGP()
    {
        OGP.SetActive(!OGP.activeSelf);
        yield return new WaitForSeconds(1);
        onlyCoroutine = null;
    }

    //��������� ����������� ���� �����
    IEnumerator ShowPauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        //yield return new WaitForSeconds(1);
        onlyCoroutine = null;
        yield break;
    }

    //��������� / ���������� ����������� ������� ����� ��� ����������� �����
    public void SetAngleDiff(float diff)
    {
        if (!angleDiffPanel.activeSelf) angleDiffPanel.SetActive(true);
        angleDiffText.text = "Разница углов: " + diff;
    }

    public void OffAngleDiff()
    {
        if (angleDiffPanel.activeSelf) angleDiffPanel.SetActive(false);
    }
}
