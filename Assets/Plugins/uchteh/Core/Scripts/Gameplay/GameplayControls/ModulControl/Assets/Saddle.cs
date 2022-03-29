using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Modul
{
	public interface ISettable
	{

	}

	public interface IHasSaddle : ICanInstanse
	{
		ISaddle Saddle { get; set; }
		void SetSaddle();
	}
	
	public interface ISaddle : ICanInstanse
	{
		IGameplayModul Modul { get; set; }
		string Id { get; set; }
	}

	/// <summary>
	/// Гнездо, седло - места, куда ставятся модули. Должно быть просто поставлено на сцену (иметь какой-то transform)
	/// </summary>
	public class Saddle : SerializedMonoBehaviour, ISaddle
	{
		public GameObject GameObject => gameObject;

		[SerializeField, BoxGroup("Параметры седла")]
		private string id;
		public string Id
		{
			get => id;
			set => id = value;
		}

		[SerializeField, HideInInspector]
		private IGameplayModul modul;
		[ShowInInspector, HideLabel, BoxGroup("Модуль")]
		public IGameplayModul Modul
		{
			get => modul;
			set
			{
				if (modul != null)
				{
					modul.Saddle = null;
				}

				modul = value;
				modul.Saddle = this;

			}

		}

		[SerializeField]
		private string saddleName = "";
		public string Name => saddleName;

		[SerializeField]
		private string modulDescription = "";
		public string ModulDescription => modulDescription;


	} 
}

