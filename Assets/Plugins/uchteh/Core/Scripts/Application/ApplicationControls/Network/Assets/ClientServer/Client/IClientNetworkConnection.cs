namespace Core.Global.Network
{
	public interface IClientNetworkConnection : INetworkConnection
	{
		bool IsConnected { get; }

		ConnectionType ServerConnection { get; }

		void Open();

		bool HasOpen();

		void Close();

		T GetRequest<T>() where T : class, IRequest;
	}
}