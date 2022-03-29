using System;
using System.Collections.Generic;
using Core.Gameplay.Modul;
using Core.Settings;
using Example.ExampleSubModuls;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

//1. Необходимо найти или создать пространство в котором будут содержаться все необходимые для модуля органы  и интерфейсы
namespace Example
{
    //                                                                                                     Id так как это интерфейс для всех модулей способных выполнять подобные функции
    //                                                                                                      V 
    //2. Если модуль можно будет заменить на другой подобный, то необходимо создать интерфейс IExampleModulId, а также описать его; 
    //                                                           Необходимо пронаследовать его от интерфейса ISettable,
    //                                                           что бы указать что возможна замена модуля в гнезде типа IExampleModulId
    //* В отдельном файле создать гнездо котором смогут содержаться модули такого типа
    /// <summary>
    /// Тип гнезда, которое подходит для модулей из этого пространства имен.  Указывает что на сцене может быть несколько разных устройств, вставляемых в гнездо такого типа
    /// </summary>
    public interface IExampleModulId : ISettable
    {
    }

    //3. Необходимо создать уникальный интерфейс IExampleModul обозначающий тип модуля, а также описать его;
    /// <summary>
    /// Образец модуля
    /// </summary>
    public interface IExampleModul : IGameplayModul
    {
    }

    //4. Необходимо создать список квестовых состояний (свойств) модуля, по возможности описав каждый;
    //   Здесь должны содержаться  только те свойства модуля, которые необходимы системе выполнения квестов,
    //   остальные свойства должны быть расположены в специальном регионе в теле класса модуля
    /// <summary>
    /// Список проверяемых состояний
    /// </summary>
    public enum Statuses
    {
        /// <summary>
        /// Пример булевого состояния
        /// </summary>
        ExampleBoolStatus1,

        /// <summary>
        /// Пример целочисленного состояния
        /// </summary>
        ExampleIntStatus2,

        /// <summary>
        /// Пример вещественного состояния
        /// </summary>
        ExampleFloatStatus3,

        /// <summary>
        /// Пример строкового состояния
        /// </summary>
        ExampleStringStatus4,

        /// <summary>
        /// Пример перечислимого состояния
        /// </summary>
        ExampleEnumStatus5
    }

    //5. Необходимо создать список функций, по возможности описав каждую;
    //   Здесь должны содержаться только сложные (Complex) функции,
    //   которые управляют поведением нескольких органов либо вызыывает функции зависимых(вложенных) подмодулей,
    //   остальные (Simple) функции должны быть расположены в органах
    /// <summary>
    /// Список функций
    /// </summary>
    public enum Funcs
    {
        /// <summary>
        /// Пример первой функции
        /// </summary>
        ExampleModulFunc1,

        /// <summary>
        /// Пример второй функции
        /// </summary>  
        ExampleModulFunc2,

        /// <summary>
        /// Пример третьей функции
        /// </summary>
        ExampleModulFunc3
    }

    //6. Необходимо создать список органов, по возможности описав каждый;
    //   Здесь должен содержаться набор уникальных индетификаторов органов, доступых этому модулю для управления 
    /// <summary>
    /// Список органов IExampleModul
    /// </summary>
    public enum Organs
    {
        /// <summary>
        ///Имя первого органа 
        /// </summary>
        ExampleModulOrgan1,

        /// <summary>
        ///Имя второго органа 
        /// </summary>
        ExampleModulOrgan2,

        /// <summary>
        ///Имя третьего органа 
        /// </summary>
        ExampleModulOrgan3
    }

    //*  Для свойств которые имеют несколько определенных состояний необходимо создать списки состояний, по возможности описав каждое
    /// <summary>
    /// Дополнительный список
    /// </summary>
    internal enum ExampleStatus
    {
        /// <summary>
        /// Первое состояние
        /// </summary>
        ExampleEnumStatus5One,

        /// <summary>
        ///  Второе состояние
        /// </summary>
        ExampleEnumStatus5Two,

