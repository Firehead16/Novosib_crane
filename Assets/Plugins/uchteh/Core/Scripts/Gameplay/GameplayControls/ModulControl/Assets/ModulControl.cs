using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;


namespace Core.Settings
{
	public static partial class Messages
	{
		public static List<string> GetOrganFuns(OrganType type)
		{
			var result = new List<string>();

			foreach (var flagName in Enum.GetNames(typeof(OrganType)))
			{
				Enum.TryParse(flagName, false, out OrganType flag);
				if (type == OrganType.All)
				{
					continue;
				}
				if (type.HasFlag(flag))
				{
					result.AddRange(Type.GetType(typeof(Organ).FullName + "+" + flag)?.GetFields().Select(s => s.Name).ToList() ?? throw new InvalidOperationException());
				}
			}
			return result;
		}

		public enum ModulControl
		{
			DoInitializers,
			ModulsCheck,
			ChangeStatus,
			ModulFunction,
			UpdateQuestStatuses,
		}

		public static class Organ
		{
			public enum Common
			{
				Activate,
				Disable,
				Check,
				SendParameters,
				OrganFunction,
			}

			public enum WayPoint
			{
				EnterWayPoint,
				ExitWayPoint,
				SetCurrentWayPoint,
			}

			public enum OpenClose
			{
				Open,
				Close,
			}

			public enum Axis
			{
				ForwardMove,
				BackwardMove
			}

			public enum CircleAxis
			{
				LeftRotate,
				RightRotate,
			}

			public enum OnOff
			{
				On, 
				Off,
			}

			public enum BlockUnblock
			{
				Block, 
				Unblock,
			}

			public enum Light
			{
				On,
				Off,
				Blink,
				Near,
				Far,
			}

			public enum Wiper
			{
				On,
				Off,
				Blink,
			}

			public enum Screw
			{
				ScrewOut, 
				ScrewIn,
			}

			public enum PultButton
			{
				Press, 
				Release,
			}

			public enum PultEncoder
			{
				SetTo,
				SetMinimum,
				SetMaximum,
			}
		}

	}

}

[Flags]
public enum OrganType
{
	Common = 1 << 0,
	OpenClose = 1 << 1,
	Axis = 1 << 2,
	Light = 1 << 3,
	Wiper = 1 << 4,
	WayPoint = 1 << 5,
	OnOff = 1 << 7,
	BlockUnblock = 1 << 8,
	Screw = 1 << 9,
	PultEncoder = 1 << 14,
	PultButton = 1 << 15,
	All = int.MaxValue
}



namespace Core.Gameplay.Modul
{
	public enum ValueType
	{
		BoolValue,
		FloatValue,
		StringValue,
		IntValue,
		EnumValue
	}


	public struct SceneModulInfo
	{
		public string Type;
		public List<string> Organs;
		public List<string> Funcs;
		public List<string> Statuses;
	}


	/// <summary>
	///  позволяет работать с модулями, получать их статусы и описания модулей 
	/// </summary>
	public sealed class ModulControl : InitableManager<IGameplayModul>, IModulControl
	{
		[ShowInInspector, ListDrawerSettings(HideAddButton = true), BoxGroup("Текущие состояния")]
		private List<IGameplayModul> LoadedModuls => IsLoaded ? Controls.Values.ToList() : null;
		public bool IsInitialized { get; set; }


		protected override void ControlsLoaded()
		{

		}

		protected override void OnItemsInitialised()
		{

		}

		protected override IReadOnlyCollection<IGameplayModul> GetChildControls()
		{
			return GetModulsOnScene();
		}


		//Todo добавить модули по вложенности гнезда, а не по инлексации



		/// <summary>
		/// Загружает модуль, проверяет, есть ли у него гнездо, содержащее индетификатор. 
		/// Если есть, то записывает в modulId Id седла. 
		/// </summary>
		/// <param name="loadedControl"></param>  
		protected override void AddControlOnLoad(IGameplayModul loadedControl)
		{
			var modulId = loadedControl.GetSaddleId();
			var offset = 0;

			do
			{
				if (string.Equals(modulId, "Free"))
				{
					modulId += loadedControl.GetType();
					modulId += (GetFreeCloneCount(modulId) + offset).ToString();
				}
				offset++;
			} while (Controls.ContainsKey(modulId));
			Controls.Add(modulId, loadedControl);
		}

		private int GetFreeCloneCount(string modulId)
		{
			return Controls.Values.Count(x => ("Free" + x.GetType()) == modulId);
		}

		public override void Notify(Message message)
		{
			base.Notify(message);

			switch (message.Type)
			{
				case Messages.ModulControl.DoInitializers:
					DoInitializers(message.Content[0] as List<InitializerIdentificator>);
					break;
				case Messages.ModulControl.UpdateQuestStatuses:
					foreach (var modul in Controls.Values)
					{
						modul.Notify(new Message(Messages.ModulControl.UpdateQuestStatuses));
					}
					break;
			}

			foreach (var controlsValue in Controls.Values)
			{
				controlsValue.Notify(message);
			}
		}

