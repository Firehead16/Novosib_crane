using System.Linq;

namespace Core.Global.Network
{
	/// <summary>
	/// Подключение клиента напрямую к базе данных
	/// </summary>
	public class HostNetworkConnection : ClientNetworkConnection
	{
		private ServerNetworkControl serverNetworkControl;

		private DatabaseNetworkConnection databaseNetworkConnection;
		
		public override void Open()
		{
			serverNetworkControl = GlobalLinkStorage.Application.GetControl<ServerNetworkControl>();
			if (serverNetworkControl)
			{
				IsConnected = true;
				ServerConnection = ConnectionType.Host;

				databaseNetworkConnection = (DatabaseNetworkConnection) serverNetworkControl.Controls.Values.Single(c => c is DatabaseNetworkConnection);
			}
		}

		public override T GetRequest<T>()
		{
			return databaseNetworkConnection.GetRequest<T>();
		}

		/// <summary>
		/// Проверить соединение с локальной базой данных
		/// </summary>
		public override bool HasOpen()
		{
			return IsConnected;
		}

		public override void Close()
		{

		}

		public override void CheckConnection()
		{

		}

	}
}