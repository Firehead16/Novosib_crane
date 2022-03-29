using Core.Global.Network;
using UnityEngine;

namespace Core.Settings
{
	[CreateAssetMenu(fileName = "ClientNetworkControlSettings", menuName = "Settings/ClientNetworkControlSettings")]
	public class ClientNetworkControlSettings : NetworkControlSettings<IClientNetworkConnection, ClientNetworkControlSettings>
	{
		
	}
}