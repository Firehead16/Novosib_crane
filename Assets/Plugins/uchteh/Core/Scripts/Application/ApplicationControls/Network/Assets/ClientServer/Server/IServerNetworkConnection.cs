namespace Core.Global.Network
{
	public interface IServerNetworkConnection : INetworkConnection
	{
		/// <summary>
		/// Открытие соединения с клиентами
		/// </summary>
		void Open();

		/// <summary>
		/// Закрытие соединения с клиентами
		/// </summary>
		void Close();

	} 
}