		public T[] GetControls<T>()
		{
			try
			{
				var result = Controls.Values.Where(x => x is T).Cast<T>().ToArray();
				return result;
			}
			catch (Exception)
			{
				Debug.LogError("Не найден Control типа:" + typeof(T));
				return null;
			}
		}

		private void DoInitializers(List<InitializerIdentificator> initializers)
		{
			foreach (var initializer in initializers)
			{
				Controls[initializer.SaddleId].Doinitializer(initializer.Initializer);
			}
		}


		#region Статические методы



		[ShowInInspector]
		[InfoBox("Модули одного типа на сцене должны содержать одинаковый набор органов!")]
		public List<SceneModulInfo> ModulsInfo => GetModulsInfo();



		public static List<SceneModulInfo> GetModulsInfo()
		{
			List<SceneModulInfo> result = new List<SceneModulInfo>();
			var modulTypes = typeof(Modul).Assembly.GetTypes()
				.Where(x => !x.IsAbstract) // Excludes BaseClass
				.Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
				.Where(x => typeof(Modul).IsAssignableFrom(x)).ToList(); // Excludes classes not inheriting from BaseClass


			foreach (var type in modulTypes)
			{
				SceneModulInfo info = GetModulInfo(type);
				result.Add(info);
			}
			return result;
		}

		public static SceneModulInfo GetModulInfo<TModul>() => GetModulInfo(typeof(TModul));
		public static SceneModulInfo GetModulInfo(Type modulType)
		{


			SceneModulInfo info = new SceneModulInfo
			{
				Type = modulType.ToString(),
				Organs = Enum.GetNames(GetModulOrgansEnum(modulType)).ToList(),
				Funcs = Enum.GetNames(GetModulFuncsEnum(modulType)).ToList(),
				Statuses = Enum.GetNames(GetModulStatusesEnum(modulType)).ToList()
			};
			return info;
		}

		public static Type GetModulStatusesEnum<TModul>() => GetModulStatusesEnum(typeof(TModul));
		public static Type GetModulStatusesEnum(Type modulType)
		{
			try
			{
				if (modulType.IsGenericType) return modulType.GenericTypeArguments[1];
				// ReSharper disable PossibleNullReferenceException
				return modulType.BaseType.GenericTypeArguments[1];
			}
			catch (Exception)
			{
				Debug.Log("Нет состояний у модуля " + modulType);
				return null;
			}
		}

		public static Type GetModulFuncsEnum<TModul>() => GetModulOrgansEnum(typeof(TModul));
		public static Type GetModulFuncsEnum(Type modulType)
		{
			try
			{
				if (modulType.IsGenericType) return modulType.GenericTypeArguments[2];
				return modulType.BaseType.GenericTypeArguments[2];
			}
			catch (Exception)
			{
				Debug.Log("Нет функций у модуля " + modulType);
				return null;
			}
		}


		public static Type GetModulOrgansEnum<TModul>() => GetModulOrgansEnum(typeof(TModul));
		public static Type GetModulOrgansEnum(Type modulType)
		{
			try
			{
				if (modulType.IsGenericType) return modulType.GenericTypeArguments[3];
				return modulType.BaseType.GenericTypeArguments[3];
				// ReSharper restore PossibleNullReferenceException
			}
			catch (Exception)
			{
				Debug.Log("Нет органов у модуля " + modulType);
				return null;
			}

		}


		/// <summary>
		/// Получить модуль по названию типа
		/// </summary>
		/// <param name="saddleId"></param>
		/// <returns></returns>
		public static Modul GetModulBySaddleId(string saddleId)
		{
			Modul modul = GetModulsOnScene().First(o => o.Saddle?.Id == saddleId);
			return modul;
		}



		/// <summary>
		/// Получить все модули со сцены
		/// </summary>
		/// <returns></returns>
		public static List<Modul> GetModulsOnScene()
		{
			return FindObjectsOfType<Modul>().ToList();
		}

		/// <summary>
		/// Получить модули со сцены
		/// </summary>
		/// <returns></returns>
		public static List<Modul> GetModulsOnScene<TModul>()
		{
			return GetModulsOnScene().Where(m => m is TModul).ToList();
		}



		/// <summary>
		/// Получить экземпляр модуля из кода
		/// </summary>
		/// <returns></returns>
		public static TModul GetModulInstance<TModul>()
			where TModul : class, IGameplayModul
		{
			return (TModul)Activator.CreateInstance(typeof(TModul));
		}


		public static TModul GetCompleteModulInstance<TModul, TStatuses, TFuncs, TOrgans>()
			where TModul : Modul<TModul, TStatuses, TFuncs, TOrgans>
		{
			return (TModul)Activator.CreateInstance(typeof(TModul));
		}

