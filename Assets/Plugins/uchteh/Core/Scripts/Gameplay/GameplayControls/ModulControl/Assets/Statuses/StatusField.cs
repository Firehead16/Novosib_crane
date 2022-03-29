using System;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Modul
{
	/// <summary>
	/// Квестовое состояние модуля
	/// </summary>
	[Serializable]
	public abstract class StatusField
	{
		[HideLabel, SerializeField, TextArea(0, 5), HideIf("showAsList")] private string description;
		/// <summary>
		/// Описание состояние
		/// Правильно сформулировать описания в зависимости от значения действительно важно, так как они будут отображены конечному пользователю
		/// </summary>
		public string Description { get => description; protected set => description = value; }

		// ReSharper disable once InconsistentNaming
		protected bool showAsList;
		public bool ShowAsList { set => showAsList = value; }

		[HideInInspector]
		public string PositiveDescription { get; protected set; }

		[HideInInspector]
		public string NegativeDescription { get; protected set; }

		public abstract dynamic Value { get; set; }
	}

	/// <summary>
	/// Данный класс нужен для отображения квестового состояния в инспекторе
	/// </summary>
	/// <typeparam name="TStatuses"></typeparam>
	[Serializable, HideReferenceObjectPicker]
	public class StatusField<TStatuses> : StatusField
	{
		[HorizontalGroup("Квестовое состояние"), SerializeField, ShowIf("showAsList"), HideLabel]
		private TStatuses id;
		public TStatuses Id { get => id; set => id = value; }

		public override dynamic Value { get; set; }
	}

	/// <summary>
	/// данный класс нужен для инициализации конкретных value у квестового состояния  модуля
	/// </summary>
	/// <typeparam name="TModul">Модуль, у которого будет данное квестовое состояние</typeparam>
	/// <typeparam name="TStatuses">Список всех квестовых состояний модуля, создается в каждом классе</typeparam>
	/// <typeparam name="TValueType">Тип квестового состояние (bool/int/float/string/enum)</typeparam>
	public class StatusField<TModul, TStatuses, TValueType> : StatusField<TStatuses>
		where TModul : IGameplayModul
	{
		[HorizontalGroup("Квестовое состояние"), SerializeField, LabelText("Значение:"), OnValueChanged("ChangeDescription")]
		private TValueType value;

		/// <summary>
		/// Значение состояния
		/// </summary>
		/// <param name="value"></param>
		public override dynamic Value
		{
			get => value;
			set
			{
				if (this.value == value) return;
				this.value = value;

				ControlExtention.GameplaySend(typeof(IChangeStatusHandler), new Message(Messages.ModulControl.ChangeStatus, this));
			}
		}

		/// <summary>
		/// Создать состояние и проинициализировать значения
		///  </summary>
		/// <param name="id">Элемент перечисления квестовых состояний</param>
		/// <param name="value">Значение состояния</param>
		/// <param name="positiveDescription">Описание состояния, которое будет показываться пользователю по умолчания. (Если состояние типа bool, то показывается, если передан value = true, float = 10f и т д)</param>
		/// <param name="negativeDescription">Описание состояния, отображается пользователю если передан value = false </param>
		public StatusField(TStatuses id, TValueType value, string positiveDescription = "", string negativeDescription = null)
		{
			PositiveDescription = positiveDescription;
			NegativeDescription = negativeDescription;
			Description = PositiveDescription;
			if (value is bool isPositive)
			{
				if (negativeDescription == null)
				{
					NegativeDescription = "ДОБАВИТЬ ОПИСАНИЕ";
				}
				if (!isPositive) Description = NegativeDescription;

			}
			Id = id;
			this.value = value;
		}

		/// <summary>
		/// Переключает описание в инспекторе при изменении значения value
		/// </summary>
		void ChangeDescription()
		{
			if (value is bool isPositive)
			{
				Description = isPositive ? PositiveDescription : NegativeDescription;
			}
		}
		/// <summary>
		/// Проверка равенства состояний
		/// </summary>
		/// <param name="statusField">Состояние</param>
		/// <returns></returns>
		public bool Equals(StatusField statusField)
		{
			return Value.Equals(((StatusField<TModul, TStatuses, TValueType>)statusField).Value);
		}

		public override string ToString()
		{
			return Id + "" + Value;
		}
	} 
}