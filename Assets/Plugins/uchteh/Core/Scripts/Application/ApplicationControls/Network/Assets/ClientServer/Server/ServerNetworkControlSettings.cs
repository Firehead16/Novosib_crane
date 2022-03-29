using System;
using Core.Global.Network;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Settings
{
	[CreateAssetMenu(fileName = "ServerNetworkControlSettings", menuName = "Settings/ServerNetworkControlSettings")]
	public class ServerNetworkControlSettings : NetworkControlSettings<IServerNetworkConnection, ServerNetworkControlSettings>
	{
		[Serializable]
		public class DatabaseConfig
		{
			public Database Database;
			public bool IsNeedLoad;
		}

		[SerializeField]
		private List<DatabaseConfig> databaseConfig = null;

		public List<DatabaseConfig> DatabaseConfigs => databaseConfig;

	}
}