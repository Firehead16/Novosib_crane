using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace Core.Global.Network
{
	public enum ConnectionType
	{
		Host,
		Local,
		Internet
	}

	public enum AddressType
	{
		None, LoopBack, Broadcast, Any
	}

	public enum Addressfam
	{
		Ipv4, Ipv6
	}



	public static class NetworkUtility
	{
		/// <summary>
		/// Проверить статус сети
		/// </summary>
		/// <param name="computer"></param>
		/// <returns></returns>
		public static ConnectionType GetNetworkStatus(Computer computer)
		{
			try
			{
				computer.IpAddress = GetIpAddress(Addressfam.Ipv4).ToString();
			}
			catch (Exception)
			{
				Debug.LogError("Нет соединения с сетью");
				return ConnectionType.Host;
			}

			if (HasInternetConnection())
			{
				return ConnectionType.Internet;
			}
			return ConnectionType.Local;
		}

		/// <summary>
		/// Проверить доступность сети
		/// </summary>
		/// <returns></returns>
		public static bool IsNetworkAvailable()
		{
			bool response = NetworkInterface.GetIsNetworkAvailable();
			return response;

		}

		/// <summary>
		/// Проверить соединение с Интернетом
		/// </summary>
		/// <returns></returns>
		public static bool HasInternetConnection()
		{
			Ping p = new Ping();
			PingReply pr = p.Send("8.8.8.8");
			if (pr != null)
			{
				IPStatus status = pr.Status;
				if (Convert.ToString(status) == "Success")
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Получить Ip адрес по форме
		/// </summary>
		/// <param name="addfam">Форма</param>
		/// <returns></returns>
		public static IPAddress GetIpAddress(Addressfam addfam)
		{
			//Return null if ADDRESSFAM is Ipv6 but Os does not support it
			if (addfam == Addressfam.Ipv6 && !Socket.OSSupportsIPv6)
			{
				return null;
			}

			IPAddress output = null;

			foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
			{

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

				NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
				NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

				if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) &&
					item.OperationalStatus == OperationalStatus.Up)
#endif
				{
					foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
					{
						//IPv4
						if (addfam == Addressfam.Ipv4)
						{
							if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
							{
								output = ip.Address;
							}
						}

						//IPv6
						else if (addfam == Addressfam.Ipv6)
						{
							if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
							{
								output = ip.Address;
							}
						}
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Получить Ip адрес по имени
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public static IPAddress GetIpAddress(string ipAddress)
		{
			return IPAddress.Parse(ipAddress);
		}

		/// <summary>
		/// Получить Ip адрес по форме и типу
		/// </summary>
		/// <param name="type">Тип</param>
		/// <param name="family">Форма</param>
		/// <returns></returns>
		public static IPAddress GetIpAddress(AddressType type, Addressfam family)
		{
			switch (type)
			{
				case AddressType.None:
					switch (family)
					{
						case Addressfam.Ipv4:
							return IPAddress.None;
						case Addressfam.Ipv6:
							return IPAddress.IPv6None;
					}
					break;
				case AddressType.LoopBack:
					switch (family)
					{
						case Addressfam.Ipv4:
							return IPAddress.Loopback;
						case Addressfam.Ipv6:
							return IPAddress.IPv6Loopback;
					}
					break;
				case AddressType.Broadcast:
					return IPAddress.Broadcast;
				case AddressType.Any:
					switch (family)
					{
						case Addressfam.Ipv4:
							return IPAddress.Any;
						case Addressfam.Ipv6:
							return IPAddress.IPv6Any;
					}
					break;
			}

			throw new Exception("Не найден адрес");
		}

		/// <summary>
		/// Получить маску подсети по Ip адресу
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public static IPAddress GetSubnetMask(IPAddress address)
		{
			foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
			{
				foreach (UnicastIPAddressInformation information in adapter.GetIPProperties().UnicastAddresses)
				{
					if (information.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						if (address.Equals(information.Address))
						{
							return information.IPv4Mask;
						}
					}
				}
			}
			throw new ArgumentException($"Не смогли найти подмаску сети для '{address}'");
		}

		/// <summary>
		/// Получить все сетевые интерфейсы
		/// </summary>
		/// <returns></returns>
		public static NetworkInterface[] GetNetworkInterfaces()
		{
			return NetworkInterface.GetAllNetworkInterfaces();
		}

		/// <summary>
		/// Получить все Mac адреса 
		/// </summary>
		public static List<string> GetMacAddresses()
		{
			List<string> macAddresses = new List<string>();

			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (nic.OperationalStatus == OperationalStatus.Up)
				{
					macAddresses.Add(nic.GetPhysicalAddress().ToString());
				}
			}

			return macAddresses;
		}

		/// <summary>
		/// Получить имя хоста по Dns имени
		/// </summary>
		/// <param name="dns">Dns имя</param>
		/// <returns></returns>
		public static string GetHostNameByDns(string dns)
		{
			IPHostEntry host = Dns.GetHostEntry(dns);
			return host.HostName;
		}

		/// <summary>
		/// Получить список Ip адресов по Dns имени
		/// </summary>
		/// <param name="dns">Dns имя</param>
		/// <returns></returns>
		public static List<IPAddress> GetIpAddressByDns(string dns)
		{
			IPHostEntry host = Dns.GetHostEntry(dns);
			return host.AddressList.ToList();
		}
	}
}