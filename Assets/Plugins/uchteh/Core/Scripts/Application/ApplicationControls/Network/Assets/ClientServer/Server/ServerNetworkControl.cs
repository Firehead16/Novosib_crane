using Core.Settings;

namespace Core.Global.Network
{
	public class ServerNetworkControl : NetworkControl<IServerNetworkConnection, ServerNetworkControlSettings>
	{
		protected override void OnItemsInitialised()
		{
			foreach (var connection in Connections)
			{
				connection.Open();
			}
		}

		/// <summary>
		/// Закрытие приложения
		/// </summary>
		public override void Unload()
		{
			foreach (var connection in Connections)
			{
				connection.Close();	
			}
		}
	} 
}