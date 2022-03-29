using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение для сериализации в байты
	/// </summary>
	public static class BinaryExtension
	{
		/// <summary>
		/// Сериализовать объект в байты
		/// </summary>
		/// <param name="customObject">Объект</param>
		/// <returns></returns>
		public static byte[] SerializeToBytes(object customObject)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, customObject);
				byte[] bytes = stream.ToArray();
				return bytes;
			}
		}

		/// <summary>
		/// Десериализовать объект из байтов
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		/// <param name="bytes">Массив байтов</param>
		/// <returns></returns>
		public static T DeserializeToObject<T>(byte[] bytes)
		{
			using (Stream stream = new MemoryStream(bytes))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				var customObject = formatter.Deserialize(stream);
				T currentObject = (T)customObject;
				return currentObject;
			}
		}

		/// <summary>
		/// Получить количество байтов в объекте
		/// </summary>
		/// <param name="customObject">Объект</param>
		/// <returns></returns>
		/// <remarks>Возвращает ноль при ошибке</remarks>
		public static long GetByteSize(object customObject)
		{
			long size;

			try
			{
				using (Stream stream = new MemoryStream())
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, customObject);
					size = stream.Length;
				}
			}
			catch (Exception exc)
			{
				Debug.LogError(exc.Message);
				return 0;
			}

			return size;
		}

		/// <summary>
		/// Разделяет объект на массивы с указанным количеством
		/// </summary>
		/// <param name="customObject">Объект</param>
		/// <param name="partLength">Количество байт в массиве</param>
		/// <returns></returns>
		public static List<byte[]> SplitByBytes(object customObject, int partLength)
		{
			long size = GetByteSize(customObject);
			byte[] bytes = new byte[size];

			using (Stream stream = new MemoryStream(bytes))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, customObject);
			}

			int partCount = (int)Math.Ceiling(bytes.Length / (double)partLength);

			if (bytes.Length < partLength) return new List<byte[]> { bytes };

			List<byte[]> byteParties = new List<byte[]>();
			for (int i = 0; i < partCount; i++)
			{
				var part = bytes.Skip(i * partLength).Take(partLength).ToArray();
				byteParties.Add(part);
			}

			return byteParties;
		}

		/// <summary>
		/// Объединить массивы байтов в один
		/// </summary>
		/// <param name="bytes">Массивы байтов</param>
		/// <returns></returns>
		public static object MergeByBytes(List<byte[]> bytes)
		{
			List<byte> byteFull = new List<byte>();

			foreach (var bytePart in bytes)
			{
				byteFull.AddRange(bytePart);
			}

			return byteFull.ToArray();
		}
	} 
}