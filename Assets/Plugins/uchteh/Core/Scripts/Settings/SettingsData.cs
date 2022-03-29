using System;
using System.Collections.Generic;
using System.Linq;
using Core.Gameplay.Questing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Settings
{
	[Serializable]
	[CreateAssetMenu(fileName = "New SettingsData", menuName = "Settings/SettingsData")]
	public class SettingsData : SerializedScriptableObject
	{
		[Header("Конфиги")]
		public List<SubSettings> SubSettings = new List<SubSettings>();

	    [Header("Режим отладки приложения")]
        public bool Debuging;

	    [SerializeField] 
	    private string documentryFolderPath = null;
	    public string DocumentryFolderPath => documentryFolderPath;

        [SerializeField] 
        private QuestSettings debugQuestSettings = null;
		public QuestSettings DebugQuestSettings => debugQuestSettings;


        [SerializeField] 
        private Gradient preselectHighlightColor = null;
		public Gradient PreselectHighlightColor => preselectHighlightColor;


		[SerializeField] 
        private Gradient fixSelectHighlightColor = null;
		public Gradient FixSelectHighlightColor => fixSelectHighlightColor;


		[SerializeField] 
        private Gradient focusInsideHighlightColor = null;
		public Gradient FocusInsideHighlightColor => focusInsideHighlightColor;


		[SerializeField] private Material blinkMaterial = null;
		public Material BlinkMaterial => blinkMaterial;


        /// <summary>
        /// Ищет конфиг типа Т в SettingStorage.Settings.SubSettings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSubSettings<T>() where T : SubSettings
		{
			if (SubSettings.Any(s => s is T))
			{
				return (T)SubSettings.First(s => s is T);

			}
		    Debug.LogError("Не найден конфиг типа:" + typeof(T));
		    return null;
		}
	}
}


