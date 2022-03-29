using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using Obi;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GrabControl : MonoBehaviour
{
    public Transform currentCargo = null;

    public GameObject GrabItself;
    private Rigidbody grabItselfRig;

    public Transform PlateAttach;

    public LiftingDevice CurrentLiftingDevice;

    private Coroutine GreiferCoroutine = null;
    private Coroutine SlabCoroutine = null;
    private Coroutine WindCoroutine = null;

    [HideInInspector] public float WindParameter;

    [SerializeField] private List<ObiRope> listRopes = new List<ObiRope>();

    public bool GreiferClosed = false;
    private static readonly int CloseGreifer = Animator.StringToHash("CloseGreifer");
    private static readonly int OpenGreifer = Animator.StringToHash("OpenGreifer");
    private Animator GreiferAnimator;

    private Vector3 randWind, ropesWind;

    public enum LiftingDevice
    {
        Greifer,            // Грейфер
        Traverse,           // Траверса
        AutoTraverse,		// Автотраверса
        Spreader,           // Спредер
        Hook,               // Крюк
    }

    private void Start()
    {
        //устанавливаем аниматор для грейфера
        if (CurrentLiftingDevice == LiftingDevice.Greifer) GreiferAnimator = GrabItself.GetComponent<Animator>();
        
        //Случайное направление ветра
        grabItselfRig = GrabItself.GetComponent<Rigidbody>();
        randWind = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
        ropesWind = new Vector3(randWind.z, 0, -randWind.x);
        //if (WindParameter > 0) WindCoroutine = StartCoroutine(Wind());
    }

    public void GrabCargo(Cargo cargo)
    {
        if (currentCargo.SafeIsUnityNull() && cargo.IsAvaliable && GreiferCoroutine == null && !cargo.PlaceToAttach.SafeIsUnityNull())
        {
            switch (CurrentLiftingDevice)
            {
                case LiftingDevice.Hook:
                case LiftingDevice.Traverse:
                case LiftingDevice.AutoTraverse:
                    if (Mathf.Abs(GrabItself.GetComponent<Rigidbody>().velocity.magnitude) < 0.3f ||
                        Mathf.Abs(GrabItself.GetComponent<Rigidbody>().angularVelocity.magnitude) < 0.3f)
                    {
                        GreiferCoroutine = StartCoroutine(GrabHookSoft(cargo));
                        cargo.PlaceToAttach.didAlready = true;
                        //выключаем подсветку у груза
                        cargo.GetComponent<Outlinable>().enabled = false;
                    }
                    break;
                case LiftingDevice.Spreader:
                    if (Mathf.Abs(GrabItself.GetComponent<Rigidbody>().velocity.magnitude) < 0.3f ||
                        Mathf.Abs(GrabItself.GetComponent<Rigidbody>().angularVelocity.magnitude) < 0.3f)
                    {
                        GreiferCoroutine = StartCoroutine(GrabSpreader(cargo));
                        cargo.PlaceToAttach.didAlready = true;
                        //выключаем подсветку у груза
                        cargo.GetComponent<Outlinable>().enabled = false;
                    }
                    break;
            }
        }
    }

    private void Update()
    {
        #region Вычисляем слабину лебёдок

        if (CraneControl.craneState.CraneRopeEnabled)
        {
            if (PlateAttach.GetComponent<ObiRigidbody>().addVel.y < 0)
                SlabCoroutine ??= StartCoroutine(CheckSlab());
            else setSlab(true);
        }

        #endregion

        #region Открытие / закрытие грейфера корутинами

        if (CurrentLiftingDevice == LiftingDevice.Greifer && GreiferCoroutine == null)
        {
            if (CraneControl.Instance.inputActionAsset.FindAction("SpreaderGrab").ReadValue<float>() > 0 && !GreiferClosed)
                GreiferCoroutine = StartCoroutine(CloseGreiferAnim());
            else if (CraneControl.Instance.inputActionAsset.FindAction("SpreaderRelease").ReadValue<float>() > 0 && GreiferClosed)
                GreiferCoroutine = StartCoroutine(OpenGreiferAnim());
        }

        #endregion

        if (!QuestStorage.Instance.SafeIsUnityNull() && !QuestControl.Instance.SafeIsUnityNull() &&
            CraneControl.Instance.CurrentGrabControl.CurrentLiftingDevice != LiftingDevice.Greifer &&
            QuestControl.Instance.cargoTarget.IsGrabbed && QuestControl.Instance.placeTrigger.ContainsCargo)
        {
            StopCoroutine(Wind());
            WindCoroutine = null;
        } 
        else if (WindCoroutine == null) WindCoroutine = StartCoroutine(Wind());
    }

    private IEnumerator Wind()
    {
        while (true)
        {
            float randVal = Random.value;
            for (float i = 0; i < (3 - WindParameter + randVal) ; i+= Time.deltaTime)
            {
                grabItselfRig.AddForce(randWind * (WindParameter * 3), ForceMode.Acceleration);
                foreach (var rope in listRopes)
                    rope.AddForce(ropesWind * (WindParameter * 3) / listRopes.Count, ForceMode.Acceleration);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            for (float j = 0; j < 7 + WindParameter + randVal; j+= Time.deltaTime)
                yield return new WaitForSeconds(Time.deltaTime);
            yield return null;
        }
    }

    void setSlab(bool value)
    {
        if (CraneControl.Instance.fqtestLEDsCoroutine == null)
        {
            CraneControl.craneState.CraneSlab1Green = value;
            CraneControl.craneState.CraneSlab2Green = value;
            CraneControl.craneState.CraneSlab1Red = !value;
            CraneControl.craneState.CraneSlab2Red = !value;
        }
    }

    IEnumerator CheckSlab()
    {
        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            if (PlateAttach.GetComponent<ObiRigidbody>().addVel.y < 0)
                yield return new WaitForSeconds(Time.deltaTime);
            else
            {
                Debug.Log("breakSlab");
                SlabCoroutine = null;
                yield break;
            }
        }
        setSlab(false);

        SlabCoroutine = null;
    }

    //Закрытие грейфера
    IEnumerator CloseGreiferAnim()
    {
        GreiferAnimator.SetBool(CloseGreifer, true);
        yield return new WaitForSeconds(4);
        GreiferClosed = true;
        GreiferCoroutine = null;
    }
    
    //Открытие грейфера
    IEnumerator OpenGreiferAnim()
    {
        GreiferAnimator.SetBool(OpenGreifer, true);
        yield return new WaitForSeconds(4);
        GreiferClosed = false;
        GreiferCoroutine = null;
    }
    
    //Захват груза спредером
    IEnumerator GrabSpreader(Cargo cargo)
    {
        //Debug.Log("GrabbedBySpreader");
        
        //делаем кинематиком грузозахват при закреплении
        GrabItself.GetComponent<Rigidbody>().isKinematic = true;
        
        Vector3 grabPos = GrabItself.transform.position + new Vector3(0, -2.65f, 0);
        Quaternion grabItselfRot;
        grabItselfRot = GrabItself.transform.rotation;
        if (Mathf.Abs(cargo.PlaceToAttach.transform.rotation.eulerAngles.y -
                      GrabItself.transform.rotation.eulerAngles.y) > 170 &&
            Mathf.Abs(cargo.PlaceToAttach.transform.rotation.eulerAngles.y -
                      GrabItself.transform.rotation.eulerAngles.y) < 190)
            grabItselfRot *= Quaternion.Euler(0, 180, 0);

        Vector3 cargoPos = cargo.CargoItself.transform.position;
        Quaternion cargoRot = cargo.CargoItself.transform.rotation;

        //поворачиваем и перемещаем груз к грузозахвату
        for (float i = 0; i <= 2; i += Time.deltaTime)
        {
            cargo.CargoItself.transform.position = Vector3.Slerp(cargoPos, grabPos, i);
            cargo.CargoItself.transform.rotation = Quaternion.Slerp(cargoRot, grabItselfRot, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Закрепляем груз FixedJoint'ом
        FixedJoint fxj = GrabItself.AddComponent<FixedJoint>();
        fxj.connectedBody = cargo.CargoItself.GetComponent<Rigidbody>();
        
        //отключаем кинематик у груза
        cargo.MakeKinematicCargo(false);
        
        //убираем кинематик у грузозахвата
        GrabItself.GetComponent<Rigidbody>().isKinematic = false;
        
        //Убираем parent у груза
        cargo.transform.parent = null;

        //устанавливаем параметры после закрепления
        cargo.IsGrabbed = true;
        cargo.IsAvaliable = false;
        currentCargo = cargo.transform;
        GreiferCoroutine = null;
        //currentCargo.GetComponent<Cargo>().CheckStropes();
    }

    IEnumerator GrabHookSoft(Cargo cargo)
    {
        //сначала держим грузозахват над грузом
        for (float i = 0; i < 5; i+= Time.deltaTime)
        {
            if (cargo.PlaceToAttach.GetComponent<PlaceToAttach>().grabControl.SafeIsUnityNull() && GreiferCoroutine != null)
            {
                Debug.Log("Stopped grabbing");
                cargo.PlaceToAttach.GetComponent<PlaceToAttach>().didAlready = false;
                cargo.GetComponent<Outlinable>().enabled = true;
                if(cargo.IsGrabbing) cargo.IsGrabbing = false;
                StopCoroutine(GreiferCoroutine);
                GreiferCoroutine = null;
                yield break;
            }
            else
            {
                if(!cargo.IsGrabbing) cargo.IsGrabbing = true;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        
        if(cargo.IsGrabbing) cargo.IsGrabbing = false;
        
        //Debug.Log("GrabedByHook");
        //включаем солвер и лебёдки без рендера
        cargo.SetSolverActive(true);
        cargo.SetStropes(true);
        
        GameObject PlaceToAttach = cargo.PlaceToAttach.gameObject;
        
        //отключаем кинематик у груза
        cargo.MakeKinematicCargo(false);

        Quaternion grabItselfRot;
        Vector3 grabPos;

        //делаем кинематиком грузозахват при закреплении
        //GrabItself.GetComponent<Rigidbody>().isKinematic = true;

        var ptaPos = cargo.PlaceToAttach.transform;
        var ptaRotation = ptaPos.rotation;
        //Debug.Log("angDiff " + grabItselfRot.eulerAngles + " cargo " + ptaRotation.eulerAngles);

        Vector3 cargoPos = ptaPos.position;
        
        //поворачиваем и перемещаем груз к грузозахвату
        for (float i = 0; i <= 2; i += Time.deltaTime)
        {
            grabPos = GrabItself.transform.position;
            grabItselfRot = GrabItself.transform.rotation;
            if (Mathf.Abs(cargo.PlaceToAttach.transform.rotation.eulerAngles.y -
                          GrabItself.transform.rotation.eulerAngles.y) > 170 &&
                Mathf.Abs(cargo.PlaceToAttach.transform.rotation.eulerAngles.y -
                          GrabItself.transform.rotation.eulerAngles.y) < 190)
            {
                //Debug.Log("a " + cargo.PlaceToAttach.transform.rotation.eulerAngles.y + " b " + GrabItself.transform.rotation.eulerAngles.y);
                grabItselfRot *= Quaternion.Euler(0, 180, 0);
            }

            cargo.PlaceToAttach.transform.position = Vector3.Slerp(cargoPos, grabPos, i);
            cargo.PlaceToAttach.transform.rotation = Quaternion.Slerp(ptaRotation, grabItselfRot, i);
            yield return new WaitForEndOfFrame();//  WaitForSeconds(Time.deltaTime);
        }
        
        //включаем отображение строп у груза
        cargo.ShowStropes(true);

        //убираем кинематик у грузозахвата
        //GrabItself.GetComponent<Rigidbody>().isKinematic = false;

        //крепим PlaceToAttach к грузозахвату
        FixedJoint fxj = GrabItself.AddComponent<FixedJoint>();
        Rigidbody RigigdbodyToAttach = PlaceToAttach.GetComponent<Rigidbody>();
        fxj.connectedBody = RigigdbodyToAttach;
        
        //выключаем кинематик у грузозахвата
        cargo.UnKinematicAttach(false);
        
        //Убираем parent у груза
        cargo.transform.parent = null;

        //устанавливаем параметры после закрепления груза
        cargo.IsGrabbed = true;
        cargo.IsAvaliable = false;
        currentCargo = cargo.transform;
        GreiferCoroutine = null;
        currentCargo.GetComponent<Cargo>().CheckStropes();
    }

    public void ReleaseCargo()
    {
        //Debug.Log("ReleaseCargo GrabControl " + currentCargo);
        //включаем кинематик у груза
        if(CurrentLiftingDevice != LiftingDevice.Spreader) currentCargo.GetComponent<Cargo>().MakeKinematicCargo(true);
        //выключаем отображение строп у груза
        currentCargo.GetComponent<Cargo>().ShowStropes(false);

        //удаляем связь FixedJoint'ом груза с грузозахватом
        FixedJoint fxj = GrabItself.GetComponent<FixedJoint>();
        fxj.connectedBody = null;
        Destroy(fxj);

        //Делаем кинематиком PlaceToAttach
        currentCargo.GetComponent<Cargo>().UnKinematicAttach(true);
        currentCargo.GetComponent<Cargo>().IsGrabbed = false;
        //Ставим parent у груза, если место разгрузки не авто или ж/д
        //TODO проверить
        if (QuestStorage.Instance.QuestCargoPlace != QuestCargoPlace.Auto ||
            QuestStorage.Instance.QuestCargoPlace != QuestCargoPlace.RailwayPlatform)
            currentCargo.SetParent(currentCargo.GetComponentInChildren<CargoItself>().Hit.transform);
        currentCargo = null;
    }
}
