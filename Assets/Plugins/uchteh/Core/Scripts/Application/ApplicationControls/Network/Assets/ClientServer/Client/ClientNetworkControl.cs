using System.Collections;
using System.Linq;
using System.Threading;
using Core.Extensions;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Global.Network
{
	public sealed class ClientNetworkControl : NetworkControl<IClientNetworkConnection, ClientNetworkControlSettings>
	{
		/// <summary>
		/// Подключение к серверу
		/// </summary>
		[ShowInInspector, ReadOnly]
		public ConnectionType ServerConnection { get; private set; }

		/// <summary>
		/// Текущее соединение
		/// </summary>
		public IClientNetworkConnection CurrentConnection { get; private set; }

		protected override void OnItemsInitialised()
		{
			StartCoroutine(SetConnection());
		}

		public override void Unload()
		{
			StartCoroutine(DeleteComputerAsync());

			foreach (var networkConnection in Connections)
			{
				networkConnection.Close();
			}
		}


		#region Создание соединения

		/// <summary>
		/// Установить соединение
		/// </summary>
		/// <returns></returns>
		private IEnumerator SetConnection()
		{
			if (Connections.Any())
			{
				foreach (var connection in Connections)
				{
					yield return StartCoroutine(ConnectionTimeOut(connection));

					if (connection.IsConnected)
					{
						ServerConnection = connection.ServerConnection;
						CurrentConnection = connection;
						CurrentConnection.CheckConnection();

						StartCoroutine(AddComputerAsync());
						break;
					}
					this.DebugLog("Соединение через " + connection.ServerConnection + " недоступно");
				}

				if (!Connections.Exists(c => c.IsConnected))
				{
					this.DebugLogError("Нет подключения к базе данных");
				}
			}
		}

		/// <summary>
		/// Ожидание соединения
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		private IEnumerator ConnectionTimeOut(IClientNetworkConnection connection)
		{
			float timeout = 10;

			while (timeout > 0 && !connection.HasOpen())
			{
				this.DebugLog("Ожидание соединения через " + connection.ServerConnection + "... " + timeout + " c ");
				timeout--;
				yield return new WaitForSeconds(1f);
			}
		}

		#endregion

		#region Добавить компьютер

		/// <summary>
		/// Заполнить данные компьютера
		/// </summary>
		/// <returns></returns>
		private IEnumerator AddComputerAsync()
		{
			var thread = new Thread(() =>
			{
				if (!CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().IsCompExist(Computer.Name))
				{
					CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().AddComp(Computer);
				}
				else
				{
					Computer = CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().GetComputerByComputerName(Computer.Name);
				}
			});
			thread.Start();

			while (thread.IsAlive)
			{
				Send(typeof(ILoading), new Message(Messages.Load.LoadingStringUpdate, "Заполнение данных компьютера"));
				yield return new WaitForSeconds(0.1f);
			}
		}
		
		#endregion

		#region Удалить компьютер

		/// <summary>
		/// Заполнить данные компьютера
		/// </summary>
		/// <returns></returns>
		private IEnumerator DeleteComputerAsync()
		{
			var thread = new Thread(() =>
			{
				if (CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().IsCompExist(Computer.Name))
				{
					CurrentConnection.GetRequest<IAuthorizationDatabaseRequest>().DeleteComp(Computer);
				}
			});
			thread.Start();

			while (thread.IsAlive)
			{
				Debug.Log("Удаление данных компьютера");
				Send(typeof(ILoading), new Message(Messages.Load.LoadingStringUpdate, "Удаление данных компьютера"));
				yield return new WaitForSeconds(0.1f);
			}
		}

		#endregion

	}

}