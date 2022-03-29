using UnityEngine;

namespace Core.Gameplay.Interface
{
	[CreateAssetMenu(fileName = "VRCanvasControlsSettings", menuName = "Settings/VRCanvasControlsSettings")]
	public class VRCanvasControlsSettings : GameplayCanvasControlsSettings<IVrCanvasControl, VRCanvasControlsSettings>
	{

	}
}