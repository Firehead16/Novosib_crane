using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Obi;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OGP : MonoBehaviour
{
    //[SerializeField]
    //private Transform PortalCrane, Avtostrop, Greifer;//, QuestController;
    [SerializeField]
    public GameObject Leb1Up, Leb1Down, Leb2Up, Leb2Down, ArrForward, ArrBack, RotR, RotL;
    [SerializeField]
    public GameObject FactWeight, Netto, Work, Danger, Stop, Sovm1, Sovm2, Slab1, Slab2;
    [SerializeField]
    private GameObject w20, w40, w60, w80, w100, w120;
    [SerializeField]
    public TMP_Text TextWeight, TextPullout, TextWorkmode;

    private List<GameObject> AllIndicators = new List<GameObject>();
    private List<GameObject> OverweightIndicators = new List<GameObject>();
    private int framesToRefresh;
    private double correctedWeight;
    private float pullout;
    private float overweight;
    private static readonly int PullOutState = Animator.StringToHash("PullOutState");
    
    // [SerializeField]
    // List<ObiRope> GrabRopes = new List<ObiRope>();

    private float tension = 0;


    // Start is called before the first frame update
    void Start()
    {
        #region ��������� ������ ���� �����������
        AllIndicators.Add(Leb1Up);
        AllIndicators.Add(Leb1Down);
        AllIndicators.Add(Leb2Up);
        AllIndicators.Add(Leb2Down);
        AllIndicators.Add(ArrForward);
        AllIndicators.Add(ArrBack);
        AllIndicators.Add(RotR);
        AllIndicators.Add(RotL);

        AllIndicators.Add(FactWeight);
        AllIndicators.Add(Netto);
        AllIndicators.Add(Work);
        AllIndicators.Add(Danger);
        AllIndicators.Add(Stop);
        AllIndicators.Add(Sovm1);
        AllIndicators.Add(Sovm2);

        AllIndicators.Add(w20);
        AllIndicators.Add(w40);
        AllIndicators.Add(w60);
        AllIndicators.Add(w80);
        AllIndicators.Add(w100);
        AllIndicators.Add(w120);
        #endregion

        #region ��������� ���������� ���������
        OverweightIndicators.Add(w20);
        OverweightIndicators.Add(w40);
        OverweightIndicators.Add(w60);
        OverweightIndicators.Add(w80);
        OverweightIndicators.Add(w100);
        OverweightIndicators.Add(w120);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        /*if (GrabRopes.Count == 0) GetTension();
        else tension = AvgTension();*/

        if (CraneControl.craneState.CraneReadyEnabled)
        {
            if (!FactWeight.activeSelf) FactWeight.SetActive(true);
            if (!Netto.activeSelf) Netto.SetActive(true);
            if (!Work.activeSelf) Work.SetActive(true);
            if (!Sovm1.activeSelf) Sovm1.SetActive(true);
            if (!Sovm2.activeSelf) Sovm2.SetActive(true);

            #region режим работы ОГП
            switch (CraneControl.Instance.CurrentGrabControl.CurrentLiftingDevice)
            {
                case GrabControl.LiftingDevice.Greifer:
                    TextWorkmode.text = "GR";
                    break;
                case GrabControl.LiftingDevice.Traverse:
                case GrabControl.LiftingDevice.AutoTraverse:
                case GrabControl.LiftingDevice.Hook:
                    TextWorkmode.text = "HK";
                    break;
                case GrabControl.LiftingDevice.Spreader:
                    TextWorkmode.text = "SP";
                    break;
            }
            #endregion

            if (framesToRefresh == 20)
            {
                tension = CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<ObiRigidbody>().addVel.y * 
                          ((int)CraneControl.Instance.CurrentRopeSpeed + 1);
                
                #region расчёт слабины лебёдок
                
                if (CraneControl.craneState.CraneSlab1Green)
                {
                    if (CraneControl.Instance.CurrentGrabControl.currentCargo.SafeIsUnityNull())
                    {
                        correctedWeight = tension + CraneControl.Instance.CurrentGrabControl.PlateAttach
                            .GetComponent<Rigidbody>().mass / 10;
                    }
                    else
                    {
                        correctedWeight = tension + CraneControl.Instance.CurrentGrabControl.PlateAttach
                            .GetComponent<Rigidbody>().mass / 10 + CraneControl.Instance.CurrentGrabControl.currentCargo
                            .GetComponent<Cargo>().CargoItself.GetComponent<Rigidbody>().mass / 4;
                    }
                }
                else correctedWeight = 0;
                Slab1.SetActive(CraneControl.craneState.CraneSlab1Red);
                Slab2.SetActive(CraneControl.craneState.CraneSlab2Red);
                #endregion
                
                #region текст на ОГП

                TextWeight.text = (Math.Round(correctedWeight, 2)).ToString();
                //Debug.Log(60.0f - Mathf.Round(CraneControl.Instance.Arrow.GetFloat(PullOutState) * 60.0f));
                pullout = 60.0f - Mathf.Round(CraneControl.Instance.Arrow.GetFloat(PullOutState) * 60.0f);
                TextPullout.text = pullout.ToString();

                #endregion

                #region расчёт перегруза с учётом вылета стрелы

                var poc = pullout / 60.0f;
                overweight = (float)correctedWeight / (1.1f - poc);
                OverweightIndicators(overweight);
                framesToRefresh = 0;

                #endregion
            }
            else framesToRefresh++;

            #region индикатор вращения
            if (CraneControl.Instance.inputActionAsset.FindAction("RotationMovement").ReadValue<float>() == 0.0f ||
                Hardware.Instance.IsConnected() && Mathf.Abs(Joystick.all[CraneControl.JoystickB].stick.x.ReadValue()) < 0.05f)
            {
                RotR.SetActive(false);
                RotL.SetActive(false);
            }

            if (CraneControl.Instance.inputActionAsset.FindAction("RotationMovement").ReadValue<float>() > 0 ||
                Hardware.Instance.IsConnected() && Joystick.all[CraneControl.JoystickB].stick.x.ReadValue() > 0)
                RotR.SetActive(true);
            else RotR.SetActive(false);
            if (CraneControl.Instance.inputActionAsset.FindAction("RotationMovement").ReadValue<float>() < 0 ||
                Hardware.Instance.IsConnected() && Joystick.all[CraneControl.JoystickB].stick.x.ReadValue() < 0)
                RotL.SetActive(true);
            else RotL.SetActive(false);
            #endregion

            #region индикатор стрелы
            if (CraneControl.Instance.inputActionAsset.FindAction("ArrowMovement").ReadValue<float>() == 0.0f ||
                Hardware.Instance.IsConnected() && Mathf.Abs(Joystick.all[CraneControl.JoystickA].stick.y.ReadValue()) < 0.05f)
            {
                ArrBack.SetActive(false);
                ArrForward.SetActive(false);
            }
            if (CraneControl.Instance.inputActionAsset.FindAction("ArrowMovement").ReadValue<float>() < 0 ||
                Hardware.Instance.IsConnected() && Joystick.all[CraneControl.JoystickA].stick.y.ReadValue() < 0) ArrBack.SetActive(true);
            else ArrBack.SetActive(false);
            if (CraneControl.Instance.inputActionAsset.FindAction("ArrowMovement").ReadValue<float>() > 0 ||
                Hardware.Instance.IsConnected() && Joystick.all[CraneControl.JoystickA].stick.y.ReadValue() > 0) ArrForward.SetActive(true);
            else ArrForward.SetActive(false);
            #endregion

            #region индикатор лебедки
            if (CraneControl.Instance.inputActionAsset.FindAction("RopeLengthChanging").ReadValue<float>() == 0.0f ||
                Hardware.Instance.IsConnected() && Mathf.Abs(Joystick.all[CraneControl.JoystickB].stick.y.ReadValue()) < 0.05f)
            {
                Leb1Up.SetActive(false);
                Leb1Down.SetActive(false);
                Leb2Up.SetActive(false);
                Leb2Down.SetActive(false);
            }

            if (CraneControl.Instance.inputActionAsset.FindAction("RopeLengthChanging").ReadValue<float>() > 0 ||
                Hardware.Instance.IsConnected() && Joystick.all[CraneControl.JoystickB].stick.y.ReadValue() > 0)
            {
                Leb1Down.SetActive(true);
                Leb2Down.SetActive(true);
            }
            else
            {
                Leb1Down.SetActive(false);
                Leb2Down.SetActive(false);
            }

            if (CraneControl.Instance.inputActionAsset.FindAction("RopeLengthChanging").ReadValue<float>() < 0 || 
                Hardware.Instance.IsConnected() && Joystick.all[CraneControl.JoystickB].stick.y.ReadValue() < 0)
            {
                Leb1Up.SetActive(true);
                Leb2Up.SetActive(true);
            }
            else
            {
                Leb1Up.SetActive(false);
                Leb2Up.SetActive(false);
            }
            #endregion

            #region лампы перегруза
            void OverweightIndicators(float overweight)
            {
                if (overweight < 20)
                {
                    w20.SetActive(true);
                    foreach (var ow in this.OverweightIndicators.Except(new List<GameObject> { w20 })) ow.SetActive(false);
                    Work.SetActive(true);
                    Danger.SetActive(false);
                    Stop.SetActive(false);
                }

                if (overweight > 20 && overweight < 40)
                {
                    w40.SetActive(true);
                    foreach (var ow in this.OverweightIndicators.Except(new List<GameObject> { w40 })) ow.SetActive(false);
                    Work.SetActive(true);
                    Danger.SetActive(false);
                    Stop.SetActive(false);
                }

                if (overweight > 40 && overweight < 60)
                {
                    w60.SetActive(true);
                    foreach (var ow in this.OverweightIndicators.Except(new List<GameObject> { w60 })) ow.SetActive(false);
                    Work.SetActive(true);
                    Danger.SetActive(false);
                    Stop.SetActive(false);
                }

                if (overweight > 60 && overweight < 80)
                {
                    w80.SetActive(true);
                    foreach (var ow in this.OverweightIndicators.Except(new List<GameObject> { w80 })) ow.SetActive(false);
                    Work.SetActive(true);
                    Danger.SetActive(true);
                    Stop.SetActive(false);
                }

                if (overweight > 80 && overweight < 100)
                {
                    w100.SetActive(true);
                    foreach (var ow in this.OverweightIndicators.Except(new List<GameObject> { w100 })) ow.SetActive(false);
                    Work.SetActive(false);
                    Danger.SetActive(true);
                    Stop.SetActive(false);
                }

                if (overweight > 100)
                {
                    w120.SetActive(true);
                    foreach (var ow in this.OverweightIndicators.Except(new List<GameObject> { w120 })) ow.SetActive(false);
                    Work.SetActive(false);
                    Danger.SetActive(false);
                    Stop.SetActive(true);
                }
            }
            #endregion
        }

        #region кран выключен - гасим прибор
        else
        {
            foreach (var ind in AllIndicators)
            {
                ind.SetActive(false);
            }

            TextWeight.text = "";
            TextPullout.text = "";
            TextWorkmode.text = "";
        }
        #endregion
    }
}
