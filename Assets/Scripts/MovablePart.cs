using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovablePart : MonoBehaviour
{
    private enum MoveType
    {
        Translate,
        Rotate,
        ConfJointRotate,
        Anim,
        Rope
    }
    private enum AxisToMove
    {
        x, y, z
    }
    private enum JoystickIndex
    {
        JoystickA, JoystickB, JoystickC
    }
    private InputActionAsset inputActionAssetf;
    [SerializeField] private string actionName;
    private Transform _part;
    [SerializeField] private MoveType _moveType;
    [SerializeField] private bool isAxisInverted;
    [SerializeField] private bool isLocalSpace;
    [SerializeField] private AxisToMove currentAxis;
    [SerializeField] public float minLimit, maxLimit;
    [SerializeField] private float speed;
    [SerializeField] private List<RopeControl> ropes = new List<RopeControl>();
    [SerializeField] private float velocityCoefficient;
    [SerializeField] public float velocityValue;

    [SerializeField] private bool ReplacedByJoystick;

    [SerializeField] private JoystickIndex jIndex;
    [SerializeField] private bool jAxisX;

    [SerializeField] private float speedUp;

    private bool JoystickSettingsExists;

    private float confJointAngle;

    SoftJointLimit highLimit;
    SoftJointLimit lowLimit;

    public float inputValue;
    private Vector3 direction;
    private static readonly int PullOutState = Animator.StringToHash("PullOutState");

    void Start()
    {
        _part = this.transform;
        direction = new Vector3(currentAxis == AxisToMove.x ? 1 : 0,
            currentAxis == AxisToMove.y ? 1 : 0,
            currentAxis == AxisToMove.z ? 1 : 0);
        if (isAxisInverted) direction = -direction;

        if (_moveType == MoveType.ConfJointRotate)
        {
            highLimit = new SoftJointLimit();
            lowLimit = new SoftJointLimit();
        }
        string AppPath = Path.GetDirectoryName(Application.dataPath);
        JoystickSettingsExists = File.Exists(AppPath + "\\Config\\sticks.ini");
        
        //TODO считывать из конфига скорости
        if (File.Exists(AppPath + "\\Config\\speed.ini"))
        {
            //Debug.Log("TryingToReadFile");
            foreach (var str in File.ReadAllLines(AppPath + "\\Config\\speed.ini"))
            {
                if (str.Contains(actionName))
                {
                    speedUp = (float)Convert.ToDouble(str.Split(' ')[1]);
                    //Debug.Log(actionName + " Speeded Up for " + (float)Convert.ToDouble(str.Split(' ')[1]));
                }
            }
        }
        else
        {
            Debug.Log("speed.ini doesn't exists");
        }
    }

    void FixedUpdate()
    {
        //присваиваем инпут по имени экшена
        if (!CraneControl.Instance.inputActionAsset.SafeIsUnityNull())
        {
            if (!ReplacedByJoystick || !JoystickSettingsExists || !Hardware.Instance.IsConnected())
                inputValue = CraneControl.Instance.inputActionAsset.FindAction(actionName).ReadValue<float>();
            else
            {
                switch (jIndex)
                {
                    case JoystickIndex.JoystickA:
                        inputValue = jAxisX
                            ? Joystick.all[CraneControl.JoystickA].stick.x.ReadValue()
                            : Joystick.all[CraneControl.JoystickA].stick.y.ReadValue();
                        break;
                    case JoystickIndex.JoystickB:
                        inputValue = jAxisX
                            ? Mathf.Clamp(Joystick.all[CraneControl.JoystickB].stick.x.ReadValue() + CraneControl.Instance.inputActionAsset.FindAction(actionName).ReadValue<float>(), -1, 1)
                            : Joystick.all[CraneControl.JoystickB].stick.y.ReadValue();
                        break;
                    case JoystickIndex.JoystickC:
                        inputValue = jAxisX
                            ? Joystick.all[CraneControl.JoystickC].stick.x.ReadValue()
                            : Joystick.all[CraneControl.JoystickC].stick.y.ReadValue();
                        break;
                }
            }

            /*if (actionName == "RotationMovement" && Hardware.Instance.IsConnected())
            {
                inputValue += Joystick.all[CraneControl.JoystickB].stick.x.ReadValue();
                inputValue = Mathf.Clamp(inputValue, -1f, 1f);
            }*/
            
            
        }
        //Debug.Log(inputValue);

        if (velocityCoefficient > 0)
        {
            if (Mathf.Abs(velocityValue) <= Mathf.Abs(inputValue) &&
                CraneControl.craneState.CraneReadyEnabled) //ускорение
            {
                velocityValue += inputValue * velocityCoefficient;
            }
            else
            {
                if (Mathf.Abs(velocityValue) > 0.2f) //замедление
                {
                    velocityValue *= 1 - velocityCoefficient * 2;
                }
                else velocityValue = 0;
            }

            inputValue = Mathf.Clamp(velocityValue, -1.0f, 1.0f);
        }
        else inputValue *= CraneControl.craneState.CraneReadyEnabled ? 1 : 0;
        
        if (speedUp != 0) inputValue *= speedUp;

        switch (_moveType)
        {
            case MoveType.Translate: //Перемещение по рельсам
                _part.Translate(direction * (inputValue * Time.deltaTime * speed), isLocalSpace ? Space.Self : Space.World);

                if (!isLocalSpace)
                {
                    Vector3 trp = this.transform.position;
                    //limit
                    transform.position = currentAxis switch
                    {
                        AxisToMove.x => new Vector3(Mathf.Clamp(trp.x, minLimit, maxLimit), trp.y, trp.z),
                        AxisToMove.y => new Vector3(trp.x, Mathf.Clamp(trp.y, minLimit, maxLimit), trp.z),
                        AxisToMove.z => new Vector3(trp.x, trp.y, Mathf.Clamp(trp.z, minLimit, maxLimit)),
                        _ => transform.position
                    };
                }
                else
                {
                    Vector3 trp = this.transform.localPosition;
                    //limit
                    transform.localPosition = currentAxis switch
                    {
                        AxisToMove.x => new Vector3(Mathf.Clamp(trp.x, minLimit, maxLimit), trp.y, trp.z),
                        AxisToMove.y => new Vector3(trp.x, Mathf.Clamp(trp.y, minLimit, maxLimit), trp.z),
                        AxisToMove.z => new Vector3(trp.x, trp.y, Mathf.Clamp(trp.z, minLimit, maxLimit)),
                        _ => transform.localPosition
                    };
                }
                break;
            case MoveType.Rotate: //вращение "башни" крана
                _part.localRotation = Quaternion.Euler(
                    currentAxis == AxisToMove.x ? _part.localRotation.eulerAngles.x + inputValue * Time.deltaTime * speed : _part.localRotation.x,
                    currentAxis == AxisToMove.y ? _part.localRotation.eulerAngles.y + inputValue * Time.deltaTime * speed : _part.localRotation.y,
                    currentAxis == AxisToMove.z ? _part.localRotation.eulerAngles.z + inputValue * Time.deltaTime * speed : _part.localRotation.z);
                break;
            case MoveType.Anim: //вылет стрелы крана
                _part.GetComponent<Animator>().SetFloat(PullOutState,
                    Mathf.Clamp01(_part.GetComponent<Animator>().GetFloat(PullOutState) +
                                  (isAxisInverted ? -1 : 1) * inputValue * speed * Time.deltaTime));
                break;
            case MoveType.Rope: //контроль длины лебёдок
                if (inputValue > 0) inputValue *= CraneControl.craneState.CraneSlab1Green ? 1 : 0;
                foreach (var ropeControl in ropes)
                {
                    switch (CraneControl.Instance.CurrentRopeSpeed)
                    {
                        case CraneControl.RopeSpeed.FirstSpeed:
                            speed = 1;
                            break;
                        case CraneControl.RopeSpeed.SecondSpeed:
                            speed = 1.5f;
                            break;
                        case CraneControl.RopeSpeed.ThirdSpeed:
                            speed = 2;
                            break;
                    }
                    ropeControl.Cursor.ChangeLength(Mathf.Clamp(
                        ropeControl.Rope.restLength + (inputValue * speed * Time.deltaTime), minLimit, maxLimit));
                }

                break;
            case MoveType.ConfJointRotate: //Вращение ConfigurableJoint'а
                float eps = 0.5f;
                confJointAngle += inputValue * speed;
                confJointAngle = Mathf.Clamp(confJointAngle, -175.0f, 175.0f);
                //if (Mathf.Abs(confJointAngle) >= 177) 
                if (Mathf.Abs(inputValue) > 0.01f)
                {
                    highLimit.limit = confJointAngle + eps;
                    lowLimit.limit = confJointAngle - eps; 
                }
                //учитывать полный круг вряд ли получится, конфджоинт дёргается при переходе через 0
                _part.GetComponent<ConfigurableJoint>().highAngularXLimit = highLimit;
                _part.GetComponent<ConfigurableJoint>().lowAngularXLimit = lowLimit;

                //Debug.Log("addVel " + GetComponent<ObiRigidbody>().addVel);
                
                break;
        }
    }
}
