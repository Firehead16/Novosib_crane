using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CabinElements : MonoBehaviour
{
    [SerializeField] private Transform leftJoystick, rightJoystick, thirdJoystick, ropeSpeedTumbler, key;

    [SerializeField] [CanBeNull] private Material ControlEnabled,
        ReadyEnabled,
        OverWeight,
        MoveEnabled,
        TowerEnabled,
        ArrowEnabled,
        RopeEnabled,
        RailEnabled,
        ControlerFail,
        RopeFail,
        ArrowFail,
        TowerFail,
        RailFail,
        Slab1Green,
        Slab1Red,
        Slab2Green,
        Slab2Red;

    void Update()
    {
        if (Hardware.Instance.IsConnected())
        {
            leftJoystick.localRotation =
                Quaternion.Euler(
                    Joystick.all[CraneControl.JoystickA].stick.x.ReadValue() * 15f,
                    -Joystick.all[CraneControl.JoystickA].stick.y.ReadValue() * 15f, 0);
        
            rightJoystick.localRotation =
                Quaternion.Euler(
                    Joystick.all[CraneControl.JoystickB].stick.x.ReadValue() * 15f,
                    -Joystick.all[CraneControl.JoystickB].stick.y.ReadValue() * 15f, 0);
        
            thirdJoystick.localRotation =
                Quaternion.Euler(
                    Joystick.all[CraneControl.JoystickC].stick.x.ReadValue() * 15f,
                    -Joystick.all[CraneControl.JoystickC].stick.y.ReadValue() * 15f, 0);
        }

        switch (CraneControl.Instance.CurrentRopeSpeed)
        {
            case CraneControl.RopeSpeed.FirstSpeed:
                ropeSpeedTumbler.localRotation = Quaternion.Euler(0,0,-30);
                break;
            case CraneControl.RopeSpeed.SecondSpeed:
                ropeSpeedTumbler.localRotation = Quaternion.Euler(0,0,0);
                break;
            case CraneControl.RopeSpeed.ThirdSpeed:
                ropeSpeedTumbler.localRotation = Quaternion.Euler(0,0,30);
                break;
        }

        key.localRotation = Quaternion.Euler(CraneControl.craneState.CraneControlEnabled? 0 : 90, -90, -90);
        
        SetLamp(ControlEnabled, CraneControl.craneState.CraneControlEnabled);
        SetLamp(ReadyEnabled, CraneControl.craneState.CraneReadyEnabled);
        SetLamp(OverWeight, CraneControl.craneState.CraneOverWeight);
        SetLamp(MoveEnabled, CraneControl.craneState.CraneMoveEnabled);
        SetLamp(TowerEnabled, CraneControl.craneState.CraneTowerEnabled);
        SetLamp(ArrowEnabled, CraneControl.craneState.CraneArrowEnabled);
        SetLamp(RopeEnabled, CraneControl.craneState.CraneRopeEnabled);
        SetLamp(RailEnabled, CraneControl.craneState.CraneRailEnabled);
        SetLamp(ControlerFail, CraneControl.craneState.CraneControllerFail);
        SetLamp(RopeFail, CraneControl.craneState.CraneRopeFail);
        SetLamp(ArrowFail, CraneControl.craneState.CraneArrowFail);
        SetLamp(TowerFail, CraneControl.craneState.CraneTowerFail);
        SetLamp(RailFail, CraneControl.craneState.CraneRailFail);
        SetLamp(Slab1Green, CraneControl.craneState.CraneSlab1Green);
        SetLamp(Slab1Red, CraneControl.craneState.CraneSlab1Red);
        SetLamp(Slab2Green, CraneControl.craneState.CraneSlab2Green);
        SetLamp(Slab2Red, CraneControl.craneState.CraneSlab2Red);
        
    }
    
    

    void SetLamp(Material lamp, bool value)
    {
        if (value) lamp.EnableKeyword("_EMISSION");
        else lamp.DisableKeyword("_EMISSION");
    }
}
