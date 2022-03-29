#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class FindMissingScriptsRecursivelyMenu : EditorWindow
{
	static int goCount = 0, componentsCount = 0, missingCount = 0;

	[MenuItem("Window/FindMissingScriptsRecursively")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScriptsRecursivelyMenu));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
		{
			FindInSelected();
		}
	}

	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		goCount = 0;
		componentsCount = 0;
		missingCount = 0;
		foreach (GameObject g in go)
		{
			FindInGo(g);
		}
		Debug.Log($"Searched {goCount} GameObjects, {componentsCount} components, found {missingCount} missing");
	}

	private static void FindInGo(GameObject g)
	{
		goCount++;
		Component[] components = g.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			componentsCount++;
			if (components[i] == null)
			{
				missingCount++;
				string s = g.name;
				Transform t = g.transform;
				while (t.parent != null)
				{
					s = t.parent.name + "/" + s;
					t = t.parent;
				}
				Debug.Log(s + " has an empty script attached in position: " + i, g);
			}
		}

		foreach (Transform childT in g.transform)
		{
			FindInGo(childT.gameObject);
		}
	}
}

#endif