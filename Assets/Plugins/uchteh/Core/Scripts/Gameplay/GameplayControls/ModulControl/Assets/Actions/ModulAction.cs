using Core.Settings;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay.Modul
{
    public enum ActionType
    {
        Simple,
        Complex
    }

    [Serializable]
    public class ModulAction
    {
        public static ModulAction GetNewModulAction<TModul, TStatus, TFuncs, TOrgans>()
        {
            return (ModulAction)Activator.CreateInstance(typeof(ModulAction<,,,>).MakeGenericType(typeof(TModul), typeof(TStatus), typeof(TFuncs), typeof(TOrgans)));
        }
    }

    [HideReferenceObjectPicker]
    public class ModulAction<TModul, TStatuses, TFuncs, TOrgans> :
        ModulAction where TModul : Modul<TModul, TStatuses, TFuncs, TOrgans>
    {
        [SerializeField, LabelText("Тип действия"), OnValueChanged("ChangeActionType")]
        private ActionType modulActionType = default;

        public ActionType ModulActionType => modulActionType;


        #region Работа с органами

        [SerializeField, LabelText("Тип органа"), ShowIf("ModulActionType", ActionType.Simple), OnValueChanged("SearhValues")]
        private TOrgans organId;

        public TOrgans OrganId { get => organId; set => organId = value; }


        [SerializeField, LabelText("Функция органа"), ShowIf("ModulActionType", ActionType.Simple), OnValueChanged("SearhValues")]
        private string organFunc;

        public string OrganFunc { get => organFunc; set => organFunc = value; }

        //[ShowIf("ModulActionType", ActionType.Simple)]
        //[SerializeField, LabelText("Точка передвижения")]
        //private float nextActionParameter;
        //public float NextActionParameter { get => nextActionParameter; set => nextActionParameter = value; }


        //[ShowIf("ModulActionType", ActionType.Simple)]
        //[SerializeField, LabelText("В анимационных координатах")]
        //private object[] IsAnimationCoord;

        #endregion

        #region Работа с модулями

        [SerializeField, LabelText("Функция модуля"), ShowIf("ModulActionType", ActionType.Complex)]
        private TFuncs modulFunc;
        public TFuncs ModulFunc
        {
            get { return modulFunc; }
            set { modulFunc = value; }
        }


        [SerializeField, LabelText("Параметры функции"), ShowIf("ModulActionType", ActionType.Complex)]
        private List<object> parameters = new List<object>();
        public object[] Parameters => parameters.ToArray();
        #endregion

        #region Работа с органами


        /// <summary>
        /// Поиск значения
        /// </summary>
#pragma warning disable 414
        private bool searchActive;
        // ReSharper disable once NotAccessedField.Local
        [SerializeField, BoxGroup("SearchResults:", false), DisplayAsString(false), HideLabel, ShowIf("searchActive")] private string searchedValues;
#pragma warning restore 414

        /// <summary>
        /// Поиск значений действия
        /// </summary>
        public void SearhValues()
        {
            searchedValues = "";
            string value;

            searchActive = true;
            value = OrganFunc;
            var variants = Messages.GetOrganFuns(ModulControl.GetCompleteModulInstance<TModul, TStatuses, TFuncs, TOrgans>().GetOrganType(OrganId));

            var chars = value.ToLowerInvariant().ToCharArray();
            List<string> findString = new List<string>();
            foreach (var variant in variants)
            {
                var valid = false;

                var s = variant.ToLowerInvariant();
                if (chars.Length == 0)
                {
                    valid = true;
                }
                else for (var index = 0; index < chars.Length; index++)
                    {
                        var mChar = chars[index];
                        var i = s.IndexOf(mChar);
                        if (i == -1)
                        {
                            break;
                        }
                        if (index + 1 == chars.Length)
                        {
                            valid = true;
                            break;
                        }
                        if (i + 1 < s.Length)
                        {
                            s = s.Substring(i + 1);
                        }
                    }

                if (valid) findString.Add(variant);
            }
            foreach (var find in findString)
            {
                searchedValues += find + "\n";
            }

            if (findString.Count == 1)
            {
                searchActive = false;
                searchedValues = findString[0] + "<=";
                value = findString[0];
                GUI.FocusControl(null);
                switch (ModulActionType)
                {
                    case ActionType.Simple:
                        OrganFunc = value;
                        break;
                }
            }
        }


        /// <summary>
        /// Смена типа действия
        /// </summary>
        private void ChangeActionType()
        {
            searchActive = false;
        }

        #endregion
    }
}