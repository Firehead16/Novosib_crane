namespace Core.Global.Network
{
	public abstract class ServerNetworkConnection : BaseControlMethods, IServerNetworkConnection, IMessageObserver
	{
		public bool IsInitialized { get; set; }

		public abstract void Open();

		public abstract void Close();

		public abstract void CheckConnection();
	} 
}