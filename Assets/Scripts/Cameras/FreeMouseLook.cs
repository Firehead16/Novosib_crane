using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]

public class FreeMouseLook : MonoBehaviour
{
	private Camera freelookCamera;

	private RaycastHit DescriptionHit;

	[SerializeField]
	private TMP_Text DescriptionText;

	[SerializeField]
	private bool InCabinCamera;

	[SerializeField] 
	private float Sensitivity = 5.0f;
	
	private Vector2 tmp = Vector2.zero;


	private void Start() => freelookCamera = transform.GetComponent<Camera>();

	private void Update()
	{
		if(Joystick.all.Count > 1) tmp = (Vector2)Joystick.all[CraneControl.JoystickB].allControls[13].ReadValueAsObject();
		var rotationX = (Sensitivity * Mouse.current.delta.x.ReadValue() + tmp.x * 20) * Time.deltaTime;
		var rotationY = (Sensitivity * Mouse.current.delta.y.ReadValue() + tmp.y * 20) * Time.deltaTime;

		Vector3 cameraRotation = freelookCamera.transform.rotation.eulerAngles;

		if (freelookCamera.enabled)
		{
			if (!tmp.Equals(Vector2.zero) || Mouse.current.rightButton.ReadValue() > 0)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;

				cameraRotation.x -= rotationY;
				cameraRotation.y += rotationX;
			}
			else
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}

		if (InCabinCamera)
		{
			if (Keyboard.current.ctrlKey.isPressed)
			{
				if (Mouse.current.scroll.y.ReadValue() == 120) GetComponent<Camera>().fieldOfView = 
					Mathf.Clamp(GetComponent<Camera>().fieldOfView - 5.0f, 20, 75);

				if (Mouse.current.scroll.y.ReadValue() == -120) GetComponent<Camera>().fieldOfView = 
					Mathf.Clamp(GetComponent<Camera>().fieldOfView + 5.0f, 20, 75);
			}
		}

		#region Ограничение вращения камеры по Y

		if (freelookCamera.enabled)
		{
			if (freelookCamera.transform.rotation.eulerAngles.x < 90)
			{
				freelookCamera.transform.rotation = Quaternion.Euler(Mathf.Clamp(cameraRotation.x, -5, 60), cameraRotation.y, cameraRotation.z);
			}
			else
			{
				freelookCamera.transform.rotation = Quaternion.Euler(Mathf.Clamp(cameraRotation.x, 300, 370), cameraRotation.y, cameraRotation.z);
			}
		}
		#endregion

		#region Описания при наведении курсора

		if (freelookCamera.isActiveAndEnabled && !DescriptionText.SafeIsUnityNull())
		{
			bool outOfCamera = Physics.Raycast(freelookCamera.transform.position, freelookCamera.transform.forward, out DescriptionHit);
			if (outOfCamera && DescriptionHit.transform.GetComponent<Description>() != null)
			{
				//Debug.Log("obj " + DescriptionHit.transform);
				if (InCabinCamera == DescriptionHit.transform.GetComponent<Description>().InCabin)
				{
					DescriptionText.text = DescriptionHit.transform.GetComponent<Description>().DescriptionString;
				}
			}
			else DescriptionText.text = "";
		}
		#endregion
	}
}
