using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class TransformExtensions
{
	public enum Axis
	{
		XPlus,
		YPlus,
		ZPlus,
		XMinus,
		YMinus,
		ZMinus
	}

	/// <summary>
	/// Получить глобальное направление по оси
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="axis"></param>
	/// <returns></returns>
	public static Vector3 GetWorldDirection(this Transform transform, Axis axis)
	{
		switch (axis)
		{
			case Axis.XPlus:
				return transform.right;
			case Axis.YPlus:
				return transform.up;
			case Axis.ZPlus:
				return transform.forward;
			case Axis.XMinus:
				return -transform.right;
			case Axis.YMinus:
				return -transform.up;
			case Axis.ZMinus:
				return -transform.forward;
			default:
				return transform.up;
		}
	}

	/// <summary>
	/// Получить локальное направление по оси
	/// </summary>
	/// <param name="axis"></param>
	/// <returns></returns>
	public static Vector3 GetLocalDirection(Axis axis)
	{
		switch (axis)
		{
			case Axis.XPlus:
				return Vector3.right;
			case Axis.YPlus:
				return Vector3.up;
			case Axis.ZPlus:
				return Vector3.forward;
			case Axis.XMinus:
				return Vector3.left;
			case Axis.YMinus:
				return Vector3.down;
			case Axis.ZMinus:
				return Vector3.back;
			default:
				return Vector3.up;
		}
	}

	/// <summary>
	/// Уничтожение дочерних объектов на Transform
	/// </summary>
	/// <param name="given"></param>
	/// <returns></returns>
	public static void KillAllChildrens(this Transform given)
	{
		int count = given.childCount;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy(given.GetChild(i).gameObject);
		}
	}


	/// <summary>
	/// Получить общую массу компонентов Rigidbody у объекта given
	/// </summary>
	/// <param name="transform"></param>
	/// <returns></returns>
	public static float GetMassofRigidbodies(this Transform transform)
	{
		float mass = 0;
		foreach (Rigidbody part in transform.GetComponents<Rigidbody>())
		{
			mass += part.mass;
		}
		return mass;
	}

	/// <summary>
	/// Получить центр масс компонентов Rigidbody у объекта given
	/// </summary>
	/// <param name="transform"></param>
	/// <returns></returns>
	public static Vector3 GetCoMofRigidbodies(this Transform transform)
	{
		Vector3 coM = Vector3.zero;
		int count = 0;
		foreach (Rigidbody part in transform.GetComponents<Rigidbody>())
		{
			coM += part.centerOfMass * part.mass;
			count++;
		}

		return coM / count;
	}

	/// <summary>
	/// Получает центр масс компонента Renderer (более точный, чем Transform.position), если нет рендерера, то берет у SkinnedMeshRenderer. Помечает центр масс голубым лучом.
	/// </summary>
	/// <param name="transform"></param>
	/// <returns></returns>
	public static Vector3 GetCoMofRenderer(this Transform transform)
	{
		Vector3 position = new Vector3();
		try
		{
			position = transform.GetComponent<Renderer>().bounds.center;
		}
		catch
		{
			try
			{
				position = transform.GetComponent<SkinnedMeshRenderer>().bounds.center;
			}
			catch (Exception e)
			{
				Debug.LogError(e + "На объекте " + transform.name + " нет меша");
			}
		}
		Debug.DrawLine(position, position + Vector3.right, Color.blue);
		return position;
	}

	/// <summary>
	/// Копировать позиции и поворот трансформа
	/// </summary>
	/// <param name="to"></param>
	/// <param name="from"></param>
	public static void CopyPositionAndRotation(this Transform to, Transform from)
	{
		to.position = from.position;
		to.rotation = from.rotation;
	}
}

