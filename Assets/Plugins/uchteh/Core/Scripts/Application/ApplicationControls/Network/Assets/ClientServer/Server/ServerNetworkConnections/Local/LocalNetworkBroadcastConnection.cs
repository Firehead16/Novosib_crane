using System;
using System.Net;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Global.Network
{
	/// <summary>
	/// Подключение для клиента в сети
	/// </summary>
	public class LocalNetworkBroadcastConnection : ServerNetworkConnection
	{
		[ShowInInspector, ReadOnly]
		private UdpChat chat;

		[ShowInInspector, ReadOnly]
		private ServerTcpComponent server;

		[SerializeField]
		private ILocalServerRequestHandler requestHandler = null;

		private DatabaseNetworkConnection databaseNetworkConnection;

		/// <summary>
		/// Время между отправкой сообщений в мс
		/// </summary>
		private const int ChatTimeout = 1000;

		/// <summary>
		/// Открыть подключение по сети к серверу
		/// </summary>
		public override void Open()
		{
			if (GlobalLinkStorage.Application.GetControl<INetwork>().NetworkConfig != ConnectionType.Host)
			{
				try
				{
					databaseNetworkConnection = FindObjectOfType<DatabaseNetworkConnection>();

					requestHandler?.Load();

					StartChat();
					StartServer();
				}
				catch (Exception ex)
				{
					Close();
					Debug.LogError(ex.Message);
				}
			}
			else
			{
				Debug.LogError("Нет сети");
			}
		}

		/// <summary>
		/// Закрыть подключение по сети к серверу
		/// </summary>
		public override void Close()
		{
			StopChat();
			StopServer();
		}

		/// <summary>
		/// Сделать запрос в базу данных
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetRequest<T>() where T : class, IRequest
		{
			if (databaseNetworkConnection == null)
			{
				throw new Exception("Нет соединения с базой данных");
			}
			return databaseNetworkConnection.GetRequest<T>();
		}

		/// <summary>
		/// Проверка работы 
		/// </summary>
		public override void CheckConnection()
		{

		}

		/// <summary>
		/// Запустить Udp чат
		/// </summary>
		private void StartChat()
		{
			Computer computer = GlobalLinkStorage.Application.GetControl<INetwork>().Computer;

			chat = new UdpChat(computer.IpAddress);
			chat.StartWorking(() => chat.SendBroadcast(computer.IpAddress, ChatTimeout));

			Debug.Log("Udp чат запущен на сервере с адресом " + computer.IpAddress);
		}

		/// <summary>
		/// Остановить Udp чат
		/// </summary>
		private void StopChat()
		{
			chat.StopWorking();
			Debug.Log("Udp чат остановлен на сервере");
		}

		/// <summary>
		/// Запустить Tcp сервер
		/// </summary>
		private void StartServer()
		{
			server = new ServerTcpComponent(IPAddress.Any, 8888);
			server.OnClientSendRequest += RequestHandler;
			server.Connect();
		}

		/// <summary>
		/// Остановить Tcp сервер
		/// </summary>
		private void StopServer()
		{
			server.OnClientSendRequest -= RequestHandler;
			server.Disconnect();
		}

		/// <summary>
		/// Запрос от пользователя
		/// </summary>
		/// <param name="client"></param>
		/// <param name="request"></param>
		private void RequestHandler(ClientTcpObject client, Request request)
		{

			if (requestHandler == null)
			{
				throw new Exception("Не могу обработать запрос к серверу");
			}

			Request response = requestHandler.GetHandlerResponse(request);
			var data =  Request.SerializeToBytes(response);
			server.SendMessage(data, client.Id);
		}

	}
}