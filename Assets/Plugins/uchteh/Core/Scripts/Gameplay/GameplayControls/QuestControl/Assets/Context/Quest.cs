using System;
using System.Collections.Generic;
using System.Linq;
using Core.Gameplay.Modul;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Questing
{
	[Serializable]
    public class Quest : SerializedScriptableObject
    {
        public event Action<Quest> OnComplete;

        [SerializeField]
        private QuestData questData;

        public QuestData QuestData
        {
            get => questData;
            set => questData = value;
        }

        public Type GetModulSaddleType() => QuestData.ModulSaddleType;


        /// <summary>
        /// Новый квест
        /// </summary>
        public void Start()
        {
            StartBranch(0);
        }

        /// <summary>
        /// Закончить  квест
        /// </summary>
        public void Complete()
        {
            OnComplete?.Invoke(this);
        }

        /// <summary>
        /// Добавить новую ветку
        /// </summary>
        /// <param name="branch"></param>
        public void AddBranch(Branch branch)
        {
            QuestData.BranchList.Add(branch);

            if (QuestData.BranchList.Count == 1)
            {
                StartBranch(0);
            }
        }

        /// <summary>
        /// Удалить ветку
        /// </summary>
        /// <param name="branch"></param>
        public void DeleteBranch(Branch branch)
        {
            QuestData.BranchList.Remove(branch);

            // Если есть предыдущие или следующие ветки, то выбираем их
            if (QuestData.BranchList.Any())
            {
                if (HasPrevBranch())
                {
                    PrevBranch();
                }
                else
                {
                    QuestData.CurrentBranchIndex--;
                    NextBranch();
                }
            }
            // Иначе если нет других веток, то устанавливаем ветку и шаг null
            else
            {
                QuestData.CurrentBranch = null;
            }
        }

        /// <summary>
        /// Выбрать ветку 
        /// </summary>
        /// <param name="newBranch"></param>
        public void StartBranch(int newBranch)
        {
            if (QuestData.BranchList != null)
            {
                QuestData.CurrentBranch = QuestData.BranchList.Any() ? QuestData.BranchList[newBranch] : null;
            }
        }

        /// <summary>
        /// Переходить на другую ветку
        /// </summary>
        /// <param name="branchNumber"></param>
        public void ChangeBranch(int branchNumber)
        {
            QuestData.CurrentBranch = QuestData.BranchList[branchNumber];
        }

        /// <summary>
        /// Перейти на предыдущую ветку
        /// </summary>
        public void PrevBranch()
        {
            int prevBranch = QuestData.CurrentBranchIndex - 1;
            ChangeBranch(prevBranch);
        }

        /// <summary>
        /// Перейти на следующую ветку
        /// </summary>
        public void NextBranch()
        {
            int nextBranch = QuestData.CurrentBranchIndex + 1;

            ChangeBranch(nextBranch);
        }

        /// <summary>
        /// Проверить предыдущую ветку
        /// </summary>
        /// <returns></returns>
        public bool HasPrevBranch()
        {
            bool result = QuestData.CurrentBranchIndex != 0;
            return result;
        }

        /// <summary>
        /// Проверить следующую ветку
        /// </summary>
        /// <returns></returns>
        public bool HasNextBranch()
        {
            bool result = QuestData.CurrentBranchIndex != QuestData.BranchList.Count - 1;
            return result;
        }
    }


    public class QuestData
    {
        [LabelText("Название квеста")]
        public string Name;

        [LabelText("Описание квеста"), TextArea(1, 5)]
        public string Description;

        [BoxGroup("Ветки квеста:", false), LabelText("Ветки квеста")]
        [SerializeField, ListDrawerSettings(HideAddButton = true, ShowIndexLabels = true, ListElementLabelName = "Name")]
        private  List<Branch> branchList = new List<Branch>();
        public List<Branch> BranchList => branchList;


        [SerializeField, HideInInspector]
        private Type modulSaddleType;

        public Type ModulSaddleType
        {
            get => modulSaddleType;
            protected set => modulSaddleType = value;
        }


        private int curBranch;

        public int CurrentBranchIndex
        {
            get => curBranch;
            protected internal set => curBranch = value;
        }

        public Branch CurrentBranch
        {
            get => BranchList[curBranch];
            set
            {
                if (value == null)
                {
                    curBranch = 0;
                }
                curBranch = BranchList.IndexOf(value);
            }
        }

    }

    public class QuestData<TModul, TStatus> : QuestData where TModul : Modul<TModul, TStatus>
    {

        private Type GetModulSaddleType
        {
            get
            {
                return typeof(TModul).GetInterfaces().First(x => x.GetInterfaces().Contains(typeof(ISettable)));
            }
        }

        [Button]
        public void TestAndUpdate()
        {
            Debug.Log(typeof(TModul));
            ModulSaddleType = typeof(TModul).GetInterfaces()
                .First(x => x.GetInterfaces().Contains(typeof(ISettable)));
            Debug.Log(ModulSaddleType);

        }

        public QuestData()
        {
            ModulSaddleType = GetModulSaddleType;
        }

        [BoxGroup("Ветки квеста:", false), Button, LabelText("Добавить ветку")]
        public void AddBranch()
        {
            BranchList.Add(Branch.GetNewBranch<TModul, TStatus>());
            Debug.Log(BranchList.Last().KeyStatus.IsPart);
        }
    }
}