using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Global.Network
{
    [Serializable]
    public class UdpChat
    {   
        /// <summary>
        /// 224.0.0.0 до 239.255.255.255
        /// </summary>
	    private IPAddress remoteAddress = IPAddress.Parse("228.228.228.228");

        /// <summary>
        ///  Порт для отправки данных
        /// </summary>
        [SerializeField]
        private int remotePort = 8001;

        /// <summary>
        /// Локальный порт для прослушивания входящих подключений
        /// </summary>
        [SerializeField]
        private int localPort = 8001;

        /// <summary>
        /// Локальный адрес компьютера
        /// </summary>
        [SerializeField]
        private string localAddress;

        [SerializeField]
        private bool isWork;


        private CancellationTokenSource tokenSource;

        public UdpChat(string localIpAddress)
        {
            localAddress = localIpAddress;
        }

        /// <summary>
        /// Запуск чата
        /// </summary>
        /// <param name="action"></param>
        public void StartWorking(Action action)
        {
            isWork = true;

            tokenSource = new CancellationTokenSource();
            Task.Run(action, tokenSource.Token);
        }

        /// <summary>
        /// Остановка чата
        /// </summary>
        public void StopWorking()
        {
	        isWork = false;
	        tokenSource.Cancel();
        }


        /// <summary>
        /// Отправляем данные 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void SendBroadcast(string message, int timeout)
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки
            IPEndPoint endPoint = new IPEndPoint(remoteAddress, remotePort);
            try
            {
                while (true)
                {
	                byte[] data = Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, endPoint); // отправка

                    if (tokenSource.Token.IsCancellationRequested)
                    {
	                    Debug.Log("Cancellation Token Detected");
	                    break;
                    }
                    Thread.Sleep(timeout);
                }
            }
            catch (Exception)
			{
                Debug.Log(localAddress + " отключился TCP чата");
            }
            finally
            {
                sender.Close();
            }
        }

        /// <summary>
        /// Получаем данные
        /// </summary>
        /// <returns></returns>
        public void ReceiveBroadCast(ref string message, bool isNeedSelfReceive = false)
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            receiver.JoinMulticastGroup(remoteAddress, 20);

            IPEndPoint remoteIp = null;

            try
            {
                while (true)
                {
	                byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    if (isNeedSelfReceive && remoteIp.Address.ToString().Equals(localAddress))
                        continue;
                    if (data.Any())
                    {
                        message = Encoding.Unicode.GetString(data);
                    }

                    if (tokenSource.Token.IsCancellationRequested)
                    {
	                    Debug.Log("Cancellation Token Detected");
	                    break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}