using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using UnityEngine;

namespace Core.Global.Network
{
	/// <summary>
	/// Подключение для клиента на том же компьютере
	/// </summary>
	public class DatabaseNetworkConnection : ServerNetworkConnection
	{
		private List<Database> databases = new List<Database>();

		public T GetRequest<T>() where T : class, IRequest
		{
			Database availableDatabases = null;

			foreach (var database in databases)
			{
				if (database.AvailableRequests.Exists(r => r is T))
				{
					availableDatabases = database;
					break;
				}
			}

			if (availableDatabases == null)
			{
				throw new Exception("Не найдена база для запроса");
			}

			return (T)availableDatabases.AvailableRequests.Single(r => r is T);
		}

		public override void Open()
		{
			var serverSettings = ServerNetworkControlSettings.Default();

			if (serverSettings.DatabaseConfigs.Any())
			{
				// Инициализируем базы данных
				foreach (var databaseConfig in serverSettings.DatabaseConfigs)
				{
					if (databaseConfig.IsNeedLoad)
					{
						var database = Instantiate(databaseConfig.Database, Vector3.zero, Quaternion.identity, transform);
						database.Load();
						databases.Add(database);
					}
				}
			}
			else
			{
				throw new Exception("Не установлены хранилища данных");
			}
		}

		public override void Close()
		{
			foreach (var database in databases)
			{
				Destroy(database);
			}
			databases.Clear();
		}

		public override void CheckConnection()
		{

		}
	}
}