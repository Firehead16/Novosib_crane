using System.Collections.Generic;
using UnityEngine;

namespace Core.Settings
{
	public abstract class NetworkControlSettings<TNetworkConnection, TSetting> : SubSettings<TSetting>, IInitializebleList<TNetworkConnection>
		where TNetworkConnection : IInitialize
		where TSetting : SubSettings<TSetting>
	{
		public const int ConnectionCheckTimeout = 5;

		[SerializeField]
		private List<TNetworkConnection> connectionPrefabs = null;

		public List<TNetworkConnection> Controls => connectionPrefabs;
	}
}