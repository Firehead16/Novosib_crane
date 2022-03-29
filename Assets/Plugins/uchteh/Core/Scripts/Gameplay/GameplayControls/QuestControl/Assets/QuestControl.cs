using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Gameplay.Modul;
using Core.Gameplay.Reporting;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum QuestControl
		{
			StartWork,
			FinishWork,
			StartQuests,
			UpdateQuests,
			CompleteQuests,
			ChangeQuest,
			PrepareQuest,
			AbortQuests,
			WarningQuest,
		}
	}
}

namespace Core.Gameplay.Questing
{
	/// <summary>
	/// Надстройка над ModulControl, позволяет выполнять задания сформированые из последоваительности наборов требуемых квестовых сотояний Контроллер квестов. 
	/// </summary>
	public class QuestControl : BaseControlMethods, IQuestControl
	{
		/// <summary>
		/// Все полученные и выполняемые квесты
		/// </summary>
		[ShowInInspector, ReadOnly]
		private static List<QuestSettings> accessQuestSettings;

		[SerializeField]
		private QuestSettings questSettings;

		[SerializeField]
		private List<QuestIdentificator> activeQuests;


		private bool isCompleteWork;

		public bool IsInitialized { get; set; }

		public override void Initialize()
		{

		}

		public override void Unload()
		{
			ResetQuests();
		}

		public override void Notify(Message message)
		{
			base.Notify(message);

			switch (message.Type)
			{
				case Messages.Application.DebugStartScene:
					questSettings = SettingsStorage.Settings.DebugQuestSettings;
					break;
				case Messages.QuestControl.StartWork:

					accessQuestSettings = (List<QuestSettings>)message.Content[0];

					string reportName = $"Задание от {DateTime.Now} :" + "\n";

					for (var index = 0; index < accessQuestSettings.Count; index++)
					{
						var accessQuestSetting = accessQuestSettings[index];
						reportName += (index + 1) + ": " + accessQuestSetting.Name + "\n";
					}

					Send(typeof(IReport), new Message(Messages.Report.StartReport, reportName));

					Notify(new Message(Messages.QuestControl.ChangeQuest, accessQuestSettings[0]));
					Notify(new Message(Messages.QuestControl.PrepareQuest));
					Notify(new Message(Messages.QuestControl.StartQuests));
					break;
				case Messages.QuestControl.FinishWork:
					Send(typeof(IReport), new Message(Messages.Report.FinishReport));
					Send(typeof(IGameplayInterface), new Message(Messages.QuestControl.FinishWork));
					break;
				case Messages.QuestControl.ChangeQuest:
					questSettings = message.Content[0] as QuestSettings;

					if (questSettings == null)
					{
						this.DebugLogError("Не установлены настройки для квеста");
					}
					else
					{
						if (SettingsStorage.Mode == Mode.Exam)
						{
							questSettings.IsNeedCheckStatuses = true;
							Send(typeof(IGameplayInterface), new Message(Messages.QuestControl.ChangeQuest, questSettings));
						}
					}
					break;
				case Messages.QuestControl.PrepareQuest:
					Send(typeof(IModulControl), new Message(Messages.ModulControl.DoInitializers, questSettings.Initializers));
					break;
				case Messages.QuestControl.StartQuests:
					UpdateActiveQuests();

					if (activeQuests != null)
					{
						foreach (var activeQuest in activeQuests)
						{
							StartQuest(activeQuest);
						}

					}

					UpdateQuestsState();
					break;

				case Messages.QuestControl.AbortQuests:
					AbortQuests();
					break;
				case Messages.QuestControl.CompleteQuests:

					break;
				case Messages.QuestControl.UpdateQuests:
					UpdateQuestsState();
					break;
				case Messages.ModulControl.ChangeStatus:
					UpdateQuestsState();
					break;
			}
		}

		/// <summary>
		/// Обновить активные квесты
		/// </summary>
		private void UpdateActiveQuests()
		{
			activeQuests = questSettings.QuestLine.GetActiveItems()?.Cast<QuestIdentificator>().ToList();
		}

		/// <summary>
		/// Все квесты завершены
		/// </summary>
		private void CompleteQuests()
		{
			if (!isCompleteWork)
			{
				isCompleteWork = true;
				this.DebugLog("Все квесты завершены");

				Send(null, new Message(Messages.QuestControl.CompleteQuests));
				Send(typeof(IReport), new Message(Messages.Report.AddRightActionInReport, questSettings.Name));

				ResetQuests();

				// Убираем пройденный набор квестов из списка
				accessQuestSettings.Remove(questSettings);

				// Если выбрали несколько квестов, то перезапускаем сцену
				if (accessQuestSettings.Any())
				{
					SettingsStorage.ScenePrepareMessages.Add(typeof(IQuestControl), new Message(Messages.QuestControl.StartWork, accessQuestSettings));
					Send(null, new Message(Messages.Load.ReloadScene, Mode.Exam));
				}
				// Иначе выводим панель с результатом
				else
				{
					Notify(new Message(Messages.QuestControl.FinishWork));
				}
			}

		}

		/// <summary>
		/// Прохождение преравано
		/// </summary>
		private void AbortQuests()
		{
			isCompleteWork = true;
			this.DebugLog("Квесты прерваны");

			ResetQuests();
			Send(typeof(IReport), new Message(Messages.Report.AddLineInReport, DateTime.Now + " Выполнение заданий было прервано"));
			Notify(new Message(Messages.QuestControl.FinishWork));
		}

		/// <summary>
		/// Сбросить состояние квестов в настройках
		/// </summary>
		private void ResetQuests()
		{
			if (questSettings)
			{
				questSettings.QuestLine.Reset();
			}
		}


