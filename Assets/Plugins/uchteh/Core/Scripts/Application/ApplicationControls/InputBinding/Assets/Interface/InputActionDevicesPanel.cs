using System;
using System.Collections.Generic;
using System.Linq;
using Core.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionDevicesPanel : Panel, IRebindMenu
{
	public event Action<DeviceInfo> OnSelectDevice; 

	[SerializeField, Tooltip("scroll view content")] 
	private Table table = null;

	private List<DeviceInfo> devices = new List<DeviceInfo>();

	public override void Initialize()
	{
		base.Initialize();

		table.OnSelectLine += line => { OnSelectDevice?.Invoke(line.TableObject); };
	}

	public override void Show(bool isTimeShow = false)
	{
		base.Show(isTimeShow);
		FillTable();
	}

	private void FillTable()
	{
		table.Clear();
		devices.Clear();

		foreach (var inputDevice in InputSystem.devices)
		{
			devices.Add(new DeviceInfo()
			{
				Id = inputDevice.deviceId,
				Description = inputDevice.layout
			});
		}

		if (devices.Any())
		{
			foreach (var deviceInfo in devices)
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