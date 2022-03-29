using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Gameplay.Interface
{
	[CreateAssetMenu(fileName = "InterfaceControlsSettings", menuName = "Settings/InterfaceControlsSettings")]
	public sealed class InterfaceControlsSettings : SubSettings<InterfaceControlsSettings>, IInitializebleList<IInterfaceControl>
	{
		[SerializeField] private List<IInterfaceControl> interfaceControls = null;

		public List<IInterfaceControl> Controls => interfaceControls;
	}

}

