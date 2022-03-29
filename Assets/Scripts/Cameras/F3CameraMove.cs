using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class F3CameraMove : MonoBehaviour
{
    [SerializeField] private Camera F3Camera;
    [SerializeField] private GameObject GrabDevice;
    [SerializeField] private float Sensitivity = 5;

    
    /*void Start()
    {
        if (!CraneControl.Instance.CurrentGrabControl.GrabItself.SafeIsUnityNull())
            GrabDevice = CraneControl.Instance.CurrentGrabControl.GrabItself;
    }*/
    
    void LateUpdate()
    {
        if(GrabDevice.SafeIsUnityNull()) GrabDevice = CraneControl.Instance.CurrentGrabControl.GrabItself;
        transform.position = GrabDevice.transform.position;
        
        var rotationX = Sensitivity * Mouse.current.delta.x.ReadValue() * Time.deltaTime;
        var rotationY = Sensitivity * Mouse.current.delta.y.ReadValue() * Time.deltaTime;

        Vector3 cameraRotation = transform.rotation.eulerAngles;

        if (!GetComponentInChildren<Camera>().SafeIsUnityNull())
        {
            if (Mouse.current.rightButton.ReadValue() > 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                transform.Rotate(Vector3.right, -rotationY); //cameraRotation.x -= rotationY;
                transform.Rotate(transform.up, rotationX); //cameraRotation.y += rotationX;
                transform.SetPositionAndRotation(transform.position,
                    Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
            if (Keyboard.current.ctrlKey.isPressed)
            {
                if (Mouse.current.scroll.y.ReadValue() == 120) F3Camera.fieldOfView = 
                    Mathf.Clamp(F3Camera.fieldOfView - 5.0f, 20, 75);

                if (Mouse.current.scroll.y.ReadValue() == -120) F3Camera.fieldOfView = 
                    Mathf.Clamp(F3Camera.fieldOfView + 5.0f, 20, 75);
            }
            
            #region Ограничение вращения камеры по Y

            if (F3Camera.enabled)
            {
                if (transform.rotation.eulerAngles.x < 90)
                {
                    transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.eulerAngles.x, -10, 60), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.eulerAngles.x, 300, 370), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }

                if (GetComponentInChildren<Camera>().transform.position.y < 2 || GetComponentInChildren<F3CameraItself>().isCollided)
                {
                    transform.Rotate(Vector3.right, Time.deltaTime * 50);
                }
            }
            #endregion

            
        }
    }

}
