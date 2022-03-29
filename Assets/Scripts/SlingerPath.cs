using System;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class SlingerPath : MonoBehaviour
{
    [SerializeField] private TMP_Text slinger;
    [SerializeField] private GameObject SlingerPanel;

    public string FindPath(Transform target)
    {
        Vector3 grabPos = CraneControl.Instance.CurrentGrabControl.GrabItself.transform.position;
        Vector3 ropesAttachPos = CraneControl.Instance.GetComponentInChildren<TorusCloneMovement>().transform.position;
        Vector3 tarPos = target.position;

        #region Сообщение нажат аварийник

        if (CraneControl.Instance.isEmegencyStop)
        {
            return("Нажат аварийный стоп");
        }

        #endregion
        
        #region Проверка строповки
        if (QuestStorage.Instance.LiftingDevice != LiftingDevice.Greifer &&
            QuestStorage.Instance.LiftingDevice != LiftingDevice.Spreader &&
            QuestControl.Instance.cargoTarget.IsGrabbed && !QuestControl.Instance.cargoTarget.AlreadyLifted &&
            !QuestControl.Instance.cargoTarget.IsStropeChecked) 
        {
            switch (QuestControl.Instance.cargoTarget.cargoBinding)
            {
                case Cargo.CheckBinding.OnGround:
                    if (QuestStorage.Instance.questMode == QuestMode.Learn)
                    {
                        return "Приподнимите груз для проверки строповки";
                    }
                    break;
                case Cargo.CheckBinding.InProgress:
                    return "Производится проверка строповки";
                    break;
                case Cargo.CheckBinding.Complete:
                    return "Проверка строповки завершена";
                    break;
                case Cargo.CheckBinding.Mistake:
                    return "Груз поднят без проверки строповки";
                    break;
            }
            
        }
        #endregion

        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.LiftingDevice != LiftingDevice.Greifer &&
            QuestStorage.Instance.LiftingDevice != LiftingDevice.Spreader)
        {
            if(QuestControl.Instance.cargoTarget.IsGrabbing) return ("Происходит закрепление груза");
            if(QuestControl.Instance.cargoTarget.IsReleasing) return ("Происходит отпускание груза");
        }
        
        #region "Статичная" высота
        float finDistance = Vector3.Distance(new Vector3(tarPos.x, 0, tarPos.z), new Vector3(grabPos.x, 0, grabPos.z));
        
        if (QuestStorage.Instance.LiftingDevice != LiftingDevice.Greifer && QuestControl.Instance.cargoTarget.IsGrabbed && finDistance > 10 &&
            (QuestStorage.Instance.QuestCargoPlace == QuestCargoPlace.SecondVessel && grabPos.y < 30 ||
             QuestStorage.Instance.QuestCargoPlace != QuestCargoPlace.SecondVessel && grabPos.y < 15))
        {
            return("Вира");
        }
        #endregion
        
        #region Положение крана на рельсах

        // место на крановых рельсах
        if (Mathf.Abs(target.position.x - CraneControl.Instance.Base.transform.position.x) < 20) //было Instance.Arrow.transform
        {
            if (target.position.z - CraneControl.Instance.Base.transform.position.z > 0)
            {
                if (Mathf.Abs(target.position.z - 30 - CraneControl.Instance.Base.transform.position.z) >= 0.5f)
                {
                    if (target.position.z - 30 - CraneControl.Instance.Base.transform.position.z > 0.5f) return ("рельсы вправо");
                    else return("рельсы влево");
                }
            }
            else
            {
                if (Mathf.Abs(target.position.z + 30 - CraneControl.Instance.Base.transform.position.z) >= 0.5f)
                {
                    if (target.position.z + 30 - CraneControl.Instance.Base.transform.position.z > 0.5f) return ("рельсы вправо");
                    else return("рельсы влево");
                }
            }
            
        }
        else // место слева или справа от крана
        {
            if (Mathf.Abs(target.position.z - CraneControl.Instance.Base.transform.position.z) >= 0.5f)
            {
                if (target.position.z > CraneControl.Instance.Base.transform.position.z) return("рельсы вправо"); //targetPos_z > grabPos
                else return("рельсы влево");
            }
        }

        #endregion

        #region Поворот крана

        tarPos = target.position;
        var crPos = CraneControl.Instance.Arrow.transform.position;
        Vector3 fCraneToTarget = new Vector3(tarPos.x - crPos.x, 0, tarPos.z - crPos.z);
        Vector3 fCraneToGrab = new Vector3(grabPos.x - crPos.x, 0, grabPos.z - crPos.z);
        if (Mathf.Abs(Vector3.SignedAngle(fCraneToTarget, fCraneToGrab, Vector3.up)) > 2) // если разница углов от крана до места и угла стрелы > 5
        {
            if (Vector3.SignedAngle(fCraneToTarget, fCraneToGrab, Vector3.up) < 0)
            {
                return("Поверните кран по часовой");
            } else return("Поверните кран против часовой");
        }

        #endregion
        
        #region Вылет стрелы

        var distTar = Vector3.Distance(new Vector3(crPos.x, 0, crPos.z), new Vector3(tarPos.x, 0, tarPos.z));
        var distGrab = Vector3.Distance(new Vector3(crPos.x, 0, crPos.z), new Vector3(ropesAttachPos.x, 0, ropesAttachPos.z));

        if (Mathf.Abs(distGrab - distTar) > 0.5f)
        {
            if(distGrab - distTar < 0) return("Выдвиньте стрелу");
            else return("Сократите стрелу");
        }

        #endregion
        
        #region Вылет спредера

        if (QuestStorage.Instance.LiftingDevice == LiftingDevice.Spreader)
        {
            if (QuestStorage.Instance.CargoType == CargoType.Container20Ft &&
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>()
                    .GrabberDistance <
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>().f20ft ||
                QuestStorage.Instance.CargoType == CargoType.Container40Ft &&
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>()
                    .GrabberDistance <
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>().f40ft)
            {
                return ("Выдвиньте спредер");
            }

            if (QuestStorage.Instance.CargoType == CargoType.Container20Ft &&
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>()
                    .GrabberDistance >
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>().f20ft ||
                QuestStorage.Instance.CargoType == CargoType.Container40Ft &&
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>()
                    .GrabberDistance >
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>().f40ft)
            {
                return ("Сократите вылет спредера");
            }
            
            if ((QuestStorage.Instance.CargoType == CargoType.Container20Ft &&
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>().Is20ft ||
                QuestStorage.Instance.CargoType == CargoType.Container40Ft &&
                CraneControl.Instance.CurrentGrabControl.PlateAttach.GetComponent<SpreaderParameters>().Is40ft) &&
                CraneControl.craneState.CraneSlab1Red && !QuestControl.Instance.cargoTarget.IsGrabbed)
            {
                return ("Нажмите кнопку захвата груза");
            }

            if (QuestControl.Instance.cargoTarget.IsGrabbed && QuestControl.Instance.placeTrigger.ContainsCargo && CraneControl.craneState.CraneSlab1Red)
            {
                return ("Нажмите кнопку освобождения груза");
            }
        }

        #endregion
        
        #region Высота грузозахвата над грузом

        if (grabPos.y - target.position.y > 2)
        {
            if(grabPos.y - target.position.y > 0) return("Майна");
        }
        return String.Empty;

        #endregion
    }

    private void Update()
    {
        //проверка с грейфером
        /*Debug.Log(!QuestStorage.Instance.SafeIsUnityNull());
        Debug.Log(QuestStorage.Instance.LiftingDevice == LiftingDevice.Greifer);
        Debug.Log(!CraneControl.Instance.CurrentGrabControl.GreiferClosed);
        Debug.Log(FindPath(QuestControl.Instance.bulkCargo.transform).Length > 0);*/

        #region Если груз не захвачен - указываем путь до груза

        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.LiftingDevice != LiftingDevice.Greifer && !QuestControl.Instance.cargoTarget.IsGrabbed && 
            FindPath(QuestControl.Instance.cargoTarget.CargoItself).Length > 0)
        {
            SlingerPanel.SetActive(true);
            slinger.text = FindPath(QuestControl.Instance.cargoTarget.CargoItself);
        }

        #endregion

        #region Грейфер открыт - указываем путь до эмиттера

        else if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.LiftingDevice == LiftingDevice.Greifer &&
                 !CraneControl.Instance.CurrentGrabControl.GreiferClosed &&
                 FindPath(QuestControl.Instance.bulkCargo.transform).Length > 0)
        {
            SlingerPanel.SetActive(true);
            slinger.text = FindPath(QuestControl.Instance.bulkCargo.transform);
        } 

        #endregion

        #region В противном случае указываем путь до места разгрузки

        else if (!QuestStorage.Instance.SafeIsUnityNull() && FindPath(QuestControl.Instance.placeTrigger.transform).Length > 0 &&
                 (QuestStorage.Instance.LiftingDevice != LiftingDevice.Greifer && QuestControl.Instance.cargoTarget.IsGrabbed ||
                  QuestStorage.Instance.LiftingDevice == LiftingDevice.Greifer && CraneControl.Instance.CurrentGrabControl.GreiferClosed))
        {
            //Debug.Log("cargo_y " + QuestControl.Instance.cargoTarget.CargoItself.position.y);
            SlingerPanel.SetActive(true);
            slinger.text = FindPath(QuestControl.Instance.placeTrigger.transform); 
        }

        #endregion
        
        else // выключаем команды при отсутствии таковых
        {
            SlingerPanel.SetActive(false);
            slinger.text = string.Empty;
        }
    }
}
