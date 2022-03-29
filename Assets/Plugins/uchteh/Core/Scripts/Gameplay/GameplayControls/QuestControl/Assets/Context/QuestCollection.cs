using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Questing
{
   
    // todo не поддерживает повторение квестов
    /// <summary>
    /// Класс для работы с несколькими квестами одновременно
    /// </summary>
    [Serializable]
    public class QuestCollection : ICompletableCollectionItem
    {
        [HideIf("label", "")]
#pragma warning disable 414
        private string label;

        [ToggleLeft, LabelText("Выполнять параллельно"), HorizontalGroup("CollParameters"), OnValueChanged("OnCollectionChange")]
        public bool IsParallel; //выполнять параллельно 

        /// <summary>
        /// Необходимо выполнить все 
        /// </summary>
        [ToggleLeft, LabelText("Выполнить все"), HorizontalGroup("CollParameters"), ShowIf("IsParallel"), OnValueChanged("OnCollectionChange")]
        public bool NeedCompleteAll;

        [HideLabel, HideReferenceObjectPicker, ShowInInspector, ListDrawerSettings(ShowIndexLabels = true), InfoBox("$label", VisibleIf = "showLabel", InfoMessageType = InfoMessageType.None), OnValueChanged("OnCollectionChange"), GUIColor("GetColor")]
        public List<ICompletableCollectionItem> Collection;

        private bool showLabel;
#pragma warning restore 414


        private Color GetColor()
        {
            var result = Color.white;

            if (IsParallel)
            {
                result = new Color(0.9f, 0.9f, 0.75f);
                if (NeedCompleteAll) result += new Color(0.5f, 0f, 0f);
            }
            return result;
        }

        private void OnCollectionChange()
        {
            if (Collection.Any())
            {
                showLabel = true;
                if (IsParallel)
                {

                    label = "Нужно выполнить одно из ";
                    if (NeedCompleteAll) label = "Нужно выполнить в любом порядке ";
                }
                else
                {
                    label = "Последовательно выполнить";
                }
            }
            else

                showLabel = false;

        }

        /// <summary>
        /// Проверка на выполнение квестов
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            if (IsParallel)
            {
                if (NeedCompleteAll)
                {
                    return !HasOneUncomplited();//если выполнены все
                }
                return HasOneComplited(); //если один выполнен
            }
            return Collection.Last().IsCompleted(); //если последний выполнен
        }

        /// <summary>
        /// Есть ли один незавершенный квест
        /// </summary>
        /// <returns></returns>
        private bool HasOneUncomplited()
        {
            return Collection.Exists(c => !c.IsCompleted());
        }

        /// <summary>
        /// Есть ли один завершенный квест
        /// </summary>
        /// <returns></returns>
        private bool HasOneComplited()
        {
            return Collection.Exists(c => c.IsCompleted());
        }

        /// <summary>
        /// Получить активные квесты
        /// </summary>
        /// <returns></returns>
        public List<ICompletableCollectionItem> GetActiveItems()
        {
            if (IsParallel)
            {
                var result = new List<ICompletableCollectionItem>();

                foreach (var item in Collection)
                {
                    if (item.IsCompleted()) continue;
                    result.AddRange(item.GetActiveItems());
                }
                return result;
            }
            var notCompletedCollection = Collection.FirstOrDefault(collectionItem => collectionItem.IsCompleted() == false);
            return notCompletedCollection?.GetActiveItems();
        }

        /// <summary>
        /// Получить незавершенные квесты 
        /// </summary>
        /// <returns></returns>
        public List<ICompletableCollectionItem> GetUncompletedItems()
        {
            var result = new List<ICompletableCollectionItem>();

            foreach (var item in Collection)
            {
                result.AddRange(item.GetUncompletedItems());
            }

            return result;
        }

        /// <summary>
        /// Сбросить состояние
        /// </summary>
        public void Reset()
        {
            foreach (var item in Collection)
            {
                item.Reset();
            }
        }
    }
}