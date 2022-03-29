using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core.Extensions;
using Core.Settings;
using Core.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public struct PathData
{
	private string path;
	public string Path => path;


	private string device;
	public string Device => device;


	private string displayName;
	public string DisplayName => displayName;


	private string usage;
	public string Usage => usage;


	public PathData(string path, string device, string displayName, string usage)
	{
		this.path = path;
		this.device = device;
		this.displayName = displayName;
		this.usage = usage;
	}

	public static string GetPathText(PathData path)
	{
		var aSplit = path.Path.Split('/');
		var b = new StringBuilder();
		foreach (string s in aSplit.Skip(2))
		{
			b.Append("/" + s);
		}
		return "<" + aSplit[1] + ">" + path.Usage + b;
	}
}


[RequireComponent(typeof(CanvasGroupLerpHideAndShowBehavior))]
public class InputListenPanel : GamepadISX, IRebindMenu, IHideAndShowBehavior
{
	private BindingButton currentBindingButton;

	[SerializeField, Tooltip("scroll view content")] private Transform content = null;


	private IHideAndShowBehavior hideAndShowBehaviorScript;

	private InputAction mAxisMoveAction; // ввод аксисов
	private InputAction mAxisVector2Action; // ввод Vector2
	private List<PathData> AllPressPath = new List<PathData>();


	public bool IsNewInstanse { get; set; }
	public bool IsLoaded { get; set; }
	public bool IsInitialized { get; set; }


	public void Load()
	{
		hideAndShowBehaviorScript = GetComponent<CanvasGroupLerpHideAndShowBehavior>();
	}


	public void Initialize()
	{
		this.DebugInitialize();
		PrepareBaseClass();
		Hide();
	}

	public void Unload() { }


	/// <summary>
	/// Показать меню
	/// </summary>
	public virtual void Show(bool isTimeShow = false)
	{
		Enable();
		if (hideAndShowBehaviorScript != null) hideAndShowBehaviorScript.Show();
		AllPressPath.Clear();
		UpdateButtonList();
	}

	/// <summary>
	/// Скрыть меню
	/// </summary>
	public virtual void Hide()
	{
		Disable();
		if (hideAndShowBehaviorScript != null) hideAndShowBehaviorScript.Hide();
	}

	public GameObject GameObject => gameObject;

	public void Notify(Message message)
	{
		this.DebugNotify(message);
		switch (message.Type)
		{
			case Messages.RebindControl.StartRebind:
				Show();
				currentBindingButton = message.Content[0] as BindingButton;
				break;
		}
	}

	private void AddPath(PathData data)
	{
		if (!content.gameObject.activeSelf) return;
		if (AllPressPath.Exists(x => PathData.GetPathText(x) == PathData.GetPathText(data))) return;
		AllPressPath.Add(data);
		UpdateButtonList();
	}

	public void UpdateButtonList()
	{
		foreach (Transform contentChild in content)
		{
			Destroy(contentChild.gameObject);
		}
		if (AllPressPath.Count == 0) return;
		foreach (var path in AllPressPath)
		{
			var currentpathButton = UiBuilder.CreateButton(content, Vector3.one, PathData.GetPathText(path));
			currentpathButton.onClick.AddListener(() =>
			{
				AllPressPath.Clear();
				Hide();
				currentBindingButton.PathData = path;
			});
		}
	}

	/// <summary>
	/// открыть панель с выбором
	/// </summary>
	public void OpenPanel()
	{
	}

	void PrepareBaseClass()
	{
		MStickMoveAction = new InputAction("StickMoveAction", InputActionType.PassThrough, "*/<stick>");
		MStickMoveAction.performed += callbackContext => StickMove(callbackContext.control as StickControl);
		MStickMoveAction.canceled += callbackContext => StickMove(callbackContext.control as StickControl);
		MStickMoveAction.Enable();

		MButtonAction = new InputAction("ButtonPressAction", InputActionType.PassThrough, "*/<button>");
		MButtonAction.performed += callbackContext => OnButtonPress(callbackContext.control as ButtonControl);
		MButtonAction.canceled += callbackContext => OnButtonPress(callbackContext.control as ButtonControl);
		MButtonAction.Enable();

		MdPadAction = new InputAction("Dpadpressaction", InputActionType.PassThrough, "*/<dpad>");
		MdPadAction.performed += callbackContext => OnDpadPress(callbackContext.control as DpadControl);
		MdPadAction.canceled += callbackContext => OnDpadPress(callbackContext.control as DpadControl);
		MdPadAction.Enable();

		mAxisMoveAction = new InputAction("AxisMoveAction", InputActionType.PassThrough, "*/<Axis>");
		mAxisMoveAction.performed += callbackContext => AxisMove(callbackContext.control as AxisControl);
		mAxisMoveAction.canceled += callbackContext => AxisMove(callbackContext.control as AxisControl);
		mAxisMoveAction.Enable();

		mAxisVector2Action = new InputAction("Vector2Action", InputActionType.PassThrough, "*/<Vector2>");
		mAxisVector2Action.performed += callbackContext => OnVector2Press(callbackContext.control as Vector2Control);
		mAxisVector2Action.canceled += callbackContext => OnVector2Press(callbackContext.control as Vector2Control);
		mAxisVector2Action.Enable();

	}