        /// <summary>
        ///  Третье состояние
        /// </summary>
        ExampleEnumStatus5Tree
    }
    /// <summary>
    /// Дополнительный список 2
    /// </summary>
    internal enum ExampleStatus2
    {
        /// <summary>
        /// Первое состояние
        /// </summary>
        ExampleEnumOne,

        /// <summary>
        ///  Второе состояние
        /// </summary>
        ExampleEnumTwo,

        /// <summary>
        ///  Третье состояние
        /// </summary>
        ExampleEnumThree
    }

    //7. Необходимо создать закрытый класс модуля и пронаследовать его уникальным интерфейсом модуля, а также снабдить описанием
    /// <summary>
    /// Образец модуля
    /// </summary>
    public sealed class ExampleModul : Modul<ExampleModul, Statuses, Funcs, Organs>, IExampleModul,IExampleModulId
    //	   ^			^			             ^			 ^		   ^	  ^        ^
    //     |            |			             |          |         |      |        интерфейс модуля
    //     |	      	-------------------------           |         |      список органов
    //     |		             |                          |         список функций
    //     закрытый 		     класс модуля	     список квестовых состояний
    {
        [SerializeField] private IExampleModulId saddle1 = null;
        public IExampleModulId Saddle1 => saddle1;

        [SerializeField] private IExampleSubModulsId saddle2 = null;
        public IExampleSubModulsId Saddle2 => saddle2;
        //8. Для свойств, если они есть, необходимо создать  регион 
        //   Причем для каждого квестового состояния должно быть свое отдельное свойство, но свойств может быть больше чем квестовых состояний

        #region Свойства

        /// <summary>
        /// Свойство булевого квестового состояния
        /// </summary>
        private bool ExampleBoolStatus1
        {
            get => SpecialStatus.GetStatusField(Statuses.ExampleBoolStatus1).Value;
            set => SpecialStatus.GetStatusField(Statuses.ExampleBoolStatus1).Value = value;
        }

        /// <summary>
        /// Свойство булевого квестового состояния
        /// </summary>
        private int ExampleIntStatus2
        {
            get => SpecialStatus.GetStatusField(Statuses.ExampleIntStatus2).Value;
            set => SpecialStatus.GetStatusField(Statuses.ExampleIntStatus2).Value = value;
        }

        /// <summary>
        /// Свойство вещественного квестового состояния
        /// </summary>
        private float ExampleFloatStatus3
        {
            get => SpecialStatus.GetStatusField(Statuses.ExampleFloatStatus3).Value;
            set => SpecialStatus.GetStatusField(Statuses.ExampleFloatStatus3).Value = value;
        }

        /// <summary>
        /// Свойство строкового квестового состояния
        /// </summary>
        private string ExampleStringStatus4
        {
            get => SpecialStatus.GetStatusField(Statuses.ExampleStringStatus4).Value;
            set => SpecialStatus.GetStatusField(Statuses.ExampleStringStatus4).Value = value;
        }

        /// <summary>
        /// Свойство перечислимого квестового состояния
        /// </summary>
        private ExampleStatus ExampleEnumStatus5
        {
            get => SpecialStatus.GetStatusField(Statuses.ExampleEnumStatus5).Value;
            set => SpecialStatus.GetStatusField(Statuses.ExampleEnumStatus5).Value = value;
        }

        [SerializeField, HideLabel, FoldoutGroup("Состояния"), LabelText("ExampleBool")] private bool exampleBool;
        /// <summary>
        /// Свойство булевого состояния
        /// </summary>
        /// 
        private bool ExampleBool
        {
            get => exampleBool;
            set => exampleBool = value;
        }

        [SerializeField, HideLabel, FoldoutGroup("Состояния"), LabelText("ExampleInt")] private int exampleInt;
        /// <summary>
        /// Свойство доступа целочисленного состояния
        /// </summary>
        private int ExampleInt
        {
            get => exampleInt;
            set => exampleInt = value;
        }

        [SerializeField, HideLabel, FoldoutGroup("Состояния"), LabelText("ExampleFloat")] private float exampleFloat;
        /// <summary>
        /// Свойство вещественного состояния
        /// </summary>
        private float ExampleFloat
        {
            get => exampleFloat;
            set => exampleFloat = value;
        }

        [SerializeField, HideLabel, FoldoutGroup("Состояния"), LabelText("ExampleString")] private string exampleString;
        /// <summary>
        /// Свойство строкового состояния
        /// </summary>
        private string ExampleString
        {
            get => exampleString;
            set => exampleString = value;
        }

        [SerializeField, HideLabel, FoldoutGroup("Состояния"), LabelText("ExampleEnum")] private ExampleStatus2 exampleEnum;
        /// <summary>
        /// Свойство перечислимого состояния 
        /// </summary>
        private ExampleStatus2 ExampleEnum
        {
            get => exampleEnum;
            set => exampleEnum = value;
        }

        #endregion

        //9. Необходимо создать словарь квестовых состояний модуля и убедиться, что подсказки состояний достаточно адекватны для русскоговорящего человека;
        //   При создании статусов обратите внимание, что указанное здесь значение будет отображаться в инспекторе до запуска приложения;
        //   Правильно сформулировать описание в зависимости от значения действительно важно, так как они будут отображены конечному пользователю
        /// <summary>
        /// Словарь квестовых состояний
        /// </summary>
        public override SortedDictionary<Statuses, StatusField<Statuses>> StartStatusData =>
            new SortedDictionary<Statuses, StatusField<Statuses>>
            {
                {
                    Statuses.ExampleBoolStatus1,                                                                                                                     //============================
                    new StatusField<ExampleModul, Statuses, bool>(Statuses.ExampleBoolStatus1,  false,                                                              //  Это подсказки пользователю 
                        "Включите ExampleBoolStatus1 ","Выключите ExampleBoolStatus1")//<==========================================================================//
                },                                                                                                                                                //    
                {                                                                                                                                                //
                                                                                                                                                                //         Их текст может быть  
                    Statuses.ExampleIntStatus2,                                                                                                                //         изменен  в инспекторе    
                    new StatusField<ExampleModul, Statuses, int>(Statuses.ExampleIntStatus2, 20,                                                              //          при создании квеста,      
                        "Установите значение состояния ExampleIntStatus2 равным ") //<=======================================================================//  но рекомендуется продумать его здесь         
                },                                                                                                                                          //             
                {                                                                                                                                          //
                                                                                                                                                          //     
                    Statuses.ExampleFloatStatus3,                                                                                                        //      
                    new StatusField<ExampleModul, Statuses, float>(Statuses.ExampleFloatStatus3, 10f,                                                   //       
                        "Установите значение состояния ExampleFloatStatus3 равным") //<================================================================//      
                },                                                                                                                                    //
                {                                                                                                                                    //
                                                                                                                                                    //
                    Statuses.ExampleStringStatus4,                                                                                                 //
                    new StatusField<ExampleModul, Statuses, string>(Statuses.ExampleStringStatus4, "ExampleStringStatus4Content",                 //
                        "Установите значение состояния ExampleStringStatus4Content равным") //<==================================================//
                },                                                                                                                              //
                {                                                                                                                              //
                                                                                                                                              //
                    Statuses.ExampleEnumStatus5,                                                                                             //
                    new StatusField<ExampleModul, Statuses, ExampleStatus>(Statuses.ExampleEnumStatus5,ExampleStatus.ExampleEnumStatus5One, //
                        "Установите значение ExampleStringStatus4Content равным") //<======================================================//
                }
            };

      

        public override Dictionary<string, Type> GetChildSaddles()
        {
            return new Dictionary<string, Type>
            {
                {"ExampleModulIdSaddle1Id",typeof(IExampleModulId)},
                {"ExampleModulIdSaddle1Id2",typeof(IExampleModulId)},
                {"ExampleSubModulsIdSaddle2Id",typeof(IExampleSubModulsId)},
                {"ExampleSubModulsIdSaddle2Id2", typeof(IExampleSubModulsId)}
            };
        }

        public override OrganType GetOrganType(Organs id)
        {
            switch (id)
            {
                case Organs.ExampleModulOrgan1: return OrganType.Common;
                case Organs.ExampleModulOrgan2: return OrganType.Common | OrganType.Axis;
                case Organs.ExampleModulOrgan3: return OrganType.OpenClose | OrganType.Common;
                default: return OrganType.All;
            }
        }

        [Button]
        private void ExampleTest()
        {
            ExampleBoolStatus1 = true;
            ExampleIntStatus2 = 10;
            ExampleFloatStatus3 = 0.1f;
            ExampleStringStatus4 = "asdfgh";
            ExampleEnumStatus5 = ExampleStatus.ExampleEnumStatus5One;
        }

        protected override void DoFunc(Funcs func, params object[] parameters)
        {
            switch (func)
            {
                case Funcs.ExampleModulFunc1:
                    GetOrgan(Organs.ExampleModulOrgan1).Notify(new Message(Messages.Organ.Common.Check));
                    break;

                //Пример вызова функции с параметрами в органе
                case Funcs.ExampleModulFunc2:
                    float parameter1 = 0.5f;
                    float parameter2 = 0.5f;
                    GetOrgan(Organs.ExampleModulOrgan2).Notify(new Message(Messages.Organ.Common.SendParameters, parameter1, parameter2));
                    break;

                //Пример вызова функции в модульконтроле
                case Funcs.ExampleModulFunc3:
                    Send(typeof(IModulControl), new Message(Messages.ModulControl.ModulsCheck, null));
                    break;
            }
        }


        public override void Notify(Message message)
        {
            //в базовом реализации метода notify все сообщеия помеченные типом Messages.ModulControl.ModulFunction будут отправлены в DoFunc(...)
            base.Notify(message);
            switch (message.Type)
            {
                case Messages.ModulControl.ModulsCheck:
                   
                    break;

            }
        }

        public void MoveX(InputAction.CallbackContext x)
        {
            transform.Translate((Vector3.forward * x.ReadValue<float>()) / 10);
        }

        public void MoveY(InputAction.CallbackContext y)
        {
            transform.Translate((Vector3.right * y.ReadValue<float>()) / 10);
        }
        public void ExampleTest(InputAction.CallbackContext x)
        {
            ExampleTest();
        }


        protected override void OnItemsInitialised()
        {

        }
    }


