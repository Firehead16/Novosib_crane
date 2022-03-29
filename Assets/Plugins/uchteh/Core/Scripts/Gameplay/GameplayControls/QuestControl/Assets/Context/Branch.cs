using System;
using Core.Gameplay.Modul;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Questing
{
    /// <summary>
    /// Хранилище списков требуемых для прохождения и ошибочных состояний модуля для каждого задания
    /// Квест последовательно проходит по веткам, пока не будет завершен
    /// </summary>
    public class Branch
    {
        [LabelText("Название ветки")]
        [SerializeField]
        private string name;

        public string Name => name;


        [SerializeField, LabelText("Описание"), TextArea(1, 5)]
        private string description;

        public string Description => description;


        [SerializeField, HideReferenceObjectPicker, LabelText("Ошибочные состояния:")]
        private ModulStatus errorStatus;

        public ModulStatus ErrorStatus => errorStatus;

   
        [SerializeField, HideReferenceObjectPicker, LabelText("Ключевые состояния:")]
        private ModulStatus keyStatus;

        public ModulStatus KeyStatus => keyStatus;


        [SerializeField, HideReferenceObjectPicker, LabelText("Проверяемые состояния после ключевого состояния")]
        private ModulStatus outStatus;

        public ModulStatus OutStatus => outStatus;


    
        public static Branch GetNewBranch<TModul,TStatus>() where TModul :Modul<TModul,TStatus>
        {
            Branch branch = new Branch
            {
                name = "",
                description = "",
                errorStatus = (ModulStatus)Activator.CreateInstance(typeof(ModulStatus<,>).MakeGenericType(typeof(TModul), typeof(TStatus))),
                keyStatus = (ModulStatus)Activator.CreateInstance(typeof(ModulStatus<,>).MakeGenericType(typeof(TModul), typeof(TStatus))),
                outStatus = (ModulStatus)Activator.CreateInstance(typeof(ModulStatus<,>).MakeGenericType(typeof(TModul), typeof(TStatus)))

            };

            branch.errorStatus.LoadEmpty(true);
            branch.keyStatus.LoadEmpty(true);
            branch.outStatus.LoadEmpty(true);

           return branch;
        }
    }
}
