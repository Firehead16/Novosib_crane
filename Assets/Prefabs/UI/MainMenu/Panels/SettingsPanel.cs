using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
	public event Action OnControlButtonClick;
	//public event Action OnLampButtonClick;

	public event Action OnBackToMainButtonClick;
	
	[SerializeField]
	private TMP_InputField InputJoystickA, InputJoystickB, InputJoystickC;

	[SerializeField]
	private Button controlButton = null;

	// [SerializeField]
	// private Button lampButton = null;

	[SerializeField]
	private Button backToMainButton = null;

	[SerializeField] private Button SaveJoysticksButton = null;


	private void Start()
	{
		string AppPath = Path.GetDirectoryName(Application.dataPath);
		string JoySettings = "";
		if (File.Exists(AppPath + "\\Config\\sticks.ini"))
		{
			JoySettings = File.ReadLines(AppPath + "\\Config\\sticks.ini").First();
			Debug.Log("From File " + JoySettings);
			string[] subStrings = JoySettings.Split(' ');
			InputJoystickA.text = subStrings[0];
			InputJoystickB.text = subStrings[1];
			InputJoystickC.text = subStrings[2];
		}

		controlButton.onClick.AddListener(() => OnControlButtonClick?.Invoke());
		//lampButton.onClick.AddListener(() => OnLampButtonClick?.Invoke());
		backToMainButton.onClick.AddListener(() => OnBackToMainButtonClick?.Invoke());
		SaveJoysticksButton.onClick.AddListener((() => SaveJoystickSettings()));
	}
	
	public void SaveJoystickSettings()
	{
		string AppPath = Path.GetDirectoryName(Application.dataPath);
		File.WriteAllText(AppPath + "\\Config\\sticks.ini", InputJoystickA.text + " " + InputJoystickB.text + " " + InputJoystickC.text);
	}
}
