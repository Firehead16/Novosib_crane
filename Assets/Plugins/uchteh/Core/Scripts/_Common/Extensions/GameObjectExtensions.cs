using System;
using System.Reflection;
using UnityEngine;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение GameObject для Unity
	/// </summary>
	public static class GameObjectExtensions
	{
		/// <summary>
		/// Получить или создать и получить компонент
		/// </summary>
		/// <typeparam name="T">Тип компонента</typeparam>
		/// <param name="gameObject">Объект</param>
		/// <returns>Компонент указанного типа</returns>
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T result = gameObject.GetComponent<T>();
			if (result == null)
			{
				result = gameObject.AddComponent<T>();
			}
			return result;
		}

		/// <summary>
		/// Копировать компонент на другой объект
		/// </summary>
		/// <typeparam name="T">Тип компонента</typeparam>
		/// <param name="component">Компонент</param>
		/// <param name="gameObject">Объект для копирования</param>
		/// <returns>Скопированный компонент указанного типа</returns>
		public static T CopyComponent<T>(this GameObject gameObject, T component) where T : Component
		{
			Type type = component.GetType();
			Component copy = gameObject.AddComponent(type);

			FieldInfo[] fields = type.GetFields();

			foreach (var field in fields)
			{
				field.SetValue(copy, field.GetValue(component));
			}
            PropertyInfo[] propertys = type.GetProperties();

            foreach (var property in propertys)
            {
                if (property.CanWrite&& property.Name.ToLower() != "name") property.SetValue(copy, property.GetValue(component));
            }

            return copy as T;
		}

		/// <summary>
		/// Найти компонент на родителе объекта
		/// </summary>
		/// <typeparam name="T">Тип компонента</typeparam>
		/// <param name="gameObject">Объект</param>
		/// <returns>Компонент указанного типа или null</returns>
		public static T FindComponentInParent<T>(this GameObject gameObject) where T : MonoBehaviour
		{
			var parent = gameObject.transform.parent;

			if (parent)
			{
				var findComponent = gameObject.transform.parent.GetComponent<T>();

				if (findComponent)
				{
					return findComponent;
				}
			    parent.gameObject.FindComponentInParent<T>();
			}

			return null;
		}
	}

}