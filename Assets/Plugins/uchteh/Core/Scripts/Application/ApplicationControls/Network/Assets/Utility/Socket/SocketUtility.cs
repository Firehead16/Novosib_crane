using System.Net.Sockets;

namespace Core.Global.Network
{
	public static class SocketUtility
	{

		//Таким образом, при создании сокета мы можем указывать разные комбинации протоколов, 
		//типов сокета, значений из перечисления AddressFamily.Однако в то же время не все комбинации являются корректными.
		//Так, для работы через протокол Tcp, нам надо обязательно указать параметры: AddressFamily.InterNetwork, SocketType.Stream и ProtocolType.Tcp.
		//Для Udp набор параметров будет другим: AddressFamily.InterNetwork, SocketType.Dgram и ProtocolType.Udp.
		//Для других протоколов набор значений будет отличаться.Поэтому использование сокетов может потребовать некоторого знания принципов работы отдельных протоколов.


		/// <summary>
		/// Создать Tcp сокет
		/// </summary>
		/// <returns></returns>
		public static Socket CreateTcpSocket()
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			return socket;
		}

		/// <summary>
		/// Создать Upd сокет
		/// </summary>
		/// <returns></returns>
		public static Socket CreateUdpSocket()
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			return socket;
		}
		
	}
}