using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Загружает менеджеры. В случае если дочерний менеджер является InitableManager, то он будет проинициализирован в своём методе OnControlsLoad, но BaseControl нет. 
/// </summary>
/// <typeparam name="TCtrl"></typeparam>
public abstract class BaseManager<TCtrl> : BaseControlMethods
    where TCtrl : class, IControl
{
    /// <summary>
    /// Список объектов, с которыми работает BaseControl (загружает/отгружает, etc)
    /// </summary>
    protected Dictionary<string, TCtrl> Controls;
    
    protected virtual IReadOnlyCollection<TCtrl> GetChildControls()
    {
        return GetComponentsInChildren<TCtrl>();
    }
    
 

    public override void Load()
    {
        base.Load();

        LoadControls(GetChildControls());
        ControlsLoaded();
    }

    protected virtual void ControlsLoaded()
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

    protected virtual void AddControlOnLoad(TCtrl loadedControl)
    {
	    Controls.Add(GetControlKey(loadedControl), loadedControl);
    }

    protected virtual string GetControlKey(TCtrl loadedControl)
    {
	    return loadedControl.GetType().ToString();
    }

    public void AddControls(IReadOnlyCollection<TCtrl> controls)
    {
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
    public TCtrl GetControl(Type type) 
    {
        var result = Controls.Values.FirstOrDefault(x => x.GetType().IsAssignableFrom(type));
        if (result != null)
        {
            return result;
        }
        Debug.LogError("Не найден Control типа:" + type);
        return null;
    }


    public T GetControl<T>() where T : class, TCtrl
    {
        try
        {
            var result = (T)Controls.Values.FirstOrDefault(x => x is T);
            return result;
        }
        catch (Exception)
        {
            Debug.LogError("Не найден Control типа:" + typeof(T));
            return null;
        }       
    }
}