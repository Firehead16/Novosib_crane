using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class PlaceToAttach : MonoBehaviour
{
    public bool didAlready = false;

    public GrabControl grabControl = null;

    private float angleDiff;

    void Start()
    {
        if (GetComponentInParent<Cargo>().IsAvaliable) GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<GrabControl>() != null && other.GetComponent<GrabControl>().currentCargo == null ||
             other.GetComponentInParent<GrabControl>() != null && other.GetComponentInParent<GrabControl>().currentCargo == null) && !didAlready)
        {
            grabControl = other.GetComponent<GrabControl>() != null
                ? other.GetComponent<GrabControl>()
                : other.GetComponentInParent<GrabControl>();
            GetComponentInParent<Cargo>().GrabControl = grabControl;
            Debug.Log("GotIt " + grabControl);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        grabControl = null;
        CanvasControl.Instance.OffAngleDiff();
    }

    void Update()
    {
        if (GetComponentInParent<Cargo>().IsAvaliable &&!grabControl.SafeIsUnityNull() && !didAlready &&
            (grabControl.CurrentLiftingDevice != GrabControl.LiftingDevice.Greifer))
        {
            angleDiff = Mathf.Round(Mathf.Abs(transform.rotation.eulerAngles.y -
                                              grabControl.GrabItself.transform.rotation.eulerAngles.y));
            if (angleDiff >= 180) angleDiff -= 180;
            CanvasControl.Instance.SetAngleDiff(angleDiff);

            if (angleDiff < 5 || angleDiff > 175)
            {
                //Debug.Log("PTA grab");

                switch (grabControl.CurrentLiftingDevice)
                {
                    case GrabControl.LiftingDevice.Hook:
                    case GrabControl.LiftingDevice.Traverse:
                    case GrabControl.LiftingDevice.AutoTraverse:
                        GetComponentInParent<Cargo>().GrabInCargo();
                        break;
                    case GrabControl.LiftingDevice.Spreader:
                        if (grabControl.GrabItself.GetComponent<Rigidbody>().velocity.y < 0 &&
                            CraneControl.Instance.inputActionAsset.FindAction("SpreaderGrab").ReadValue<float>() > 0 &&
                            (QuestStorage.Instance.QuestType != Quest.Containers || grabControl.CurrentLiftingDevice != GrabControl.LiftingDevice.Spreader ||
                             QuestStorage.Instance.CargoType == CargoType.Container20Ft && grabControl.GetComponentInChildren<SpreaderParameters>().Is20ft ||
                             QuestStorage.Instance.CargoType == CargoType.Container40Ft && grabControl.GetComponentInChildren<SpreaderParameters>().Is40ft))
                        {
                            GetComponentInParent<Cargo>().GrabInCargo();
                        }
                        // Debug.Log("spreader y vel " + grabControl.GrabItself.GetComponent<Rigidbody>().velocity.y);
                        break;
                }
            }
        }
    }
}
