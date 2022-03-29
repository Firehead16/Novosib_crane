using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Global.Network
{
	[Serializable]
	public class ClientTcpObject
	{
		public event Action<ClientTcpObject, Request> OnSendRequest;

		[ShowInInspector, ReadOnly]
		protected internal string Id { get; private set; }

		protected internal NetworkStream Stream { get; private set; }

		[ShowInInspector, ReadOnly]
		private string name;

		/// <summary>
		/// Клиент TCP
		/// </summary>
		private TcpClient client;

		/// <summary>
		/// Сервер TCP
		/// </summary>
		private ServerTcpComponent server;

		public ClientTcpObject(TcpClient tcpClient, ServerTcpComponent serverTcpComponent)
		{
			Id = Guid.NewGuid().ToString();
			client = tcpClient;
			server = serverTcpComponent;

			serverTcpComponent.AddConnection(this);
		}

		/// <summary>
		/// Обработка запроса клиента
		/// </summary>
		public void Process()
		{
			try
			{
				Stream = client.GetStream();

				// получаем имя пользователя
				Request request = GetMessage();
				name = request.RequestType;
				
				// в бесконечном цикле получаем сообщения от клиента
				while (true)
				{
					try
					{
						Request receiveRequest = GetMessage();
						OnSendRequest?.Invoke(this, receiveRequest);
					}
					catch (Exception exception)
					{
						Debug.LogError($"Сервер отключил пользователя {name} с ошибкой : " + exception.Message);
						break;
					}
				}
			}
			finally
			{
				// в случае выхода из цикла закрываем ресурсы
				server.RemoveConnection(Id);
				Close();
			}
		}

		/// <summary>
		/// Чтение входящего сообщения и преобразование в строку
		/// </summary>
		/// <returns></returns>
		private Request GetMessage()
		{
			byte[] bytes = new byte[64]; // буфер для получаемых данных
			List<byte> fulldata =new List<byte>();

			do
			{
				int count = Stream.Read(bytes, 0, bytes.Length);
				fulldata.AddRange(bytes);
			}

			while (Stream.DataAvailable);
	
			Request request = Request.DeSerializeToBytes(fulldata.ToArray());


			return request;
		}

		/// <summary>
		/// Закрытие подключения
		/// </summary>
		protected internal void Close()
		{
			Stream?.Close();
			client?.Close();
		}
	}
}