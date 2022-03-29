using System;
using System.Collections.Generic;
using Core.Settings;
using Core.Ui;
using UnityEngine;

public enum Mode
{
	Main,
	Exam,
}


/// <summary>
/// Хранилище для настроек
/// </summary>
public static class SettingsStorage
{
	private static SettingsData settings;

    /// <summary>
	/// Файл настроек, настраивается в инспекторе. Инициализируется сразу включении
	/// </summary>
	public static SettingsData Settings
    {
        get
        {
            return settings;
        }
        set
		{
			settings = value;
			Debug.Log("Настройки установлены"); 
		}
    }

    public static ThemeSettings ThemeSettings;

    public static Dictionary<Type, Message> ScenePrepareMessages = new Dictionary<Type, Message>();
    
    public static Mode Mode = Mode.Main;
}