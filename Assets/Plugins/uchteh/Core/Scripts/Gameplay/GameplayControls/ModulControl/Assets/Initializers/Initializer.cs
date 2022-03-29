using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Modul
{
    [Serializable]
    public class Initializer : SerializedScriptableObject
    {
        [SerializeField]
        private InitializerData initializerData;

        public InitializerData InitializerData
        {
            get => initializerData;
            set => initializerData = value;
        }

        public Type GetModulSaddleTypeId() => InitializerData.ModulSaddleTypeId;
        public Type GetModulType() => InitializerData.ModulType;

    }

    public class InitializerData 
    {
        [SerializeField, HideInInspector]
        private Type mdulSaddleTypeId;
        [SerializeField, HideInInspector]
        private Type modulType;

        public Type ModulSaddleTypeId
        {
            get => mdulSaddleTypeId;
            protected set => mdulSaddleTypeId = value;
        }

        public Type ModulType
        {
            get => modulType;
            protected set => modulType = value;
        }


        [SerializeField,BoxGroup("Действия модуля:", false), ListDrawerSettings(HideAddButton = true), LabelText("Действия модуля")]
        private List<ModulAction> modulActions = new List<ModulAction>();
        public List<ModulAction> ModulActions => modulActions;

    }

    public class InitializerData<TModul, TStatus, TFuncs, TOrgans> : InitializerData where TModul : Modul<TModul, TStatus>
    {
        public InitializerData()
        {
            ModulSaddleTypeId = typeof(TModul).GetInterfaces()
                .First(x => x.GetInterfaces().Contains(typeof(ISettable)));
            ModulType = typeof(TModul);

        }

        [Button]
        public void TestAndUpdate ()
        {
            Debug.Log(typeof(TModul));
            ModulSaddleTypeId = typeof(TModul).GetInterfaces()
                .First(x => x.GetInterfaces().Contains(typeof(ISettable)));
            Debug.Log(ModulSaddleTypeId);

        }

        [BoxGroup("Действия модуля:", false), Button]
        public void AddModulAction()
        {
            ModulActions.Add(ModulAction.GetNewModulAction<TModul, TStatus, TFuncs, TOrgans>());       
        }
    }
}