    //1. 
    namespace ExampleSubModuls
    {
        //2.
        /// <summary>
        /// Тип гнезда, которое подходит для подмодулей IExampleSubModul1 и IExampleSubModul2 из этого пространства имен.
        /// </summary>
        public interface IExampleSubModulsId : ISettable
        {
        }

        //* Общий функционал вариаций необходимо объявлять в обобщающем интерфейсе 
        public interface ISubModul : IGameplayModul
        {
            bool CommonQuestStatus { get; }
            bool CommonProperty { get; }
        }

        //* Рекомендуется создавать пространство имен для каждого модуля или подмодуля для большего порядка
        namespace ExampleSubModul1
        {
            //3.
            /// <summary>
            /// Первая вариация подмодуля
            /// </summary>
            public interface IExampleSubModul1 : ISubModul
            {
            }

            //4.
            /// <summary>
            /// Список функций подмодуля
            /// </summary>
            public enum Organs
            {
                SubModul1Organ
            }

            //5.
            /// <summary>
            /// Список функций подмодуля
            /// </summary>
            public enum Funcs
            {
                SubModul1Func1,
                SubModul1Func2
            }

            //6. 
            /// <summary>
            /// Список проверяемых состояний подмодуля
            /// </summary>
            public enum Statuses
            {
                CommonQuestStatus
            }


