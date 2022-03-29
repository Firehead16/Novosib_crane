using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class F2CameraZoom : MonoBehaviour
{
    private Camera F2Camera;

    private void Awake() => F2Camera = GetComponent<Camera>();

    void Update()
    {
        if (Keyboard.current.shiftKey.isPressed)
        {
            if (Mouse.current.scroll.y.ReadValue() == 120) F2Camera.fieldOfView = 
                Mathf.Clamp(F2Camera.fieldOfView - 5.0f, 20, 75);

            if (Mouse.current.scroll.y.ReadValue() == -120) F2Camera.fieldOfView = 
                Mathf.Clamp(F2Camera.fieldOfView + 5.0f, 20, 75);
        }
    }
}
