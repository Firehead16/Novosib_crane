
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение для математики
	/// </summary>
	public static class MathExtensions
	{
		/// <summary>
		/// Функция для добавления изогнутого смещения в сторону 1 для значения в диапазоне 0-1
		/// </summary>
		/// <param name="factor">Значение диапазон</param>
		/// <returns>Смещение</returns>
		public static float CurveFactor(float factor)
		{
			return 1 - (1 - factor) * (1 - factor);
		}

		/// <summary>
		/// Незакрепленная версия Lerp, чтобы значение превышало диапазон 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float ULerp(float from, float to, float value)
		{
			return (1.0f - value) * from + value * to;
		}

		/// <summary>
		/// Получить рандомный список указанного количества
		/// </summary>
		/// <param name="elementCount">Количество чисел</param>
		/// <returns>Рандомный список чисел</returns>
		public static List<int> RandomizeOrder(int elementCount)
		{
			var result = new List<int>();

			for (var i = 0; i < elementCount; i++)
			{
				var uniqueInt = UnityEngine.Random.Range(0, elementCount);
				while (result.Exists(member => member == uniqueInt))
				{
					uniqueInt = UnityEngine.Random.Range(0, elementCount);
				}
				result.Add(uniqueInt);
			}
			return result;
		}

		/// <summary>
		/// Сделать симметричную кривую
		/// </summary>
		/// <param name="startRelCenter"></param>
		/// <param name="endRelCenter"></param>
		/// <param name="centerPoint"></param>
		/// <param name="isClockwise"></param>
		/// <returns></returns>
		public static List<Vector2> MakeSimetricCurve(Vector3 startRelCenter, Vector3 endRelCenter, Vector3 centerPoint, bool isClockwise)
		{
			float k = 0.02f;

			var a = startRelCenter - centerPoint;
			var b = endRelCenter - centerPoint;
			var x = endRelCenter - startRelCenter;
			var r = a.magnitude;
			var j = Vector2.Perpendicular(a).normalized;
			var i = (Vector2)a.normalized;

			var sector = GetAngle(a.magnitude, b.magnitude, x.magnitude);
			var points = new List<Vector2>();

			if (Vector3.Dot(x, j) > 0)
				sector = 360 - sector;
			if (isClockwise)
			{
				sector = 360 - sector;
			}
			sector *= Mathf.Deg2Rad;
			var delta = 1 / (k * r);
			int count = (int)(sector / delta);
			delta = sector / count;
			if (!isClockwise) delta = -delta;
			{
				points.Add(startRelCenter);
				for (float alfa = delta; Mathf.Abs(alfa) < sector; alfa += delta)
				{
					var x2 = r * Mathf.Cos(alfa) * i + r * Mathf.Sin(alfa) * j;
					points.Add((Vector2)centerPoint + x2);
				}
				points.Add(endRelCenter);
			}
			return points;
		}

		/// <summary>
		/// Получить угол из трех координат
		/// </summary>
		/// <param name="a">Координата A</param>
		/// <param name="b">Координата B</param>
		/// <param name="c">Координата C</param>
		/// <returns></returns>
		public static float GetAngle(float a, float b, float c)
		{
			float alpha = (Mathf.Acos((+a * a + b * b - c * c) / (2 * a * b))) * 180 / Mathf.PI;
			return alpha;
		}

		/// <summary>
		/// Привести строку в вещественное число
		/// </summary>
		/// <param name="text">Текст</param>
		/// <returns>Число</returns>
		public static float? StringToFloat(string text)
		{
			NumberFormatInfo provider = new NumberFormatInfo
			{
				NumberDecimalSeparator = "."
			};

			try
			{
				float result = (float)Convert.ToDouble(text, provider);
				return result;
			}
			catch (Exception)
			{
				Debug.LogError("Не могу привести текст к вещественному числу");
			}

			return null;
		}
	}

}