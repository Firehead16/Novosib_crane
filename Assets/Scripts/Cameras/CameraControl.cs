using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] public Camera F1Camera, F2Camera, F3Camera, F4CameraMain, F4CameraSecond;
    [SerializeField] public Camera OnGroundCamera;
    private List<Camera> allCameras = new List<Camera>();
    
    void Start()
    {
        #region Добавление всех камер в лист

        allCameras.Add(F1Camera);
        allCameras.Add(F2Camera);
        allCameras.Add(F3Camera);
        allCameras.Add(F4CameraMain);
        allCameras.Add(F4CameraSecond);
        allCameras.Add(OnGroundCamera);

        #endregion

        CraneControl.Instance.inputActionAsset.FindAction("F1Camera").performed += context =>  SetCamera(1);
        CraneControl.Instance.inputActionAsset.FindAction("F2Camera").performed += context =>  SetCamera(2);
        CraneControl.Instance.inputActionAsset.FindAction("F3Camera").performed += context =>  SetCamera(3);
        CraneControl.Instance.inputActionAsset.FindAction("F4Camera").performed += context =>  SetCamera(4);
        CraneControl.Instance.inputActionAsset.FindAction("F5Camera").performed += context =>  SetCamera(5);
        
        SetCamera(4);
    }

    void SetCamera(int value)
    {
        switch (value)
        {
            /*case 0:
                foreach (var camera in allCameras)
                {
                    camera.gameObject.SetActive(false);
                }
                break;*/
            case 1:
                foreach (var camera in allCameras.Except(new List<Camera>(){F1Camera}))
                {
                    camera.gameObject.SetActive(false);
                }
                F1Camera.gameObject.SetActive(true);
                break;
            case 2:
                foreach (var camera in allCameras.Except(new List<Camera>(){F2Camera}))
                {
                    camera.gameObject.SetActive(false);
                }
                F2Camera.gameObject.SetActive(true);
                break;
            case 3:
                foreach (var camera in allCameras.Except(new List<Camera>(){F3Camera}))
                {
                    camera.gameObject.SetActive(false);
                }
                F3Camera.gameObject.SetActive(true);
                break;
            case 4:
                foreach (var camera in allCameras.Except(new List<Camera>(){F4CameraMain, F4CameraSecond}))
                {
                    camera.gameObject.SetActive(false);
                }
                F4CameraMain.gameObject.SetActive(true);
                F4CameraSecond.gameObject.SetActive(true);
                break;
            case 5:
                foreach (var camera in allCameras.Except(new List<Camera>(){OnGroundCamera}))
                {
                    camera.gameObject.SetActive(false);
                }
                OnGroundCamera.gameObject.SetActive(true);
                break;
        }
        CanvasControl.Instance.SetCrosshair(value);
    }
    
}
