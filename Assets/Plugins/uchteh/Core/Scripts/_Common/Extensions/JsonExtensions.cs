using System;
using UnityEngine;


namespace Core.Extensions
{
	/// <summary>
	/// Расширение для JsonUtility
	/// </summary>
	public static class JsonExtensions
	{
		/// <summary>
		/// Десериализация Json в массив объектов
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		/// <param name="json">Строка Json</param>
		/// <returns>Массив объектов</returns>
		public static T[] FromJson<T>(string json)
		{
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		/// <summary>
		/// Сериализация массива объектов в Json
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		/// <param name="array">Массив объектов</param>
		/// <returns>Строка Json</returns>
		public static string ToJson<T>(T[] array)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper);
		}

		/// <summary>
		/// Сериализация массива объектов в Json
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		/// <param name="array">Массив объектов</param>
		/// <param name="prettyPrint">Использовать форматирование</param>
		/// <returns>Строка Json</returns>
		public static string ToJson<T>(T[] array, bool prettyPrint)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper, prettyPrint);
		}

		/// <summary>
		/// Хранилище массива объектов
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		[SerializeField]
		private class Wrapper<T>
		{
			public T[] Items;
		}
	}

}