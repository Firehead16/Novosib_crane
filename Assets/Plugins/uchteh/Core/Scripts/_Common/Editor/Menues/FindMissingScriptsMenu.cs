#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class FindMissingScriptsMenu : EditorWindow
{
	[MenuItem("Tools/Gameobject/FindMissingScripts")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScriptsMenu));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected prefabs"))
		{
			FindInSelected();
		}
	}
	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		int goCount = 0, componentsCount = 0, missingCount = 0;
		foreach (GameObject g in go)
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
		}

		Debug.Log($"Searched {goCount} GameObjects, {componentsCount} components, found {missingCount} missing");
	}
}

#endif