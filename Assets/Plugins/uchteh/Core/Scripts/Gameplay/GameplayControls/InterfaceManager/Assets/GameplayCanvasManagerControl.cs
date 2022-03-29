using Core.Gameplay.Modul;
using Core.Settings;
using Core.Extensions;
using UnityEngine;

namespace Core.Gameplay.Interface
{
	public abstract class GameplayCanvasManagerControl<TGameplayPanel, TSettings> : InitableManagerWithSettins<TGameplayPanel, TSettings>, IGameplayCanvasControl
where TGameplayPanel : class, IGameplayPanel
where TSettings : SubSettings<TSettings>, IInitializebleList<TGameplayPanel>
	{

		public ISaddle Saddle { get; set; }



		public override void Load()
		{
			base.Load();
			SetSaddle();
			Initialize();
		}


		protected override void OnItemsInitialised()
		{
			Debug.Log("Тут можно сделать какую-нибудь штуку с GameplayCanvas контролами");
		}


		public void Subscribe()
		{
			Debug.Log("Тут можно сделать  еще какую-нибудь штуку с GameplayCanvas контролами");
		}

		public void UnSubscribe()
		{
		
		}

		public void SetSaddle()
		{
			this.SetSaddle(Saddle);
		}
	}
}