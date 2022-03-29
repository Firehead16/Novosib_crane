using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using Sirenix.OdinInspector;

namespace Core.Settings
{
	public partial class Messages
	{
		public enum NetWork
		{
			
		}
	}
}

namespace Core.Global.Network
{
	public abstract class NetworkControl<TNetworkConnection, TNetworkControlSettings> : InitableManagerWithSettins<TNetworkConnection, TNetworkControlSettings>, INetwork
        where TNetworkConnection : class, INetworkConnection
        where TNetworkControlSettings : SubSettings<TNetworkControlSettings>, IInitializebleList<TNetworkConnection>
    {
        protected List<TNetworkConnection> Connections => Controls.Values.ToList();

        /// <summary>
        /// Текущее сетевое состояние компьютера
        /// </summary>
        [ShowInInspector, ReadOnly]
        public ConnectionType NetworkConfig { get; private set; }

        /// <summary>
        /// Текущий компьютер
        /// </summary>
        [ShowInInspector, ReadOnly]
        public Computer Computer { get; protected set; }
        
        public override void Load()
        {
	        Computer = new Computer
            {
	            Name = Environment.MachineName
            };

            NetworkConfig = NetworkUtility.GetNetworkStatus(Computer);
            base.Load();
        }
    }
} 