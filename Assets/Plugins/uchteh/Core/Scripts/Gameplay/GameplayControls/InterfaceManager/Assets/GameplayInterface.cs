using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Gameplay.Interface
{
	public abstract class GameplayInterface<TGameplayCanvasControl> : InitableManager<TGameplayCanvasControl>, IInterfaceControl
 where TGameplayCanvasControl : class, IGameplayCanvasControl
	{
		public bool IsInitialized { get; set; }

		protected override void OnItemsInitialised()
		{
			Debug.Log("Тут можно сделать какую-нибудь штуку с GameplayCanvas контролами");
		}

	}


	public abstract class GameplayInterface<TGameplayCanvasControl, TPlatformGameplayCanvasControlsSettings> : InitableManagerWithSettins<TGameplayCanvasControl, TPlatformGameplayCanvasControlsSettings>, IInterfaceControl
		where TPlatformGameplayCanvasControlsSettings : SubSettings<TPlatformGameplayCanvasControlsSettings>, IInitializebleList<TGameplayCanvasControl>
		where TGameplayCanvasControl : class, IGameplayCanvasControl
	{
		private IEnumerable<IGameplayCanvasControl> InterfaceControls => Controls.Values;

		protected override void OnItemsInitialised()
		{
			Debug.Log("Тут можно сделать какую-нибудь штуку с GameplayCanvas контролами");
		}

		public override void Notify(Message message)
		{
			foreach (var interfaceControl in InterfaceControls)
			{
				interfaceControl.Notify(message);
			}
		}
	} 
}