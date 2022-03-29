namespace Core.Global.Network
{
	/// <summary>
	/// Подключение для пользователя через интернет
	/// </summary>
	public class InternetNetworkConnection : ServerNetworkConnection
	{

		/// <summary>
		/// Инициализация интернет-сервера
		/// </summary>
		public override void Open()
		{
			//if (ComputerNetworkConfig == ConnectionType.Internet)
			//    Debug.Log("Photon Setup");

			//messager = GetComponent<PhotonMessager>();
			//parameters = GetComponent<PhotonParameters>();
			//server = GetComponent<PhotonServer>();
			//server.CreateConnection();
		}

		public override void Close()
		{

		}

		public override void CheckConnection()
		{

		}
	}
}