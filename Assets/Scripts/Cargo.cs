using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EPOOutline;
using Obi;
using Sirenix.Utilities;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public GrabControl GrabControl;

    public Transform CargoItself;

    public bool IsAvaliable = false;
    public bool IsGrabbed = false;
    public bool IsTraverse = false;
    public bool AlreadyLifted = false;
    public bool IsStropeChecked = false;
    public bool IsReleasing = false;
    public bool IsGrabbing = false;
    public PlaceToAttach PlaceToAttach;
    [SerializeField] private ObiSolver Solver;
    [SerializeField] private List<Transform> bracingElements = new List<Transform>();
    [SerializeField] private List<ObiRope> stropes = new List<ObiRope>();
    [SerializeField] private List<ObiRope> traverseStropes = new List<ObiRope>();

    [SerializeField] private List<Transform> armLockers = new List<Transform>();

    public Coroutine stropeCheckCoroutine = null;

    //"притягивание" к нужной позиции для контейнеров
    public Transform snapPlace;
    [SerializeField] private float reachDistance;
    [SerializeField] private float snapPower;
    
    public enum CheckBinding
    {
        OnGround,
        InProgress,
        Complete,
        Mistake
    }

    public CheckBinding cargoBinding = CheckBinding.OnGround;

    public Coroutine OnlyCoroutine { get; private set; } = null;
    private Coroutine WindCoroutine = null;
    
    IEnumerator CheckSling()
    {
        
        const float timeToWait = 2f; // сколько секунд будет длится проверка 
        float timerToWait = timeToWait;
        //Vector3 lastPosition = transform.position;
        //float velocityPations = 0.4f;

        yield return new WaitForSeconds(1);
        //Vector3 startGrabPoint = transform.position;
		
        while (cargoBinding != CheckBinding.Mistake && cargoBinding != CheckBinding.Complete)
        {
            // Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
            // lastPosition = transform.position;

            if (AlreadyLifted)
            {
                cargoBinding = CheckBinding.Mistake;
                Debug.Log("Груз поднят без проверки строповки");
                QuestStorage.Instance.AddMistake("Груз поднят без проверки строповки");
                break;
            } 

            if (CargoItself.GetComponent<CargoItself>().Hit.distance > 0.5f && !CraneControl.craneState.CraneMoveEnabled) //- на кессоне срабатывает даже не подняв
            {
                //груз приподняли
                cargoBinding = CheckBinding.InProgress;
                timerToWait -= Time.deltaTime;
                if (timerToWait < 0)
                {
                    cargoBinding = CheckBinding.Complete;
                    break; //проверка пройдена
                }
            }
            else
            {
                timerToWait = timeToWait;
                //груз закрепили, но не подняли
                cargoBinding = CheckBinding.OnGround;
            }
            yield return new WaitForEndOfFrame();
        }

        IsStropeChecked = true;
        stropeCheckCoroutine = null;
    }

    public void CheckStropes() => StartCoroutine(CheckSling());

    public void SetStropes(bool isActive)
    {
        foreach (var strope in IsTraverse ? traverseStropes : stropes)
        {
            strope.gameObject.SetActive(isActive);
        }
    }
    
    public void ShowStropes(bool isVisible)
    {
        Debug.Log("ShowStropes " + isVisible);

        foreach (var strope in IsTraverse ? traverseStropes : stropes)
        {
            strope.gameObject.GetComponent<ObiRopeExtrudedRenderer>().enabled = isVisible;
        }

        if(!bracingElements.IsNullOrEmpty()) bracingElements[IsTraverse ? 1 : 0].gameObject.SetActive(isVisible);
    }

    public void SetSolverActive(bool isActive)
    {
        if (!Solver.SafeIsUnityNull()) Solver.gameObject.SetActive(isActive);
    }

    public IEnumerator cargoMistake()
    {
        GetComponent<Outlinable>().FrontParameters.Color = Color.red;
        GetComponent<Outlinable>().enabled = true;
        yield return new WaitForSeconds(1);
        GetComponent<Outlinable>().enabled = false;
    }

    public void ReleaseInCargo()
    {
        if (GrabControl.CurrentLiftingDevice == GrabControl.LiftingDevice.Spreader) GrabControl.ReleaseCargo();
        else if (OnlyCoroutine == null) OnlyCoroutine = StartCoroutine(SoftReleaseCargo());
    }

    public void GrabInCargo() => GrabControl.GrabCargo(GetComponent<Cargo>());

    private IEnumerator SoftReleaseCargo()
    {
        for (float i = 0; i < 6f; i += Time.deltaTime)
        {
            var hitTransform = CargoItself.GetComponent<CargoItself>().Hit.transform;
            
            if(!hitTransform.SafeIsUnityNull()) Debug.Log(hitTransform.name + " " + !hitTransform.GetComponent<PlaceTrigger>().SafeIsUnityNull());
            
            if (CargoItself.GetComponent<ObiRigidbody>().addVel.y > 0 ||
                CargoItself.GetComponent<CargoItself>().Hit.distance + CargoItself.GetComponent<CargoItself>().startRay > 0.1f || 
                !hitTransform.SafeIsUnityNull() && !hitTransform.GetComponent<PlaceTrigger>().SafeIsUnityNull())
            {
                Debug.Log("SoftRelease Break" + " obi " + CargoItself.GetComponent<ObiRigidbody>().addVel.y + " rig " + 
                          CargoItself.GetComponent<Rigidbody>().velocity.y);
                if (IsReleasing) IsReleasing = false;
                OnlyCoroutine = null;
                yield break;
            }
            else
            {
                if (!IsReleasing) IsReleasing = true;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        GrabControl.ReleaseCargo();
        OnlyCoroutine = null;
    }

    public void MakeKinematicCargo(bool isKinematic)
    {
        //Debug.Log("MakeKinematicCargo " + isKinematic);
        if (isKinematic && kinematicReleaseCoroutine == null) StartCoroutine(MakeKinematicAfterTime(2));
        else CargoItself.GetComponent<Rigidbody>().isKinematic = false;
        if (armLockers.Any())
            foreach (var arm in armLockers)
                arm.rotation = Quaternion.Euler(-90, isKinematic? 30 : -30, 0);
    }

    private Coroutine kinematicReleaseCoroutine = null;

    private IEnumerator MakeKinematicAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        CargoItself.GetComponent<Rigidbody>().isKinematic = true;
        kinematicReleaseCoroutine = null;
    }

    public void UnKinematicAttach(bool isKinematic)
    {
        //Debug.Log("UnKinematicAttach " + isKinematic);
        PlaceToAttach.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    private void Update()
    {
        /*if (IsGrabbed)
            Debug.Log("mag " + CargoItself.GetComponent<Rigidbody>().velocity.magnitude + " vel_y " +
                      CargoItself.GetComponent<ObiRigidbody>().addVel.y);*/

        //Притягивание к нужной позиции для контейнеров //if (snapPlace != null
        if (IsGrabbed && !QuestStorage.Instance.SafeIsUnityNull() && !snapPlace.SafeIsUnityNull())
        {
            float distance = Vector3.Distance(CargoItself.transform.position, snapPlace.position);
            if (distance < reachDistance)
            {
                // if (snapPlace.position.y >= CargoItself.transform.position.y) return; //!snapBelow && 
                CargoItself.transform.position = Vector3.Lerp(CargoItself.transform.position,
                    new Vector3(snapPlace.position.x, CargoItself.transform.position.y, snapPlace.position.z),
                    Time.deltaTime * snapPower * (reachDistance - distance));

                //Debug.Log(CargoItself.transform.rotation.eulerAngles.y + " " + snapPlace.rotation.eulerAngles.y);

                var angDiff = Mathf.Abs(CargoItself.transform.rotation.eulerAngles.y - snapPlace.rotation.eulerAngles.y);
                if (angDiff >= 180) angDiff -= 180;
                // if (angDiff < 45 || angDiff > 135) //Углы, на которых срабатывает доворачивание
                // {
                    var snapRotation = Quaternion.Euler(0,
                        (int) ((CargoItself.transform.eulerAngles.y - snapPlace.eulerAngles.y + 90) / 180) * 180,
                        0); // + 45 / 90 * 90
                    //Debug.Log((int)((CargoItself.transform.eulerAngles.y - snapPlace.eulerAngles.y + 90) / 180) * 180); 
                    CargoItself.transform.rotation =
                        Quaternion.Lerp(
                            CargoItself.transform.rotation, snapRotation,
                            Time.deltaTime * snapPower * (reachDistance - distance));
                //}
            }
        }
    }
}
