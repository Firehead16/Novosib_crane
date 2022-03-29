using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Users;


namespace UnityEngine.InputSystem
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class JoystickWithTag
	{
		static JoystickWithTag()
		{
			InputSystem.RegisterLayoutOverride(@"
			{
				""name"" : ""JoystickWithUsageTags"",
				""extend"" : ""Joystick"",
				""commonUsages"" : [
				""Left"", ""Right""
				]
			}
			");
		}
	}

	[Serializable]
	public class  DeviceInfoStorage
	{
		[SerializeField]
		public List<DeviceInfo> Devices = new List<DeviceInfo>();
	}

	[Serializable]
	public class DeviceInfo
	{
		[SerializeField]
		public int Id;

		[SerializeField]
		public string Description;
	}

	[Serializable]
	[CreateAssetMenu(fileName = "New DeviceSettings", menuName = "Settings/DeviceSettings")]
	public class DeviceSettings: SerializedScriptableObject
	{
		[SerializeField] 
		public DeviceInfoStorage Storage = new DeviceInfoStorage();

		public static string GetNeedUsage(InputDevice device)
		{
			var storage = ReadFromFile();

			foreach (var deviceInfo in storage.Devices)
			{
				if (deviceInfo.Id == device.deviceId)
				{
					return deviceInfo.Description;
				}
			}

			return null;
		}
		
		public static DeviceInfoStorage ReadFromFile()
		{
			var folder = RebindAssetsSettings.Default().CustomInputAssetFolder;
			if (File.Exists(Application.dataPath + folder + "DeviceSettings.cfg"))
				return JsonUtility.FromJson<DeviceInfoStorage>(File.ReadAllText(Application.dataPath + folder + "DeviceSettings.cfg"));
			throw new Exception("Нет файла настроек устройств");
		}

		public static void SaveInFile(DeviceInfoStorage storage)
		{
			var folder = RebindAssetsSettings.Default().CustomInputAssetFolder;
			if (!Directory.Exists(Application.dataPath + folder))
				Directory.CreateDirectory(Application.dataPath + folder);
			File.WriteAllText(Application.dataPath + folder + "DeviceSettings.cfg", JsonUtility.ToJson(storage, true));
		}

		[Button]
		public  void Read()
		{
			var folder = RebindAssetsSettings.Default().CustomInputAssetFolder;
			if (File.Exists(Application.dataPath + folder + "DeviceSettings.cfg"))
				Storage = JsonUtility.FromJson<DeviceInfoStorage>(File.ReadAllText(Application.dataPath + folder + "DeviceSettings.cfg"));
			throw new Exception("Нет файла настроек устройств");
		}

		[Button]
		public void Save()
		{
			var folder = RebindAssetsSettings.Default().CustomInputAssetFolder;
			if (!Directory.Exists(Application.dataPath + folder))
				Directory.CreateDirectory(Application.dataPath + folder);
			File.WriteAllText(Application.dataPath + folder + "DeviceSettings.cfg", JsonUtility.ToJson(Storage, true));
		}
	}
}
