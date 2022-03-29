using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnGroundCameraMovemvent : MonoBehaviour
{
    [SerializeField]
    private Camera CameraOnGround;

    //public static InputControl inputControl;

    private RaycastHit hit;
    private bool RayHit;

    private bool isPulledFOV = false;

    [SerializeField] private int xMin, xMax, zMin, zMax;

    void Movement(float xMove, float zMove)
    {
        //Vector3 lastRot = transform.rotation.eulerAngles;

        if (Math.Abs(xMove) > 0.05f) transform.Translate(Vector3.right * (xMove * Time.deltaTime * 2));

        if (Math.Abs(zMove) > 0.05f) transform.Translate(Vector3.forward * (zMove * Time.deltaTime * 2));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), transform.position.y,
            Mathf.Clamp(transform.position.z, zMin, zMax));

        float Sensitivity = 5.0f;

        //float RotationX = Sensitivity * Mouse.current.delta.x.ReadValue() * Time.deltaTime;
        //float RotationY = Sensitivity * Mouse.current.delta.y.ReadValue() * Time.deltaTime;
        Vector3 CameraRotation = CameraOnGround.transform.rotation.eulerAngles;
        CameraOnGround.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(CameraRotation));
        transform.rotation = Quaternion.Euler(0, CameraRotation.y, 0);
    }

    /*void AvoidCargo()
    {
        RayHit = Physics.Raycast(transform.position, Vector3.up, out hit, 6);
        if (RayHit) transform.Translate(Vector3.forward * (Time.deltaTime * 5));
    }*/

    //public void AvoidQuestTrigger() => transform.Translate(Vector3.forward * Time.deltaTime * 5);

    //private bool PullDelay = true;
    /*private IEnumerator PullOutFOV()
    {
        PullDelay = false;
        pullButton = false;
        if (isPulledFOV)
        {
            CameraOnGround.fieldOfView = 60;
            isPulledFOV = false;
        }
        else
        {
            CameraOnGround.fieldOfView = 20;
            isPulledFOV = true;
        }
        yield return new WaitForSeconds(1);
        PullDelay = true;
    }*/

    void Update()
    {
        float Xmovement = 0, Zmovement = 0;
        if (Joystick.all.Count > 1)
        {
            Vector2 tmp = (Vector2)Joystick.all[CraneControl.JoystickA].allControls[13].ReadValueAsObject();
            Xmovement = tmp.x;
            Zmovement = tmp.y;
        }
        else
        {
            Xmovement = - Keyboard.current.leftArrowKey.ReadValue() + Keyboard.current.rightArrowKey.ReadValue();
            Zmovement = - Keyboard.current.downArrowKey.ReadValue() + Keyboard.current.upArrowKey.ReadValue();
        }

        if(CameraOnGround.isActiveAndEnabled) Movement(Xmovement * 4.0f, Zmovement * 4.0f);

        //AvoidCargo();
        //if (pullButton && PullDelay && CameraOnGround.enabled) StartCoroutine(PullOutFOV());
    }
}
