using UnityEngine;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение для RectTransform
	/// </summary>
	public static class RectTransformExtensions
	{
		/// <summary>
		///Преобразует позиции якорей первого трансформа во второй трансформ,
		///с учетом смещения, привязки и поворота, и возвращает новые позиции якорей
		/// </summary>
		/// <param name="from">Трансформ</param>
		/// <param name="to">Трансформ для копирования</param>
		/// <returns></returns>
		public static Vector2 SwitchToRectTransform(this RectTransform from, RectTransform to)
		{
			Vector2 localPoint;
			Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
			Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
			screenP += fromPivotDerivedOffset;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
			Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
			return to.anchoredPosition + localPoint - pivotDerivedOffset;
		}

		/// <summary>
		/// Установить масштаб по-умолчанию (1,1,1)
		/// </summary>
		/// <param name="trans">Трансформ</param>
		public static void SetDefaultScale(this RectTransform trans)
		{
			trans.localScale = new Vector3(1, 1, 1);
		}

		/// <summary>
		/// Установить минимальное и максимальное смещение по-умолчанию
		/// </summary>
		/// <param name="trans">Трансформ</param>
		public static void SetDefaultStretch(this RectTransform trans)
		{
			trans.offsetMin = new Vector2(0, 0);
			trans.offsetMax = new Vector2(0, 0);
		}

		/// <summary>
		/// Установить пивот и якоря в заданный вектор
		/// </summary>
		/// <param name="trans">Трансформ</param>
		/// <param name="vector2">Вектор</param>
		public static void SetPivotAndAnchors(this RectTransform trans, Vector2 vector2)
		{
			trans.pivot = vector2;
			trans.anchorMin = vector2;
			trans.anchorMax = vector2;
		}

		/// <summary>
		/// Получить размер трансформа
		/// </summary>
		/// <param name="trans">Трансформ</param>
		/// <returns>Размер трансформа</returns>
		public static Vector2 GetSize(this RectTransform trans)
		{
			return trans.rect.size;
		}

		/// <summary>
		/// Получить ширину
		/// </summary>
		/// <param name="trans">Трансформ</param>
		/// <returns>Ширина</returns>
		public static float GetWidth(this RectTransform trans)
		{
			return trans.rect.width;
		}

		/// <summary>
		/// Получить высоту
		/// </summary>
		/// <param name="trans">Трансформ</param>
		/// <returns>Высота</returns>
		public static float GetHeight(this RectTransform trans)
		{
			return trans.rect.height;
		}

		/// <summary>
		/// Установить в позицию относительно пивота
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="newPos"></param>
		public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
		}

		/// <summary>
		/// Установить в левую нижнюю позицию
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="newPos"></param>
		public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
		}

		/// <summary>
		/// Установить в левую верхнюю позицию
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="newPos"></param>
		public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
		}

		/// <summary>
		/// Установить в правую нижнюю позицию
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="newPos"></param>
		public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
		}

		/// <summary>
		/// Установить в правую верхнюю позицию
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="newPos"></param>
		public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
		}

		/// <summary>
		/// Установить размер
		/// </summary>
		/// <param name="trans">Транформ</param>
		/// <param name="newSize">Размер</param>
		public static void SetSize(this RectTransform trans, Vector2 newSize)
		{
			Vector2 oldSize = trans.rect.size;
			Vector2 deltaSize = newSize - oldSize;
			trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
			trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
		}

		/// <summary>
		/// Установить ширину
		/// </summary>
		/// <param name="trans">Трансформ</param>
		/// <param name="newSize">Высота</param>
		public static void SetWidth(this RectTransform trans, float newSize)
		{
			SetSize(trans, new Vector2(newSize, trans.rect.size.y));
		}

		/// <summary>
		/// Установить высоту
		/// </summary>
		/// <param name="trans">Трансформ</param>
		/// <param name="newSize">Высота</param>
		public static void SetHeight(this RectTransform trans, float newSize)
		{
			SetSize(trans, new Vector2(trans.rect.size.x, newSize));
		}
	}
}
