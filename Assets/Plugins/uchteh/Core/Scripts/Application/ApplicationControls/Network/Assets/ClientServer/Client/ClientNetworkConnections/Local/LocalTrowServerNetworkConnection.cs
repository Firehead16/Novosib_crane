using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Global.Network
{
	/// <summary>
	/// Подключение к серверу по сети
	/// </summary>
	public class LocalTrowServerNetworkConnection : ClientNetworkConnection
	{
		private UdpChat clientUdp;
		
		[ShowInInspector, ReadOnly]
		private ClientTcpComponent clientTcp;
		
		[SerializeField]
		private List<ILocalClientRequestHandler> requestHandlers = new List<ILocalClientRequestHandler>();
		
		private Coroutine connectionCoroutine;

		private Dictionary<string,Request> requestList = new Dictionary<string,Request>();

		private const int TimeOutServerWait = 10000;

		public override void Open()
		{
			ServerConnection = ConnectionType.Local;
		}

		public override void Close()
		{
			StopTcpClient();
		}

		public override T GetRequest<T>()
		{
			if (!requestHandlers.Exists(r => r is T))
			{
				throw new Exception("Невозможно обработать запрос по сети");
			}

			return (T)requestHandlers.Single(r => r is T);
		}

		public override bool HasOpen()
		{
			if (GlobalLinkStorage.Application.GetControl<INetwork>().NetworkConfig == ConnectionType.Host)
				return false;

			if (connectionCoroutine == null)
			{
				connectionCoroutine = StartCoroutine(WaitLocalConnecting());
			}

			foreach (var localRequestHandler in requestHandlers)
			{
				localRequestHandler.Load();
			}

			return IsConnected;
		}

		public override void CheckConnection()
		{

		}

		/// <summary>
		/// Соединиться с базой данных в сети
		/// </summary>
		/// <returns></returns>
		private IEnumerator WaitLocalConnecting()
		{
			if (PlayerPrefs.HasKey("ServerAddress"))
			{
				string address = PlayerPrefs.GetString("ServerAddress");

				try
				{
					StartTcpClient(address, 8888);
					IsConnected = true;
					yield break;
				}
				catch
				{
					IsConnected = false;
				}
			}

			int timeOut = 5;
			string message = "";

			clientUdp = new UdpChat(GlobalLinkStorage.Application.GetControl<INetwork>().Computer.IpAddress);
			clientUdp.StartWorking(() => clientUdp.ReceiveBroadCast(ref message));

			while (timeOut > 0 && string.IsNullOrEmpty(message))
			{
				Debug.Log("Ожидание сообщения от сервера. Сообщение: " + message + "... " + timeOut + " c ");
				timeOut--;
				yield return new WaitForSecondsRealtime(1f);
			}

			clientUdp.StopWorking();

			if (!string.IsNullOrEmpty(message))
			{
				PlayerPrefs.SetString("ServerAddress", message);
				StartTcpClient(message, 8888);

				IsConnected = true;
			}
		}

		/// <summary>
		/// Запустить Tcp клиент
		/// </summary>
		/// <param name="serverIpAddress"></param>
		/// <param name="port"></param>
		private void StartTcpClient(string serverIpAddress, int port)
		{
			clientTcp = new ClientTcpComponent();
			clientTcp.Connect(GlobalLinkStorage.Application.GetControl<INetwork>().Computer.Name, serverIpAddress, port);
			clientTcp.OnServerResponse += ReceiveServerMessage;
			Debug.Log("Запущен TCP клиент");
		}

		/// <summary>
		/// Остановить Tcp клиента
		/// </summary>
		private void StopTcpClient()
		{
			if (clientTcp != null)
			{
				clientTcp.OnServerResponse -= ReceiveServerMessage;
				clientTcp.Disconnect();
				Debug.Log("Остановлен TCP клиент");
			}
		}

		public T SendServerMessage<T>(Request request)
		{
			request.WithResponse = true;
			SendServerMessage(request);
			
			Request response = null;
		
			Debug.Log("Клиент ждет ответ");

			Task waitTask = new Task(() => response = Wait(request));
			waitTask.Start();
			waitTask.Wait();

			Debug.Log("Клиент проверяет ответ");
			T responseParameters = CheckParameters<T>(response);
			requestList.Remove(response.Id);

			Debug.Log("Клиент передает ответ программе:" + responseParameters);
			return responseParameters;
		}

		private Request Wait(Request request)
		{
			int timeWait = TimeOutServerWait;
			while (!request.IsResponse && timeWait > 0)
			{
				timeWait--;
				Thread.Sleep(1000);
			}

			return request;
		}

		private T CheckParameters<T>(Request response)
		{
			if (response.Parameters.Any() && !(response.Parameters?[0].Value is T))
			{
				throw new Exception("Клиент не получил требуемых параметров. Полученные: "
				                    + response.Parameters?[0].Value.GetType()
				                    + " Необходимые: " 
				                    + typeof(T));
			}

			T answer = (T) response.Parameters[0].Value;
			return answer;
		}

		public void SendServerMessage(Request request)
		{
			requestList.Add(request.Id,request);
			clientTcp.SendMessage(request);
		}

		public void ReceiveServerMessage(Request request)
		{
			if (requestList.ContainsKey(request.Id))
			{
				Request currentRequest = requestList[request.Id];

				if (currentRequest.WithResponse)
				{
					currentRequest.Parameters = request.Parameters;
					currentRequest.IsResponse = true;
				}
				else
				{
					requestList.Remove(request.Id);
				}
			}
		}
	}
}