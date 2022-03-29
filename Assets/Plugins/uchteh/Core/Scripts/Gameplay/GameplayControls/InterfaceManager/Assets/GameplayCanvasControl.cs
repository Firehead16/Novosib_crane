using Core.Gameplay.Modul;
using Core.Ui;
using Core.Extensions;
using UnityEngine;

namespace Core.Gameplay.Interface
{
	public abstract class GameplayCanvasControl<TGameplayPanel> : InitableManager<TGameplayPanel>, IGameplayCanvasControl 
		where TGameplayPanel : class, IGameplayPanel
	{
		[SerializeField] private ThemeSettings theme = null; // Use null to default Theme

		[SerializeField] private Mode canvasMode;

		public ISaddle Saddle { get; set; }

		public bool IsInitialized { get; set; }


		public override void Load()
		{
			base.Load();
			SetSaddle();
			SetActive();
		}

		public void SetSaddle()
		{
			this.SetSaddle(Saddle);
		}

		private void SetActive()
		{
			gameObject.SetActive(SettingsStorage.Mode == canvasMode);
		}
	} 
}