            //7. 
            /// <summary>
            /// Образец подмодуля первая вариация
            /// </summary>
            public sealed class ExampleSubModul1 : Modul<ExampleSubModul1, Statuses, Funcs, Organs>, IExampleSubModul1,IExampleSubModulsId
            {

                //8.
                #region Свойства
                /// <summary>
                /// Квестововое состояние подмодуля из обобщающего интерфейса
                /// </summary>
                public bool CommonQuestStatus
                {
                    get => SpecialStatus.GetStatusField(Statuses.CommonQuestStatus).Value;
                    set => SpecialStatus.GetStatusField(Statuses.CommonQuestStatus).Value = value;
                }

                [SerializeField, BoxGroup("CommonProperty")] private bool commonProperty;
                /// <summary>
                /// Состояние подмодуля из обобщающего интерфейса
                /// </summary>
                public bool CommonProperty
                {
                    get => commonProperty;
                    set => commonProperty = value;
                }

                [SerializeField, BoxGroup("Property")] private bool property;
                /// <summary>
                /// Состояние подмодуля 
                /// </summary>
                public bool Property
                {
                    get => property;
                    set => property = value;
                }
                #endregion

                //9.
                public override SortedDictionary<Statuses, StatusField<Statuses>> StartStatusData =>
                    new SortedDictionary<Statuses, StatusField<Statuses>>
                    {
                        {
                            Statuses.CommonQuestStatus,
                            new StatusField<ExampleSubModul1, Statuses, bool>(Statuses.CommonQuestStatus, false,
                                "Включите CommonQuestStatus ", "Выключите CommonQuestStatus")
                        }
                    };