	private void Enable()
	{
		if (MButtonAction != null) MButtonAction.Enable();
		if (MdPadAction != null) MdPadAction.Enable();
		if (MStickMoveAction != null) MStickMoveAction.Enable();
		if (mAxisMoveAction != null) mAxisMoveAction.Enable();
		if (mAxisVector2Action != null) mAxisVector2Action.Enable();
	}

	private void Disable()
	{
		MButtonAction?.Disable();
		MdPadAction?.Disable();
		MStickMoveAction?.Disable();
		mAxisMoveAction?.Disable();
		mAxisVector2Action?.Disable();
	}

	#region Нажатия устройства

	private void OnButtonPress(ButtonControl control)
	{
		var deviceLayoutName = InputSystem.TryFindMatchingLayout(control.device.description);
		string usage = DeviceSettings.GetNeedUsage(control.device);
		string usageMatch = !string.IsNullOrEmpty(usage) ? "{" + usage + "}" : "";

		Regex regex = new Regex($"{control.device.description.manufacturer}\\s{control.device.description.product}" + "\\d{0,2}");
		var correctPath = regex.Replace(control.path, deviceLayoutName);

		PathData pathData = new PathData(correctPath, control.device.displayName, control.displayName, usageMatch);
		AddPath(pathData);
		string device = control.device.description.deviceClass;
		if (device == "Keyboard" || device == "Mouse") return;
		OnControllerButtonPress(control);
	}

	protected override void StickMove(StickControl control)
	{
		var deviceLayoutName = InputSystem.TryFindMatchingLayout(control.device.description);
		string usage = DeviceSettings.GetNeedUsage(control.device);
		string usageMatch = !string.IsNullOrEmpty(usage) ? "{" + usage + "}" : "";

		Regex regex = new Regex($"{control.device.description.manufacturer}\\s{control.device.description.product}" + "\\d{0,2}");
		var correctPath = regex.Replace(control.path, deviceLayoutName);

		PathData pathData = new PathData(correctPath, control.device.displayName, control.displayName, usageMatch);
		AddPath(pathData);
	}

	protected void AxisMove(AxisControl control)
	{
		var deviceLayoutName = InputSystem.TryFindMatchingLayout(control.device.description);
		string usage = DeviceSettings.GetNeedUsage(control.device);
		string usageMatch = !string.IsNullOrEmpty(usage) ? "{" + usage + "}" : "";

		Regex regex = new Regex($"{control.device.description.manufacturer}\\s{control.device.description.product}" + "\\d{0,2}");
		var correctPath = regex.Replace(control.path, deviceLayoutName);

		PathData pathData = new PathData(correctPath, control.device.displayName, control.displayName, usageMatch);
		AddPath(pathData);
	}

	private void OnVector2Press(Vector2Control control)
	{
		var deviceLayoutName = InputSystem.TryFindMatchingLayout(control.device.description);
		string usage = DeviceSettings.GetNeedUsage(control.device);
		string usageMatch = !string.IsNullOrEmpty(usage) ? "{" + usage + "}" : "";


		Regex regex = new Regex($"{control.device.description.manufacturer}\\s{control.device.description.product}" + "\\d{0,2}");
		var xPath = regex.Replace(control.x.path, deviceLayoutName);
		var yPath = regex.Replace(control.y.path, deviceLayoutName);

		PathData pathDataX = new PathData(xPath, control.device.displayName, control.displayName + "/x", usageMatch);
		PathData pathDataY = new PathData(yPath, control.device.displayName, control.displayName + "/y", usageMatch);

		AddPath(pathDataX);
		AddPath(pathDataY);
	}

	protected override void OnDpadPress(DpadControl control)
	{
		var deviceLayoutName = InputSystem.TryFindMatchingLayout(control.device.description);
		string usage = DeviceSettings.GetNeedUsage(control.device);
		string usageMatch = !string.IsNullOrEmpty(usage) ? "{" + usage + "}" : "";

		Regex regex = new Regex($"{control.device.description.manufacturer}\\s{control.device.description.product}" + "\\d{0,2}");

		PathData pathDataUp = new PathData(regex.Replace(control.up.path, deviceLayoutName), control.device.displayName, control.displayName + "/up", usageMatch);
		PathData pathDataDown = new PathData(regex.Replace(control.down.path, deviceLayoutName), control.device.displayName, control.displayName + "/down", usageMatch);
		PathData pathDataLeft = new PathData(regex.Replace(control.left.path, deviceLayoutName), control.device.displayName, control.displayName + "/left", usageMatch);
		PathData pathDataRight = new PathData(regex.Replace(control.right.path, deviceLayoutName), control.device.displayName, control.displayName + "/right", usageMatch);

		if (control.up.isPressed) AddPath(pathDataUp);
		if (control.down.isPressed) AddPath(pathDataDown);
		if (control.left.isPressed) AddPath(pathDataLeft);
		if (control.right.isPressed) AddPath(pathDataRight);

	}

	#endregion

}
