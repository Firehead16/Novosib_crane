using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace Core.Global.Network
{
	public class DatabaseSqLite : Database
	{
		public string Database;

		public string Version;

		private string projectPath;


		/// <summary>
		/// Строка подключения
		/// </summary>
		public override string ConnectionString
		{
			get
			{
				string connString = "Data Source = " + projectPath + "/" + Database + "; Version = " + Version + ";";
				return connString;
			}

		}


		private void Start()
		{
			projectPath = Application.dataPath;
		}


		public void CreateDataBase(string path)
		{
			if (!File.Exists(path))
			{
				SQLiteConnection.CreateFile(path);
			}
		}

		public override bool IsAvailable()
		{
			bool isOpen = true;
			using (SQLiteConnection tempConnectionPresent = new SQLiteConnection(ConnectionString))
			{
				try
				{
					Debug.Log("Проверяю связь");
					tempConnectionPresent.Open();

					SQLiteCommand cmd = tempConnectionPresent.CreateCommand();
					cmd.CommandText = "SELECT * FROM Sessions";
					cmd.ExecuteNonQuery();

					ConnectionState conState = tempConnectionPresent.State;

					if (conState == ConnectionState.Closed || conState == ConnectionState.Closed)
					{
						isOpen = false;
					}
				}
				catch (Exception exc)
				{
					isOpen = false;
					Debug.LogError(exc.Message);
				}

				return isOpen;
			}
		}

		public override int SendExecuteScalarWithLastId(string command)
		{
			int response = 0;

			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();

				using (SQLiteCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = "PRAGMA foreign_keys = ON;" + command;

					try
					{
						cmd.ExecuteScalar();
						response = (int)connection.LastInsertRowId;
					}
					catch (Exception exc)
					{
						Debug.LogError(exc.Message);
					}
				}

				connection.Close();
			}

			return response;
		}

		public override object SendExecuteScalar(string command)
		{
			object response = null;

			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();

				using (SQLiteCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = "PRAGMA foreign_keys = ON;" + command;

					try
					{
						response = cmd.ExecuteScalar();
					}
					catch (Exception exc)
					{
						Debug.LogError(exc.Message);
					}
				}

				connection.Close();
			}

			return response;
		}

		public override int SendExecuteNonQuery(string command)
		{
			int response = 0;

			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();

				using (SQLiteCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = "PRAGMA foreign_keys = ON;" + command;

					try
					{
						response = cmd.ExecuteNonQuery();
					}
					catch (Exception exc)
					{
						Debug.Log(exc.Message);
					}
				}

				connection.Close();
			}

			return response;
		}

	}
}