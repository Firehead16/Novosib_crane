using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Modul
{
	/// <summary>
	/// Хранилище словаря квестовых состояний конкретного модуля
	/// </summary>
	public abstract class ModulStatus : IControl
	{
		public abstract void LoadEmpty(bool isPart = false);
		public abstract void Load();
		public bool IsNewInstanse { get; set; }
		[SerializeField, HideInInspector] private bool isLoaded;
		public bool IsLoaded
		{
			get => isLoaded;
			set => isLoaded = value;
		}

		[SerializeField, HideInInspector] private bool isPart;
		public bool IsPart
		{
			get => isPart;
			set => isPart = value;
		}

		public void Unload() { }

		protected virtual void OnControlsLoaded() { }

		public abstract StatusField HasStatusField(StatusField newStatusField);

		public abstract List<StatusField> CheckValues(List<StatusField> statusData);

		public abstract List<StatusField> Data { get; }

	}

	public class ModulStatus<TModul, TStatuses> : ModulStatus
		where TModul : Modul<TModul, TStatuses>
	{

		[SerializeField, DictionaryDrawerSettings(KeyLabel = "Состояние", ValueLabel = "Подсказка и значение"), OnValueChanged("CheckControls", true)]
		private SortedDictionary<TStatuses, StatusField<TStatuses>> controls;
		public SortedDictionary<TStatuses, StatusField<TStatuses>> StatusData => IsLoaded ? controls : ModulControl.GetStartStatusesModulOnScene<TModul, TStatuses>();

		public override List<StatusField> Data
		{
			get
			{
				if (IsPart)
				{
					return controls.Values.Cast<StatusField>().ToList();
				}
				return StatusData.Values.Cast<StatusField>().ToList();
			}
		}

		public override void Load()
		{
			LoadEmpty();
			LoadControls(GetChildControls());
			OnControlsLoaded();
			IsLoaded = true;
		}
		public void LoadControls(IReadOnlyCollection<StatusField<TStatuses>> controlsToLoad)
		{
			foreach (var control in controlsToLoad)
			{
				AddControlOnLoad(control);
			}
		}


		private int count;

		private void CheckControls()
		{
			foreach (var keyValuePair in controls)
			{
				if (keyValuePair.Value == null || !keyValuePair.Key.Equals(keyValuePair.Value.Id))
				{
					controls[keyValuePair.Key] = GetStartStatus(keyValuePair.Key);
					break;
				}
			}
		}

		/// <summary>
		/// Загрузка пустого списка, вызывается при создании новой ветки и загрузке modulstatus
		/// </summary>
		public override void LoadEmpty(bool isPart = false)
		{
			{
				IsPart = isPart;
				controls = new SortedDictionary<TStatuses, StatusField<TStatuses>>();
			}
		}


		protected IReadOnlyCollection<StatusField<TStatuses>> GetChildControls()
		{
			return ModulControl.GetStartStatusesModulOnScene<TModul, TStatuses>().Values;
		}

		protected StatusField<TStatuses> GetStartStatus(TStatuses id)
		{
			return ModulControl.GetStartStatus<TModul, TStatuses>(id);
		}


		protected void AddControlOnLoad(StatusField<TStatuses> loadedControl)
		{
			controls.Add(GetControlKey(loadedControl), loadedControl);
		}


		protected TStatuses GetControlKey(StatusField loadedControl)
		{
			return ((StatusField<TStatuses>)loadedControl).Id;
		}

		/// <summary>
		/// Получить описание статуса
		/// </summary>
		/// <param name="id"></param>
		/// <param name="needPoositive"></param>
		/// <returns></returns>
		public string GetDescription(TStatuses id, bool needPoositive = true)
		{
			if (needPoositive) return StatusData.First(s => s.Key.Equals(id)).Value.PositiveDescription;
			return StatusData.First(s => s.Key.Equals(id)).Value.NegativeDescription;
		}

		/// <summary>
		/// Проверить состояния 
		/// </summary>
		/// <param name="statusDataFromQuest"></param>
		/// <returns></returns>
		public override List<StatusField> CheckValues(List<StatusField> statusDataFromQuest)
		{

			var changeStatuses = new List<StatusField>();

			foreach (var statusFieldInQuest in statusDataFromQuest)
			{
				var statusInQuest = (StatusField<TStatuses>)statusFieldInQuest;
				var statusInModul = StatusData.Single(s => s.Key.Equals(statusInQuest.Id)).Value;

				if (statusInModul.Value != statusInQuest.Value)
				{
					changeStatuses.Add(statusInQuest);
				}
			}
			return changeStatuses;
		}

		/// <summary>
		/// Проверить состояния 
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public override StatusField HasStatusField(StatusField status)
		{
			var statusField = (StatusField<TStatuses>)status;

			var errorStatus = StatusData.SingleOrDefault(s => s.Key.Equals(statusField.Id)).Value;

			if (errorStatus == null)
				return null;

			if (status.Equals(errorStatus))
				return errorStatus;
			return null;
		}

		/// <summary>
		/// Получить состояние по  идентификатору
		/// </summary>
		public StatusField<TStatuses> GetStatusField(TStatuses id)
		{
			return controls[id];
		}

	} 
}