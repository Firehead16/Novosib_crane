using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Modul
{
	public interface IOrgan : IGameplayControl
	{
		dynamic Id { get; }

		List<Info> GetInformation();

		Type GetConrollableEnum();
	}

	public class Info
	{
		public string Name;
		public List<string> Description;
	}

	public class OrganData
	{
		/// <summary>
		/// Информация об органе
		/// </summary>
		public List<Info> Information = new List<Info>();

		[SerializeField, HideInInspector]
		private IGameplayModul currentModul;

		public IGameplayModul CurrentModul
		{
			get => currentModul;
			set => currentModul = value;
		}

		public virtual dynamic Id
		{
			get => null;
		}

		public virtual void SetId(dynamic id)
		{

		}

		[SerializeField]
		private OrganType type;
		public OrganType Type
		{
			get
			{
				var organType = type;
				return organType;
			}
			protected internal set => type = value;
		}
	}

	[Serializable]
	public class OrganData<TOrganId> : OrganData
	{
		[SerializeField]
		private TOrganId id;
		public override dynamic Id => id;

		public override void SetId(dynamic organId)
		{

			id = (TOrganId)organId;
		}
	}

	[ExecuteInEditMode]
	public abstract class Organ : BaseControlMethods, IOrgan
	{
		[SerializeField]
		protected OrganData OrganData;
		public dynamic Id { get => OrganData?.Id; }

		[ShowInInspector, BoxGroup("Параметры инициализации")]
		public bool IsInitialized { get; set; }
		
		/// <summary>
		/// работает при наведение курсора на орагн
		/// </summary>
		protected virtual void FocusUpdate()
		{
			Debug.Log("Organ  FocusUpdate");
		}

#if UNITY_EDITOR
		private void Awake()
		{
			CreateOrganData();
		}
#endif

		public List<Info> GetInformation()
		{
			return OrganData?.Information;
		}

		public virtual Type GetConrollableEnum()
		{
			return null;
		}

		private void CreateOrganData()
		{
			if (OrganData == null)
			{
				TryCreateOrganData();
			}
		}


		private void TryCreateOrganData()
		{
			var currentModul = Modul.GetRootModul(transform);
			if (currentModul == null)
			{
				Debug.LogError("Недопускается пользоваться органами вне модуля");
			}
			if (OrganData == null) OrganData = (OrganData)Activator.CreateInstance(typeof(OrganData<>).MakeGenericType(currentModul?.GetOrganEnum()));
			OrganData.CurrentModul = currentModul;
		}

		public static IOrgan GetRootOrgan(Transform tr)
		{
			var organ = tr.GetComponent<IOrgan>();
			if (organ != null)
			{
				return organ;
			}
			if (tr.parent == null)
			{
				return null;
			}
			{
				return GetRootOrgan(tr.parent);
			}
		}
	}
}