                protected override void OnItemsInitialised()
                {
                    Debug.Log("Тут можно сделать какую-нибудь штуку с органами");
                }

                public override Dictionary<string, Type> GetChildSaddles()
                {
                    return null;
                }

                public override OrganType GetOrganType(Organs id)
                {
                    return OrganType.All;
                }

                protected override void DoFunc(Funcs func, params object[] parameters)
                {

                }
            }
        }

        //* Рекомендуется создавать пространство имен для каждого модуля или подмодуля для большего порядка
        namespace ExampleSubModul2
        {
            //3.
            /// <summary>
            /// Вторая вариация подмодуля
            /// </summary>
            public interface IExampleSubModul2 : ISubModul, IExampleSubModulsId
            {
            }

            //4.
            /// <summary>
            /// Список функций подмодуля
            /// </summary>
            public enum Organs
            {
                SubModul1Organ
            }

            //5.
            /// <summary>
            /// Список функций подмодуля
            /// </summary>
            public enum Funcs
            {
                SubModul1Func1,
                SubModul1Func2
            }

            //6. 
            /// <summary>
            /// Список проверяемых состояний подмодуля
            /// </summary>
            public enum Statuses
            {
                CommonQuestStatus
            }


            //7. 
            /// <summary>
            /// Образец подмодуля вторая вариация
            /// </summary>
            public sealed class ExampleSubModul2 : Modul<ExampleSubModul2, Statuses, Funcs, Organs>, IExampleSubModul2
            {

                //8.
                #region Свойства
                /// <summary>
                /// Квестововое состояние подмодуля из обобщающего интерфейса
                /// </summary>
                public bool CommonQuestStatus
                {
                    get => SpecialStatus.GetStatusField(Statuses.CommonQuestStatus).Value;
                    set => SpecialStatus.GetStatusField(Statuses.CommonQuestStatus).Value = value;
                }

                [SerializeField, BoxGroup("CommonProperty")] private bool commonProperty;
                /// <summary>
                /// Состояние подмодуля из обобщающего интерфейса
                /// </summary>
                public bool CommonProperty
                {
                    get => commonProperty;
                    set => commonProperty = value;
                }

                [SerializeField, BoxGroup("Property")] private bool property;
                /// <summary>
                /// Состояние подмодуля 
                /// </summary>
                public bool Property
                {
                    get => property;
                    set => property = value;
                }
                #endregion

                //9.
                public override SortedDictionary<Statuses, StatusField<Statuses>> StartStatusData =>
                    new SortedDictionary<Statuses, StatusField<Statuses>>
                    {
                        {
                            Statuses.CommonQuestStatus,
                            new StatusField<ExampleSubModul2, Statuses, bool>(Statuses.CommonQuestStatus, false,
                                "Включите CommonQuestStatus ", "Выключите CommonQuestStatus")
                        }
                    };

                public override Dictionary<string, Type> GetChildSaddles()
                {
                    return null;
                }

                protected override void OnItemsInitialised()
                {
                    Debug.Log("Тут можно сделать какую-нибудь штуку с органами");
                }

                public override OrganType GetOrganType(Organs id)
                {
                    return OrganType.All;
                }

                protected override void DoFunc(Funcs func, params object[] parameters)
                {

                }
            }
        }
    }
}