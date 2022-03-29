using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class AbstractManager<TCtrl> : BaseControlMethods, IAbstractManager<TCtrl> where TCtrl : class, IControl
{
	[ShowInInspector, ReadOnly]
	public Dictionary<string, TCtrl> Controls { get; protected set; }

	public abstract IReadOnlyCollection<TCtrl> GetChildControls();

	public override void Load()
	{
		base.Load();

		LoadControls(GetChildControls());
		ControlsLoaded();
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


	public abstract void AddControlOnLoad(TCtrl loadedControl);

	public virtual void ControlsLoaded()
	{

	}

	public virtual string GetControlKey(TCtrl loadedControl)
	{
		return default;
	}

	public abstract T GetControl<T>() where T : class, TCtrl;
}
