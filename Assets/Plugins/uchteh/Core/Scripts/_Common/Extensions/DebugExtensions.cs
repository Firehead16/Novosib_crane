using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение для дебага
	/// </summary>
	public static class DebugExtensions
	{
		public enum DebugColor
		{
			Red,
			Black,
			Yellow,
			Grey,
			Green,
			Blue,
			Magenta
		}

		public struct Line
		{
			public Color Color;
			public Ray Ray;
			public float Lenght;
		}

		public struct AngledSegment
		{
			public float Angle;
			public float Lenght;
		}


		public static void DrawGizmoLine(Line line)
		{
			Gizmos.color = line.Color;
			Gizmos.DrawLine(line.Ray.origin, line.Ray.origin + line.Ray.direction * line.Lenght);
		}

		public static void DrawDebugLine(Line line)
		{
			Debug.DrawLine(line.Ray.origin, line.Ray.origin + line.Ray.direction * line.Lenght, line.Color);
		}

		public static Ray GetTransformedLine(Line baseLine, params AngledSegment[] path)
		{
			var rotation = Quaternion.identity;
			var origin = baseLine.Ray.origin + baseLine.Ray.direction * baseLine.Lenght;
			if (path.Length > 0)
			{
				var angledSegment = path[0];
				rotation *= Quaternion.Euler(new Vector3(angledSegment.Angle, 0, 0));
				for (var i = 1; i < path.Length; i++)
				{
					angledSegment = path[i];
					origin += angledSegment.Lenght * (rotation * baseLine.Ray.direction).normalized;
					rotation *= Quaternion.Euler(new Vector3(angledSegment.Angle, 0, 0));
				}
			}
			return new Ray(origin, (rotation * baseLine.Ray.direction).normalized);
		}



		public static void DebugLog(this MonoBehaviour gameObject, string message)
		{
			Log(message, DebugColor.Black, gameObject);
		}

		public static void DebugLogError(this MonoBehaviour gameObject, string message)
		{
			Log(message, DebugColor.Red, gameObject);
		}

		public static void DebugLoad(this MonoBehaviour gameObject)
		{
			Log("Loading " + gameObject.GetType(), DebugColor.Blue, gameObject);
		}

		public static void DebugInitialize(this MonoBehaviour gameObject)
		{
			Log("Initializing " + gameObject.GetType(), DebugColor.Green, gameObject);
		}

		public static void DebugOnItemsInitialised(this MonoBehaviour gameObject)
		{
			Log("ItemsInitialised " + gameObject.GetType(), DebugColor.Magenta);
		}

		public static void DebugUnload(this MonoBehaviour gameObject)
		{
			Log("Start Unloading" + gameObject.GetType(), DebugColor.Black, gameObject);
		}

		public static void DebugNotify(this MonoBehaviour gameObject, object message)
		{
			Log(gameObject.GetType() + "Notify(" + message + ")", DebugColor.Yellow, gameObject);
		}

		private static void Log(string message, DebugColor color, Object context = null)
		{
			if (IsNeedDebug())
			{
				string colorName = color.ToString().ToLower();
				Debug.Log($"<color={colorName}>" + message + "</color>", context);
			}
		}

		private static bool IsNeedDebug()
		{
			return SettingsStorage.Settings.Debuging;
		}
	}
}