using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR

using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SceneObjectMenu
{
	#region Меню векторов трансформа

	[MenuItem("Tools/Gameobject/TransformCopyData/GetWorldPosition")]
	public static void GetWorldPosition()
	{
		EditorGUIUtility.systemCopyBuffer = Selection.activeTransform.position.ToString();
		Debug.Log(Selection.activeTransform.position.x.ToString("#0.###"));
		Debug.Log(Selection.activeTransform.position.y.ToString("#0.###"));
		Debug.Log(Selection.activeTransform.position.z.ToString("#0.###"));
	}

	[MenuItem("Tools/Gameobject/TransformCopyData/GetForwardVector")]
	public static void GetForwardVector()
	{
		EditorGUIUtility.systemCopyBuffer = Selection.activeTransform.forward.ToString();
		Debug.Log(Selection.activeTransform.forward.x.ToString("#0.###"));
		Debug.Log(Selection.activeTransform.forward.y.ToString("#0.###"));
		Debug.Log(Selection.activeTransform.forward.z.ToString("#0.###"));
	}

	[MenuItem("Tools/Gameobject/TransformCopyData/GetRightVector")]
	public static void GetRightVector()
	{

		EditorGUIUtility.systemCopyBuffer = Selection.activeTransform.right.ToString();
		Debug.Log(Selection.activeTransform.right.x.ToString("#0.###"));
		Debug.Log(Selection.activeTransform.right.y.ToString("#0.###"));
		Debug.Log(Selection.activeTransform.right.z.ToString("#0.###"));
	}

	[MenuItem("Tools/Gameobject/TransformCopyData/GetUpVector")]
	public static void GetUpVector()
	{
		EditorGUIUtility.systemCopyBuffer = Selection.activeTransform.up.ToString();
	}

	[MenuItem("Tools/Gameobject/TransformCopyData/GetBackVector")]
	public static void GetBackVector()
	{
		EditorGUIUtility.systemCopyBuffer = (-Selection.activeTransform.forward).ToString();
	}

	[MenuItem("Tools/Gameobject/TransformCopyData/GetLeftVector")]
	public static void GetLeftVector()
	{
		EditorGUIUtility.systemCopyBuffer = (-Selection.activeTransform.right).ToString();
	}

	[MenuItem("Tools/Gameobject/TransformCopyData/GetDownVector")]
	public static void GetDownVector()
	{
		EditorGUIUtility.systemCopyBuffer = (-Selection.activeTransform.up).ToString();
	}

	#endregion

	#region Очистка объекта 

	[MenuItem("Tools/Gameobject/CleanUpObject/CleanUpObjectWhithMesh")]
	public static void CleanUpObjectWhithMesh()
	{
		GameObject objectToClean = Selection.activeTransform.gameObject;
		objectToClean.CleanUpObjectWhithMesh();
	}


	private static void CleanUpObjectWhithMesh(this GameObject objectToClean)
	{
		int checkCount = 5;
		bool needCheck;
		do
		{
			checkCount--;
			needCheck = false;
			Component[] components = objectToClean.GetComponentsInChildren<Component>(true);
			foreach (var component in components)
			{
				if (!(component is Transform || component is MeshFilter || component is Renderer))
				{
					needCheck = true;

					if (component != null)
					{
						var parentComponent = component.gameObject.GetRequaredAtributedComponents(component.GetType());
						foreach (var pComponent in parentComponent)
						{
							Object.DestroyImmediate(pComponent);
						}
					}
				}
			}
			components = objectToClean.GetComponentsInChildren<Component>(true);
			foreach (var component in components)
			{
				if (!(component is Transform || component is MeshFilter || component is Renderer))
				{
					Object.DestroyImmediate(component);
				}
			}

		} while (needCheck && checkCount > 0);

		if (checkCount == 0)
			Debug.LogError("Зависимые объекты не были удалены, так как checkcount = 0", objectToClean);
	}

	private static HashSet<Component> GetRequaredAtributedComponents(this GameObject go, Type t)
	{
		return go.GetComponents<Component>().Where(c => Requires(c.GetType(), t)).ToHashSet();
	}

	private static bool Requires(Type obj, Type requirement)
	{
		//also check for m_Type1 and m_Type2 if required
		return Attribute.IsDefined(obj, typeof(RequireComponent)) &&
			   Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
				   .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
	}

	#endregion

	[MenuItem("Tools/Gameobject/Anchors to Corners %[")]
	static void AnchorsToCorners()
	{
		if (Selection.transforms == null || Selection.transforms.Length == 0)
		{
			return;
		}
		Undo.IncrementCurrentGroup();
		Undo.SetCurrentGroupName("AnchorsToCorners");
		var undoGroup = Undo.GetCurrentGroup();

		foreach (Transform transform in Selection.transforms)
		{
			RectTransform t = transform as RectTransform;
			Undo.RecordObject(t, "AnchorsToCorners");
			RectTransform pt = Selection.activeTransform.parent as RectTransform;

			if (t == null || pt == null) return;

			Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
												t.anchorMin.y + t.offsetMin.y / pt.rect.height);
			Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
												t.anchorMax.y + t.offsetMax.y / pt.rect.height);

			t.anchorMin = newAnchorsMin;
			t.anchorMax = newAnchorsMax;
			t.offsetMin = t.offsetMax = new Vector2(0, 0);
		}
		Undo.CollapseUndoOperations(undoGroup);
	}

	[MenuItem("Tools/Gameobject/Corners to Anchors %]")]
	static void CornersToAnchors()
	{
		if (Selection.transforms == null || Selection.transforms.Length == 0)
		{
			return;
		}
		Undo.IncrementCurrentGroup();
		Undo.SetCurrentGroupName("CornersToAnchors");
		var undoGroup = Undo.GetCurrentGroup();

		foreach (Transform transform in Selection.transforms)
		{
			RectTransform t = transform as RectTransform;
			Undo.RecordObject(t, "CornersToAnchors");

			if (t == null) return;

			t.offsetMin = t.offsetMax = new Vector2(0, 0);
		}
		Undo.CollapseUndoOperations(undoGroup);
	}

	[MenuItem("Tools/Gameobject/Mirror Horizontally Around Anchors %;")]
	static void MirrorHorizontallyAnchors()
	{
		MirrorHorizontally(false);
	}

	[MenuItem("Tools/Gameobject/Mirror Horizontally Around Parent Center %:")]
	static void MirrorHorizontallyParent()
	{
		MirrorHorizontally(true);
	}

	static void MirrorHorizontally(bool mirrorAnchors)
	{
		foreach (Transform transform in Selection.transforms)
		{
			RectTransform t = transform as RectTransform;
			RectTransform pt = Selection.activeTransform.parent as RectTransform;

			if (t == null || pt == null) return;

			if (mirrorAnchors)
			{
				Vector2 oldAnchorMin = t.anchorMin;
				t.anchorMin = new Vector2(1 - t.anchorMax.x, t.anchorMin.y);
				t.anchorMax = new Vector2(1 - oldAnchorMin.x, t.anchorMax.y);
			}

			Vector2 oldOffsetMin = t.offsetMin;
			t.offsetMin = new Vector2(-t.offsetMax.x, t.offsetMin.y);
			t.offsetMax = new Vector2(-oldOffsetMin.x, t.offsetMax.y);

			t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
		}
	}

	[MenuItem("Tools/Gameobject/Mirror Vertically Around Anchors %'")]
	static void MirrorVerticallyAnchors()
	{
		MirrorVertically(false);
	}

	[MenuItem("Tools/Gameobject/Mirror Vertically Around Parent Center %\"")]
	static void MirrorVerticallyParent()
	{
		MirrorVertically(true);
	}

	static void MirrorVertically(bool mirrorAnchors)
	{
		foreach (Transform transform in Selection.transforms)
		{
			RectTransform t = transform as RectTransform;
			RectTransform pt = Selection.activeTransform.parent as RectTransform;

			if (t == null || pt == null) return;

			if (mirrorAnchors)
			{
				Vector2 oldAnchorMin = t.anchorMin;
				t.anchorMin = new Vector2(t.anchorMin.x, 1 - t.anchorMax.y);
				t.anchorMax = new Vector2(t.anchorMax.x, 1 - oldAnchorMin.y);
			}

			Vector2 oldOffsetMin = t.offsetMin;
			t.offsetMin = new Vector2(t.offsetMin.x, -t.offsetMax.y);
			t.offsetMax = new Vector2(t.offsetMax.x, -oldOffsetMin.y);

			t.localScale = new Vector3(t.localScale.x, -t.localScale.y, t.localScale.z);
		}
	}

}

#endif