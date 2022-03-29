using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Global.Network
{
	[Serializable]
	public class ServerTcpComponent
	{
		public event Action<ClientTcpObject, Request> OnClientSendRequest;

		/// <summary>
		/// Сервер для прослушивания
		/// </summary>
		private static TcpListener listener; 

		[ShowInInspector]
		/// <summary>
		/// Клиенты
		/// </summary>
		private List<ClientTcpObject> clients = new List<ClientTcpObject>(); 

		private IPAddress ipAddress;
		
		[SerializeField]
		private int port;

		public ServerTcpComponent(IPAddress listenAddress, int listenPort)
		{
			ipAddress = listenAddress;
			port = listenPort;
		}

		/// <summary>
		/// Добавить связсь с сервером
		/// </summary>
		/// <param name="client"></param>
		protected internal void AddConnection(ClientTcpObject client)
		{
			client.OnSendRequest += OnClientSendRequest;
			clients.Add(client);
		}

		/// <summary>
		/// Удалить связь с сервером 
		/// </summary>
		/// <param name="id"></param>
		protected internal void RemoveConnection(string id)
		{
			// получаем по id закрытое подключение
			ClientTcpObject client = clients.FirstOrDefault(c => c.Id == id);
		
			// и удаляем его из списка подключений
			if (client != null)
			{
				client.OnSendRequest -= OnClientSendRequest;
				clients.Remove(client);
			}
				
		}
		
		/// <summary>
		/// Подключение сервера
		/// </summary>
		protected internal void Connect()
		{
			listener = new TcpListener(ipAddress, port);
			listener.Start();

			Task.Run(Listen);
			Debug.Log("Tcp сервер запущен. Ожидание подключений...");
		}

		/// <summary>
		/// Отключение всех клиентов
		/// </summary>
		protected internal void Disconnect()
		{
			// Остановка сервера
			listener.Stop(); 

			// Отключение клиентов
			foreach (var client in clients)
			{
				client.Close();
			}

			Debug.Log("Tcp сервер остановлен");
		}

		/// <summary>
		/// Прослушивание входящих подключений
		/// </summary>
		protected internal void Listen()
		{
			try
			{
				while (true)
				{
					TcpClient tcpClient = listener.AcceptTcpClient();
					ClientTcpObject clientTcpObject = new ClientTcpObject(tcpClient, this);
					
					Thread clientThread = new Thread(clientTcpObject.Process);
					clientThread.Start();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Disconnect();
			}
		}

		/// <summary>
		/// Сообщение конкретному клиенту
		/// </summary>
		/// <param name="message"></param>
		/// <param name="id"></param>
		protected internal void SendMessage(string message, string id)
		{
			var client = clients.First(c => c.Id == id);
			byte[] data = Encoding.Unicode.GetBytes(message);
			client.Stream.Write(data, 0, data.Length); //передача данных
		}

		/// <summary>
		/// Сообщение конкретному клиенту
		/// </summary>
		/// <param name="data"></param>
		/// <param name="id"></param>
		protected internal void SendMessage(byte[] data, string id)
		{
			var client = clients.First(c => c.Id == id);
			client.Stream.Write(data, 0, data.Length); //передача данных
		}

		/// <summary>
		/// Трансляция сообщения подключенным клиентам
		/// </summary>
		/// <param name="message"></param>
		/// <param name="id"></param>
		protected internal void BroadcastMessage(string message, string id)
		{
			byte[] data = Encoding.Unicode.GetBytes(message);
			for (int i = 0; i < clients.Count; i++)
			{
				if (clients[i].Id != id) // если id клиента не равно id отправляющего
				{
					clients[i].Stream.Write(data, 0, data.Length); //передача данных
				}
			}
		}

		/// <summary>
		/// Трансляция сообщения подключенным клиентам
		/// </summary>
		/// <param name="message"></param>
		/// <param name="id"></param>
		protected internal void BroadcastMessage(byte[] data, string id)
		{
			for (int i = 0; i < clients.Count; i++)
			{
				if (clients[i].Id != id) // если id клиента не равно id отправляющего
				{
					clients[i].Stream.Write(data, 0, data.Length); //передача данных
				}
			}
		}
	}
}