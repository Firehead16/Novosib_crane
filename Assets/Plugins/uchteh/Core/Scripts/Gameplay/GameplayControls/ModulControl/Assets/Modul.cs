using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Settings;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Core.Gameplay.Modul
{
	/// <summary>
	/// Класс хранения информации о модулях и органах, которые им принадлежат. Объекты Modul загружаются из префаба во время загрузки сцены. 
	/// Не имеет поля ModulStatus, необходим когда нужно получить в ModulStatus информацию о всех модулях (когда не важно, какие статусы у модуля есть)
	/// При создании нового модуля необходимо наследоваться от Modul, а не от этого!!!!!!
	/// </summary>
	public abstract class Modul : InitableManager<IOrgan>, IGameplayModul
	{
		public abstract ModulStatus Status { get; }


		[SerializeField, ReadOnly, BoxGroup("Параметры инициализации")] private bool isInitialized;
		[SerializeField, ReadOnly, BoxGroup("Параметры инициализации")] private ISaddle saddle;

		[ShowInInspector, ReadOnly, PropertyOrder(2), LabelText("Органы модуля:"), HideLabel,
		 DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
		private SortedDictionary<string, IOrgan> Organs
		{
			get
			{
				if (Controls != null) return new SortedDictionary<string, IOrgan>(Controls);
				return null;
			}
		}
		public ISaddle Saddle
		{
			get { return saddle; }
			set { saddle = value; }
		}

		public virtual void SetSaddle()
		{
			this.SetSaddle(Saddle);
		}
		public bool IsInitialized
		{
			get => isInitialized;
			set => isInitialized = value;
		}

		public override void Load()
		{

			base.Load();
			AddStatus();
		}

		public override void Initialize()
		{
			base.Initialize();
			SetSaddle();

		}

		public void Subscribe()
		{

		}

		public void UnSubscribe()
		{

		}




		public abstract void AddStatus();
		public abstract Type GetFuncsEnum();
		public abstract Type GetStatusesEnum();
		public abstract Type GetOrganEnum();
		public abstract void Doinitializer(Initializer initializerInitializer);
		public abstract List<StatusField> StatusData { get; }

		public abstract Dictionary<string, Type> GetChildSaddles();

		public string GetSaddleId()
		{
			if (Saddle == null)
			{
				return "Free";
			}
			return Saddle.Id;
		}

		/// <summary>
		/// Идет вверх по родительским объектам до тех пор, пока не найдет модуль. Если дошел до корневого и ничего не нашел, возвращает null
		/// (использование:  При попытке создать organData в органе проверяет, принадлежит ли этот орган модулю )
		/// </summary>
		/// <param name="tr"></param>
		/// <returns></returns>
		public static Modul GetRootModul(Transform tr)
		{
			var modul = tr.GetComponent<Modul>();
			if (modul != null)
			{
				return modul;
			}
			if (tr.parent == null)
			{
				return null;
			}
			{
				return GetRootModul(tr.parent);
			}
		}

	}

	/// <summary>
	/// Класс модуля, у которого определены квестовые состояния. 
	/// </summary>
	/// <typeparam name="TModul"></typeparam>
	/// <typeparam name="TStatuses"></typeparam>
	public abstract class Modul<TModul, TStatuses> :
		Modul where TModul : Modul<TModul, TStatuses>
	{
		protected ModulStatus<TModul, TStatuses> SpecialStatus;

		public override ModulStatus Status => SpecialStatus;

		[ShowInInspector, LabelText("Квестовые состояния")]
		public override List<StatusField> StatusData
		{
			get
			{
				if (IsLoaded)
					return SpecialStatus.StatusData.Values.Cast<StatusField>()
						.ForEach(x => x.ShowAsList = true).ToList();
				return StartStatusData.Values.Cast<StatusField>()
					.ForEach(x => x.ShowAsList = true)
					.ToList();
			}

		}


		/// <summary>
		/// Словарь квестовых состояний модуля.
		/// Переопределяется и заполняется в каждом модуле.
		/// подробнее см. файл ExampleModul.cs в базовом проекте
		/// </summary>
		/// <typeparam name="TStatuses">Список квестовых состояний, создается в виде enum отдельно от класса.</typeparam>
		/// <typeparam name="TModul">Модуль, у которого будет это квестовое состояние (то есть, этот модуль)</typeparam>
		public abstract SortedDictionary<TStatuses, StatusField<TStatuses>> StartStatusData { get; }

		public override void AddStatus()
		{
			SpecialStatus = new ModulStatus<TModul, TStatuses>();
			SpecialStatus.Load();
		}
	}


	/// <summary>
	/// Модуль, у которого определены статусы, функции и органы. При работе с модулями необходимо наследоваться именно от него
	/// </summary>
	/// <typeparam name="TModul"></typeparam>
	/// <typeparam name="TStatuses"></typeparam>
	/// <typeparam name="TFuncs"></typeparam>
	/// <typeparam name="TOrgans"></typeparam>
	public abstract class Modul<TModul, TStatuses, TFuncs, TOrgans> : Modul<TModul, TStatuses>
		where TModul : Modul<TModul, TStatuses, TFuncs, TOrgans>
	{

		public abstract OrganType GetOrganType(TOrgans id);

		[Button("Добавить квест модулю")]
		private static void CreateQuest()
		{
#if UNITY_EDITOR
			SomeExtention.NewQuest<TModul, TStatuses>(AssetDatabase.GetAssetPath(Selection.activeObject));
#endif
		}


		[Button("Добавить инициализатор модулю")]
		private static void CreateInitializer()
		{
#if UNITY_EDITOR

			SomeExtention.NewInitializer<TModul, TStatuses, TFuncs, TOrgans>(AssetDatabase.GetAssetPath(Selection.activeObject));
#endif
		}


		public override Type GetFuncsEnum()
		{
			return typeof(TFuncs);
		}
		public override Type GetStatusesEnum()
		{
			return typeof(TStatuses);
		}
		public override Type GetOrganEnum()
		{
			return typeof(TOrgans);
		}


		protected override IReadOnlyCollection<IOrgan> GetChildControls()
		{
			var organs = new List<IOrgan>();
			GetOrgans(transform, ref organs);
			return organs;
		}

		/// <summary>
		/// Получает дочерние органы и добавляет их в organs
		/// </summary>
		/// <param name="tr">transform , у которого надо получить органы</param>
		/// <param name="organs">список  органов</param>
		private void GetOrgans(Transform tr, ref List<IOrgan> organs)
		{
			var organ = tr.GetComponent<IOrgan>();
			if (organ != null)
			{
				organs.Add(organ);
			}
			for (int i = 0; i < tr.childCount; i++)
			{
				Transform ct = tr.GetChild(i);

				if (ct.GetComponent<Modul>())
				{
					return;
				}
				GetOrgans(ct, ref organs);
			}
		}

		protected IOrgan GetOrgan(TOrgans id)
		{
			return Controls[id.ToString()];
		}

		protected override string GetControlKey(IOrgan organ)
		{
			return organ.Id.ToString();
		}

		protected override void AddControlOnLoad(IOrgan organ)
		{
			Controls.Add(GetControlKey(organ), organ);
		}

		protected abstract void DoFunc(TFuncs func, params object[] parameters);

		public override void Notify(Message message)
		{
			//base.Notify(message);
			dynamic parameters;
			switch (message.Type)
			{
				case Messages.ModulControl.ModulFunction:
					parameters = message.Content.Skip(1).ToArray();
					DoFunc((TFuncs)message.Content[0], parameters);
					break;
			}
		}

		public override void Doinitializer(Initializer initializerInitializer)
		{
			foreach (var modulAction in initializerInitializer.InitializerData.ModulActions)
			{
				var action = (ModulAction<TModul, TStatuses, TFuncs, TOrgans>)modulAction;
				switch (action.ModulActionType)
				{
					case ActionType.Simple:
						GetOrgan(action.OrganId).Notify(new Message(Messages.Organ.Common.OrganFunction, action.OrganFunc));
						break;
					case ActionType.Complex:
						List<object> temp = new List<object> { action.ModulFunc };
						temp.AddRange(action.Parameters);
						Notify(new Message(Messages.ModulControl.ModulFunction, temp.ToArray()));
						break;
				}
			}
		}

	}

}