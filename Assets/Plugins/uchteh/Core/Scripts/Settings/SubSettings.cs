using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Settings
{
	/// <summary>
	/// Настройки определенного менеджера
	/// </summary>
	/// <typeparam name="TDefault"></typeparam>
	[Serializable]
	public abstract class SubSettings<TDefault> : SubSettings where TDefault : SubSettings
	{
		/// <summary>
		/// Используется для получения дефолтного файла настроек, который хранится в SettingsStorage.Settings.SubSettings (чаще всего ThemeSettings, но могут быть и просто настройки контрола)
		/// </summary>
		/// <returns></returns>
		public static TDefault Default()
		{
			if (SettingsStorage.Settings.SubSettings.Any(s => s is TDefault))
			{
				return (TDefault)SettingsStorage.Settings.SubSettings.First(s => s is TDefault);
			}
			Debug.LogError("Не найден конфиг типа:" + typeof(TDefault));
			return null;
		}
	}

	[Serializable]
	public abstract class SubSettings : SerializedScriptableObject
	{

	}
}
