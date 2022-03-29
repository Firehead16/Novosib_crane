using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Gameplay.Interface
{
	public abstract class GameplayCanvasControlsSettings<TGameplayCanvasControl, TSetting> : SubSettings<TSetting>, IInitializebleList<TGameplayCanvasControl>
			where TGameplayCanvasControl : IGameplayCanvasControl
			where TSetting : SubSettings<TSetting>

	{
		[SerializeField] private List<TGameplayCanvasControl> interfaceControls = null;

		public List<TGameplayCanvasControl> Controls => interfaceControls;
	}
}