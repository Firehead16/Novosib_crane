using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Settings;
using Core.Ui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum RebindControl
		{
			ShowRebind,
			HideRebind,
			ShowRebindAssets,
			StartRebind,
		}
	}
}

public class InputBindingControl : CanvasControl<InputActionAssetsPreviewPanel>, IInputBindingControl
{
	private InputActionAssetsPreviewPanel previewPanel;
	private InputListenPanel inputListenPanel;

	private InputActionEditDevicePanel editDevicePanel;
	private InputActionDevicesPanel devicesPanel;

	public bool IsInitialized { get; set; }

	protected override void OnItemsInitialised()
	{
		previewPanel = GetControl<InputActionAssetsPreviewPanel>();
		inputListenPanel = GetControl<InputListenPanel>();

		editDevicePanel = GetControl<InputActionEditDevicePanel>();
		devicesPanel = GetControl<InputActionDevicesPanel>();

		previewPanel.OnBindingButtonClick += bindingButton => inputListenPanel.Notify(new Message(Messages.RebindControl.StartRebind, bindingButton));
		previewPanel.OnSaveButtonClick += Save;
		previewPanel.OnQuitButtonClick += () => Notify(new Message(Messages.RebindControl.HideRebind));
		previewPanel.OnEditDeviceButtonClick += () =>
		{
			previewPanel.Hide();
			inputListenPanel.Hide();
			editDevicePanel.Show();
		};
		previewPanel.Notify(new Message(Messages.RebindControl.ShowRebindAssets));

		editDevicePanel.OnSelectLine += () => devicesPanel.Show();
		editDevicePanel.OnQuitButtonClick += () =>
		{
			editDevicePanel.Hide();
			devicesPanel.Hide();
			previewPanel.Show();
		};
		editDevicePanel.OnSaveButtonClick += (deviceInfoStorage) =>
		{
			DeviceSettings.SaveInFile(deviceInfoStorage);
			SetDeviceUsage();

			editDevicePanel.Hide();
			devicesPanel.Hide();
			previewPanel.Show();
		};

		devicesPanel.OnSelectDevice += info =>
		{
			devicesPanel.Hide();
			editDevicePanel.Refresh(info);
		};

		SetDeviceUsage();
	}

	public override void Notify(Message message)
	{
		switch (message.Type)
		{
			case Messages.RebindControl.ShowRebind:
				Show();
				break;
			case Messages.RebindControl.HideRebind:
				Hide();
				break;
		}
		previewPanel.Notify(message);
	}

	/// <summary>
	/// Сохранить изменения
	/// </summary>
	/// <param name="changedAssets"></param>
	private void Save(Dictionary<string, InputActionAsset> changedAssets)
	{
		foreach (var asset in changedAssets.Values)
		{
			SaveInputAsset(asset);
		}
	}

	/// <summary>
	/// Прочитать управление
	/// </summary>
	/// <param name="asset"></param>
	/// <returns></returns>
	public InputActionAsset ReadInputAsset(InputActionAsset asset)
	{
		var customInputAssetsFolder = RebindAssetsSettings.Default().CustomInputAssetFolder;
		if (File.Exists(Application.dataPath + customInputAssetsFolder + asset.name + ".inputactions"))
			return InputActionAsset.FromJson(File.ReadAllText(Application.dataPath + customInputAssetsFolder + asset.name + ".inputactions"));
		return asset;
	}

	/// <summary>
	/// Сохранить управление
	/// </summary>
	/// <param name="asset"></param>
	public void SaveInputAsset(InputActionAsset asset)
	{
		var customInputAssetsFolder = RebindAssetsSettings.Default().CustomInputAssetFolder;
		if (!Directory.Exists(Application.dataPath + customInputAssetsFolder))
			Directory.CreateDirectory(Application.dataPath + customInputAssetsFolder);
		File.WriteAllText(Application.dataPath + customInputAssetsFolder + asset.name + ".inputactions", asset.ToJson());
	}

	public void SetDeviceUsage()
	{
		try
		{
			DeviceInfoStorage deviceInfoStorage = DeviceSettings.ReadFromFile();

			foreach (var deviceInfo in deviceInfoStorage.Devices)
			{
				InputDevice device = InputSystem.devices.FirstOrDefault(d => d.deviceId == deviceInfo.Id);
				if (device != null)
				{
					InputSystem.SetDeviceUsage(device, deviceInfo.Description);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}

	}
}