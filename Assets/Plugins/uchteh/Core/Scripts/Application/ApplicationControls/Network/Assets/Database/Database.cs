using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;

namespace Core.Global.Network
{
	/// <summary>
	/// Базовый класс базы данных
	/// </summary>
	public abstract class Database : BaseControlMethods
	{
		[ShowInInspector,ReadOnly]
		public List<IRequest> AvailableRequests = new List<IRequest>();

		public string Server;

		/// <summary>
		/// Строка подключения
		/// </summary>
		[UsedImplicitly]
		public virtual string ConnectionString { get; }

		public override void Load()
		{
			foreach (var iRequest in GetComponents<IRequest>())
			{
				AvailableRequests.Add(iRequest);
				iRequest.Load();
			}

		}

		public override void Unload()
		{
			AvailableRequests.Clear();
		}

		/// <summary>
		/// Проверка доступности базы данных
		/// </summary>
		/// <returns></returns>
		public abstract bool IsAvailable();

		/// <summary>
		/// Вернуть ключ последней строки
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public abstract int SendExecuteScalarWithLastId(string command);

		/// <summary>
		/// Отправить запрос и вернуть список объектов базы данных
		/// </summary>
		/// <param name="command">Команда</param>
		/// <returns></returns>
		public abstract object SendExecuteScalar(string command);

		/// <summary>
		/// Отправить запрос и вернуть список строк
		/// </summary>
		/// <param name="command">Команда</param>
		/// <returns></returns>
		public abstract int SendExecuteNonQuery(string command);
	}
}