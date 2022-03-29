using Sirenix.OdinInspector;

namespace Core.Global.Network
{
	/// <summary>
	/// Базовый класс подключения клиента
	/// </summary>
	public abstract class ClientNetworkConnection : BaseControlMethods, IClientNetworkConnection, IMessageObserver
	{
		public bool IsInitialized { get; set; }

		[ShowInInspector, ReadOnly]
		public bool IsConnected { get; protected set; }

		[ShowInInspector, ReadOnly]
		public ConnectionType ServerConnection { get; protected set; }
		
		public override void Initialize()
		{
			Open();
			base.Initialize();
		}

		public abstract T GetRequest<T>() where T : class, IRequest;
		
		public abstract void Open();

		public abstract bool HasOpen();

		public abstract void Close();

		public abstract void CheckConnection();
	}
}