using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;

public abstract class Manager<TCtrl> : BaseControlMethods//, IBaseManager<TCtrl>
	where TCtrl : class, IControl
{

	protected Dictionary<string, TCtrl> Сontrols;


	public Dictionary<string, TCtrl> Controls
	{
		get => Сontrols;
		set => Сontrols = value;
	}

	//переопределямый список объектов которыми будет управлять  менеджер
	public virtual IReadOnlyCollection<TCtrl> GetChildControls()
	{
		return GetComponentsInChildren<TCtrl>();
	}


	public virtual void AddControlOnLoad(TCtrl loadedControl)
	{
		Controls.Add(GetControlKey(loadedControl), loadedControl);
	}

	public virtual string GetControlKey(TCtrl loadedControl)
	{
		return loadedControl.GetType().ToString();
	}

	public override void Load()
	{
		base.Load();

		LoadControls(GetChildControls());
		OnControlsLoaded();
	}

	public override void Unload()
	{
		base.Unload();
	}


	protected virtual void OnControlsLoaded()
	{

	}


	public void LoadControls(IReadOnlyCollection<TCtrl> controls)
	{
		Controls = new Dictionary<string, TCtrl>();
		foreach (var control in controls)
		{
			control.Load();
			control.IsLoaded = true;
			AddControlOnLoad(control);
		}
	}

	/// <summary>
	/// Получить итем по типу
	/// </summary>
	/// <returns></returns>
	public TCtrl GetControl(Type type, bool needError = true)
	{
		try
		{
			var result = Controls.Values.First(x => x.GetType().GetInterfaces().Contains(type));
			return result;
		}
		catch (Exception)
		{
			if (needError && SettingsStorage.Settings.Debuging) this.DebugLogError("Не найден Control типа:" + type);
			return null;
		}

	}

	/// <summary>
	/// Получить итем по типу
	/// </summary>
	/// <returns></returns>
	public T GetControl<T>() where T : class, TCtrl
	{
		var result = (T)Controls.Values.First(x => x is T);
		if (result != null)
		{
			return result;
		}

		this.DebugLogError("Не найден Control типа:" + typeof(T));
		return null;
	}


}