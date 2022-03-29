using System;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

namespace Core.Global.Network
{
	public class DatabaseMsSql : Database
	{
		public string Database;

		public string Username;

		public string Password;

		public bool PersistSecurityInfo;

		public bool MultipleActiveResultSets;

		public SqlConnection Connection;

		/// <summary>
		/// Строка подключения
		/// </summary>
		public override string ConnectionString
		{
			get
			{
				string connString = @"Server=" + Server + ";"
									+ "Database = " + Database + ";"
									+ "User ID = " + Username + ";"
									+ "Password = " + Password + ";"
									+ "Persist Security Info = " + (PersistSecurityInfo ? "True" : "False") + ";"
									+ "MultipleActiveResultSets = " + (MultipleActiveResultSets ? "True" : "False") + ";";
				return connString;
			}

		}

		public override bool IsAvailable()
		{
			bool isOpen = true;
			using (SqlConnection tempConnection = new SqlConnection(ConnectionString))
			{
				try
				{
					tempConnection.Open();

					SqlCommand cmd = tempConnection.CreateCommand();
					cmd.CommandText = "DBCC CHECKDB";
					cmd.ExecuteNonQuery();

					ConnectionState conState = tempConnection.State;

					if (conState == ConnectionState.Closed || conState == ConnectionState.Closed)
					{
						isOpen = false;
					}
				}
				catch
				{
					isOpen = false;
				}

				return isOpen;
			}
		}

		public override int SendExecuteScalarWithLastId(string command)
		{
			return SendExecuteNonQuery(command + " SELECT CAST(SCOPE_IDENTITY() as int);");
		}

		public override object SendExecuteScalar(string command)
		{
			object response = null;

			using (Connection = new SqlConnection(ConnectionString))
			{
				Connection.Open();

				SqlTransaction transaction = Connection.BeginTransaction();

				SqlCommand cmd = Connection.CreateCommand();
				cmd.CommandText = command;
				cmd.Transaction = transaction;
				try
				{
					response = cmd.ExecuteScalar();
					transaction.Commit();
				}

				catch (Exception exc)
				{
					transaction.Rollback();
					Debug.Log(exc.Message);
				}
			}

			return response;
		}

		public override int SendExecuteNonQuery(string command)
		{
			int response = 0;

			using (Connection = new SqlConnection(ConnectionString))
			{
				SqlTransaction transaction;
				SqlCommand cmd;
				try
				{
					Connection.Open();
					transaction = Connection.BeginTransaction();
					cmd = Connection.CreateCommand();

				}
				catch (Exception e)
				{
					Debug.Log(e);
					return response;
				}
				cmd.CommandText = command;
				cmd.Transaction = transaction;
				try
				{
					response = cmd.ExecuteNonQuery();
					transaction.Commit();
				}

				catch (Exception exc)
				{
					transaction.Rollback();
					Debug.Log(exc.Message);
				}
			}

			return response;
		}
	}
}
