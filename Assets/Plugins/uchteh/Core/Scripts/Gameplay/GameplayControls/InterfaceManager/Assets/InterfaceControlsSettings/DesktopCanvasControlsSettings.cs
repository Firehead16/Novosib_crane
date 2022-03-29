using UnityEngine;

namespace Core.Gameplay.Interface
{
	[CreateAssetMenu(fileName = "DesktopCanvasControlsSettings", menuName = "Settings/DesktopCanvasControlsSettings")]
	public class DesktopCanvasControlsSettings : GameplayCanvasControlsSettings<IDesktopCanvasControl,
			DesktopCanvasControlsSettings>
	{

	}
}