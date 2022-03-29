using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Testing;
using Core.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionEditDevicePanel : Panel, IRebindMenu
{
	public event Action OnSelectLine;
	public event Action OnQuitButtonClick;
	public event Action<DeviceInfoStorage> OnSaveButtonClick;

	[SerializeField, Tooltip("Scroll view content")]
	private Table table = null;

	[SerializeField]
	private Button quitButton = null;

	[SerializeField]
	private Button backButton = null;

	[SerializeField]
	private Button saveButton = null;

	private DeviceInfoStorage deviceInfoStorage;

	public override void Initialize()
	{
		base.Initialize();

		table.OnSelectLine += line => OnSelectLine?.Invoke();

		quitButton.onClick.AddListener(() => OnQuitButtonClick?.Invoke());
		backButton.onClick.AddListener(() => OnQuitButtonClick?.Invoke());
		saveButton.onClick.AddListener(() => OnSaveButtonClick?.Invoke(deviceInfoStorage));
	}

	public override void Show(bool isTimeShow = false)
	{
		base.Show(isTimeShow);

		deviceInfoStorage = DeviceSettings.ReadFromFile();
		FillTable();
	}

	public void Refresh(DeviceInfo info)
	{
		var currentTableObject = (DeviceInfo) table.CurLine.TableObject;
		currentTableObject.Id = info.Id;

		FillTable();
	}

	private void FillTable()
	{
		table.Clear();

		if (deviceInfoStorage.Devices.Any())
		{
			foreach (var deviceInfo in deviceInfoStorage.Devices)
			{
				TableLine line = new TableLine(table, deviceInfo, new List<Button>()
				{
					UiBuilder.CreateButton(table.transform.GetChild(0).transform, Vector3.one, deviceInfo.Id.ToString()),
					UiBuilder.CreateButton(table.transform.GetChild(1).transform, Vector3.one, deviceInfo.Description),
				});
				table.AddLine(line);
			}
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
	}
}