		private void OnApplicationQuit()
		{
			questSettings?.QuestLine?.Reset();

		}

		/// <summary>
		/// Статус обновился
		/// </summary>
		private void UpdateQuestsState()
		{
			// Нужно ли проверять статус в зависимости от режима
			if (questSettings != null && questSettings.IsNeedCheckStatuses)
			{
				if (IsExitsUncompletedQuest(out List<QuestIdentificator> uncompletedQuest))
				{
					foreach (var questIdentificator in uncompletedQuest)
					{
						CheckQuest(questIdentificator);
					}

					UpdateActiveQuests();

					Send(typeof(IGameplayInterface), new Message(Messages.QuestControl.UpdateQuests, activeQuests));

				}

				bool isComplete = !IsExitsUncompletedQuest(out List<QuestIdentificator> nextUncompletedQuest);

				if (isComplete)
				{
					CompleteQuests();
				}
				else
				{
					foreach (var questIdentificator in nextUncompletedQuest)
					{
						if (!questIdentificator.IsStarted())
						{
							StartQuest(questIdentificator);
						}
					}
				}
			}
		}

		/// <summary>
		/// Запустить новый квест
		/// </summary>
		/// <param name="questId"></param>
		private void StartQuest(QuestIdentificator questId)
		{
			questId.Start();
			CheckQuest(questId);
		}

		/// <summary>
		/// Проверить состояние квеста
		/// </summary>
		private void CheckQuest(QuestIdentificator questId)
		{
			if (HasFinishKeyStatuses(questId))
			{
				if (HasFinishOutStatuses(questId))
				{
					if (questId.Quest.HasNextBranch())
					{
						questId.Quest.NextBranch();
						CheckQuest(questId);
					}
					else
					{
						questId.Complete();
					}
				}
			}
		}

		/// <summary>
		/// Проверить что остались невыполненные квесты
		/// </summary>
		private bool IsExitsUncompletedQuest(out List<QuestIdentificator> uncompletedQuest)
		{
			uncompletedQuest = new List<QuestIdentificator>();

			UpdateActiveQuests();

			if (activeQuests == null)
			{
				return false;
			}

			var result = false;

			foreach (var questIndetificator in activeQuests)
			{
				if (!questIndetificator.IsCompleted())
				{
					result = true;
					uncompletedQuest.Add(questIndetificator);
				}
			}
			return result;
		}


		#region Работа со статусами в квесте

		/// <summary>
		/// Проверить приведет ли статус к ошибке
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		private bool CheckStatus(StatusField status)
		{
			// Установлен режим без проверки статусов
			if (!questSettings.IsNeedCheckStatuses)
				return true;

			bool isRight = true;

			if (activeQuests != null)
			{
				foreach (var questIdentificator in activeQuests.Where(q => !q.IsCompleted()))
				{
					if (!(IsNotError(questIdentificator, status) && IsNotKeyError(questIdentificator, status)))
					{
						isRight = false;
					}
				}
			}
			return isRight;
		}

		/// <summary>
		/// Проверить, что состояния квеста не ошибочное
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="newStatusField"></param>
		/// <returns></returns>
		private bool IsNotError(QuestIdentificator questId, StatusField newStatusField)
		{
			ModulStatus errorStatuses = questId.Quest.QuestData.CurrentBranch.ErrorStatus;

			StatusField errorField = errorStatuses?.HasStatusField(newStatusField);

			if (errorField != null)
			{
				Debug.LogError("Выполнено ошибочное действие в этапе " + name + " : " + errorField.Description);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Проверить, что состояние ключевое и что оно последнее
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="newStatusField"></param>
		/// <returns></returns>
		private bool IsNotKeyError(QuestIdentificator questId, StatusField newStatusField)
		{
			ModulStatus keyModulStatus = questId.Quest.QuestData.CurrentBranch.KeyStatus;

			StatusField keyStatusField = keyModulStatus?.HasStatusField(newStatusField);

			if (keyStatusField != null)
			{
				if (!HasFinishOutStatuses(questId))
				{
					Debug.LogError("Ключевое действие привело к ошибке в этапе " + name + " : " + keyStatusField.Description, this);
					return false;
				}

			}

			Debug.Log("Ключевого нет", this);
			return true;
		}

		/// <summary>
		/// Закончены ключевые статусы
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		private bool HasFinishKeyStatuses(QuestIdentificator questId)
		{
			// Состояние выполнение всех ключевых статусов
			bool completekeyStatus = true;

			// Находим для каждого модуля его статусы в ветке
			ModulStatus keyModulStatus = questId.Quest.QuestData.CurrentBranch.KeyStatus;


			var modul = ModulControl.GetModulBySaddleId(questId.SaddleId);
			if (modul)
			{
				// Если статусы в модуле и ветке не совпадают, значит 
				if (modul.Status.CheckValues(keyModulStatus.Data).Count != 0)
				{
					completekeyStatus = false;
				}
			}


			return completekeyStatus;
		}

		/// <summary>
		/// Проверить конечные состояния всех модулей
		/// </summary>
		/// <returns></returns>
		private bool HasFinishOutStatuses(QuestIdentificator questId)
		{
			bool result = true;

			if (questId.Quest.QuestData.CurrentBranch != null)
			{
				ModulStatus outModulStatus = questId.Quest.QuestData.CurrentBranch.OutStatus;
				var modul = ModulControl.GetModulBySaddleId(questId.SaddleId);
				if (modul)
				{
					// Если статусы в модуле и ветке не совпадают, то ждем когда ключевое сделаем
					if (modul.Status.CheckValues(outModulStatus.Data).Count != 0)
					{
						result = false;
					}
				}
			}
			return result;
		}

		#endregion


	}

}