		/// <summary>
		/// Получить экземпляр модуля из кода
		/// </summary>
		/// <returns></returns>
		private static Modul GetModulInstance(Type type)
		{
			return (Modul)Activator.CreateInstance(type);
		}


		private static Modul GetModulOnScene<TModul>()
		{
			return GetModulsOnScene<TModul>().First();
		}

		/// <summary>
		/// Получить гнездо по типу модуля
		/// </summary>
		/// <returns></returns>
		public static List<string> GetSaddleIdsOnScene(Type type)
		{
			HashSet<string> result = new HashSet<string>();

			//var saddleTypes = type.Assembly.GetTypes()
			//    .Where(x => !x.IsAbstract)
			//    .Where(x => !x.IsGenericTypeDefinition)
			//    .Where(x => typeof(Saddle).IsAssignableFrom(x))
			//    .ToList();
			var saddleTypes = typeof(Saddle).Assembly.GetTypes()
				.Where(x => x.BaseType == typeof(Saddle))
				.Where(x => !x.IsAbstract)
				.Where(x => !x.IsGenericTypeDefinition)
				.Where(x => x.GetInterfaces().Contains(type)).ToList();

			foreach (var saddleType in saddleTypes)
			{

				foreach (Saddle saddleObject in FindObjectsOfType(saddleType))
				{
					result.Add(saddleObject.Id);

				}

			}
			return result.ToList();
		}

		public static Saddle GetSaddle(string saddleId)
		{
			Saddle saddle = FindObjectsOfType<Saddle>().SingleOrDefault(s => s.Id == saddleId);
			return saddle;
		}

		public static List<string> GetChildSaddlesIdInFreeModuls(Type saddleIntefaceType)
		{
			HashSet<string> result = new HashSet<string>();



			foreach (var type in ModulTypes())
			{
				Modul modul = GetModulInstance(type);
				var childSaddles = modul.GetChildSaddles();
				if (childSaddles == null) continue;

				result.AddRange(childSaddles.Where(x => x.Value == saddleIntefaceType)
					.Select(x => x.Key));
			}
			return result.ToList();
		}

		private static List<Type> ModulTypes()
		{
			var modulTypes = typeof(Modul).Assembly.GetTypes()
				.Where(x => !x.IsAbstract) // Excludes BaseClass
				.Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
				.Where(x => typeof(Modul).IsAssignableFrom(x))
				.ToList(); // Excludes classes not inheriting from BaseClass    //TODO  проверить
			return modulTypes;
		}


		public static List<string> GetChildSaddlesInPotentialModuls(Type saddleIntefaceType)
		{
			HashSet<string> result = new HashSet<string>();

			var modulTypes = ModulTypes().Where(x => x.IsAssignableFrom(saddleIntefaceType));

			foreach (var type in modulTypes)
			{
				Modul modul = GetModulInstance(type);
				var childSaddles = modul.GetChildSaddles();
				if (childSaddles == null) continue;

				result.AddRange(modul.GetChildSaddles().
					Where(x => x.Value == saddleIntefaceType)
					.Select(x => x.Key));
			}
			return result.ToList();
		}


		/// <summary>
		/// Получить список статусов модуля
		/// </summary>
		public static SortedDictionary<TStatuses, StatusField<TStatuses>> GetStartStatusesModulOnScene<TModul, TStatuses>()
			where TModul : Modul<TModul, TStatuses>
		{
			return GetModulInstance<TModul>().StartStatusData;
		}

		public static StatusField<TStatuses> GetStartStatus<TModul, TStatuses>(TStatuses id)
			where TModul : Modul<TModul, TStatuses>
		{
			return GetModulInstance<TModul>().StartStatusData[id];
		}




		//TODO  перенести в экстеншен с чпу
		//public static List<UQS_CanalCoord> GetCanalCoords(string canalType)
		//{
		//    var result = new List<UQS_CanalCoord>();

		//    //switch (canalType)
		//    //{
		//    //	case "MainCanal":
		//    //		foreach (var coord in MainCanal.Coords)
		//    //		{
		//    //			result.Add(new UQS_CanalCoord(coord.ToString(), 0));
		//    //		}
		//    //		break;
		//    //	case "RotationsCanal":
		//    //		foreach (var coord in RotationsCanal.Coords)
		//    //		{
		//    //			result.Add(new UQS_CanalCoord(coord.ToString(), 0));
		//    //		}
		//    //		break;
		//    //}

		//    return result;
		//}

		#endregion


		public void Subscribe()
		{
			Debug.Log("Тут можно сделать  еще какую-нибудь штуку с IGameplayModul контролами");
		}

		public void UnSubscribe()
		{

		}

		public void SetSaddle()
		{
			//this.SetSaddle(Saddle);
		}
	}
}
