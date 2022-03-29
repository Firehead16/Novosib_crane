using System;
using System.Collections.Generic;
using Core.Gameplay.Modul;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Questing
{
	[Serializable]
    [CreateAssetMenu(fileName = "QuestSettings", menuName = "Settings/QuestSettings")]
    public class QuestSettings : SerializedScriptableObject
    {
        [InfoBox("<color=#ffffff>███</color> Последовательные квесты,  \n<color=#b0b092>███</color> параллельные квесты , \n<color=#c5b294>███</color> выполнять все действия в параллельном ")]
        [Multiline]
        public string Name;

        [Multiline]
        public string Description;


        /// <summary>
        /// Инициализаторы
        /// </summary>
        [SerializeField]
        public List<InitializerIdentificator> Initializers;

        /// <summary>
        /// Список квестов
        /// </summary>
        [SerializeField]
        public ICompletableCollectionItem QuestLine;

		/// <summary>
		/// Нужно проходить с записью результатов
		/// </summary>
		public bool IsExam { get; set; }

        /// <summary>
        ///Режим без проверки статусов
        /// </summary>
        public bool IsNeedCheckStatuses { get; set; }

       
        private void OnDestroy()
        {
            QuestLine.Reset();
        }
    }
}