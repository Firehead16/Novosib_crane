using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class SpreaderParameters : MonoBehaviour
{
    public float GrabberDistance;

    public bool Is20ft, Is40ft;

    public float f20ft = -0.3f;
    public float f40ft = 0.01f;

    [SerializeField] private Transform leftArm, rightArm;
    void Update()
    {
        GrabberDistance = (float)Math.Round(leftArm.localPosition.z, 2);

        Is40ft = GrabberDistance == 0.01f;
        Is20ft = GrabberDistance == -0.3f;

        leftArm.GetComponent<MovablePart>().enabled = CraneControl.craneState.CraneReadyEnabled && GetComponentInParent<GrabControl>().currentCargo.SafeIsUnityNull();
        rightArm.GetComponent<MovablePart>().enabled = CraneControl.craneState.CraneReadyEnabled && GetComponentInParent<GrabControl>().currentCargo.SafeIsUnityNull();
    }
}
