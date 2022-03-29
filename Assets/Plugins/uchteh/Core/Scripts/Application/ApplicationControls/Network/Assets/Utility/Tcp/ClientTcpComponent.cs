using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Core.Global.Network
{
	public class ClientTcpComponent
	{
		public event Action<Request> OnServerResponse;

		[SerializeField]
		private string clientName;

		[SerializeField]
		private string host;

		[SerializeField]
		private int port;

		[SerializeField]
		private bool receive = true;

		private TcpClient client;
		private NetworkStream stream;

		public ClientTcpComponent()
		{
			client = new TcpClient();
		}

		/// <summary>
		/// Подключение к серверу
		/// </summary>
		/// <param name="clientTcpName"></param>
		/// <param name="serverTcp"></param>
		/// <param name="serverPort"></param>
		public void Connect(string clientTcpName, string serverTcp, int serverPort)
		{
			clientName = clientTcpName;
			host = serverTcp;
			port = serverPort;

			try
			{
				client.Connect(host, port); //подключение клиента
				Debug.Log("Подключились к серверу");

				stream = client.GetStream(); // получаем поток
				Debug.Log("Получили поток от сервера");

				SendMessage(new Request(clientName, new List<RequestParameter>()));
				Debug.Log("Отправили имя компьютера");

				// запускаем новый поток для получения данных
				Thread receiveThread = new Thread(ReceiveMessage);
				receiveThread.Start(); //старт потока

				Debug.Log("Запустили получение данных");

			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				Disconnect();
			}
		}

		/// <summary>
		/// Отключиться
		/// </summary>
		public void Disconnect()
		{
			stream?.Close();//отключение потока
			client?.Close();//отключение клиента
		}

		/// <summary>
		/// Отправка сообщений
		/// </summary>
		public void SendMessage(Request request)
		{
			var data =  Request.SerializeToBytes(request);
			stream.Write(data, 0, data.Length);
		}

		/// <summary>
		/// Получение сообщений
		/// </summary>
		private void ReceiveMessage()
		{
			while (receive)
			{
				try
				{
					byte[] data = new byte[64]; // буфер для получаемых данных
					List<byte> fulldata =new List<byte>();
					do
					{
						int bytes = stream.Read(data, 0, data.Length);
						fulldata.AddRange(data);
					}
					while (stream.DataAvailable);

					Request request = Request.DeSerializeToBytes(fulldata.ToArray());
					OnServerResponse?.Invoke(request);
				}
				catch
				{
					Debug.LogError("Подключение прервано!");
					Disconnect();
					break;
				}
			}
		}
	}
}