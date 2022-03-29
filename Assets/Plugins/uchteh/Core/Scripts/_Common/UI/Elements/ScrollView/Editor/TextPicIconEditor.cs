#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using Core.Ui.Extensions;

namespace Core.Ui
{

	public class TextPicIconEditor : EditorWindow {
		[MenuItem("Window/UI/Extensions/TextPic Edit Icons")]
		protected static void ShowTextPicIconEditor() {
			var wnd = GetWindow<TextPicIconEditor>();
			wnd.titleContent.text = "Edit Icons in TextPic";
			wnd.Show();
		}

		private GameObject o;

		private static int columnWidth = 300;

		private string iconName;
		private Sprite icon;

		public void Swap(GameObject o) {
			Debug.Log("Editing icons for " + o.name);


			TextPic[] children = o.GetComponentsInChildren<TextPic>(true);
			for(int i = 0; i < children.Length; i++) {
				if (children[i] != null) {
					for (int j = 0; j < children[i].inspectorIconList.Length; j++) {
						if (!string.IsNullOrEmpty(iconName) 
						&& children[i].inspectorIconList[j].name == iconName) { 
							children[i].inspectorIconList[j].sprite = icon;
							Debug.Log("Swapped icon for " + children[i].inspectorIconList[j].name);
						}
					}
					children[i].ResetIconList();

					Debug.Log("Swapped icons for " + children[i].name);
				}
			}
		}

		public void OnGUI() {
			GUILayout.Label("Select a GameObject to edit TextPic icons", EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			GUILayout.Label("GameObject", EditorStyles.boldLabel);

			EditorGUI.BeginChangeCheck();
			
			if (Selection.activeGameObject != null) {
				o = Selection.activeGameObject;
			}
			EditorGUILayout.ObjectField(o, typeof(GameObject), true);
			EditorGUI.EndChangeCheck();

			if (o != null) {
				EditorGUILayout.BeginHorizontal();

				GUILayout.Label("Icon Name:", GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();

				iconName = EditorGUILayout.TextField(iconName, GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();

				GUILayout.Label("New Sprite:", GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();

				icon = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false, GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Edit Icons")) {
					Swap(o);
				}

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
			}
		}
	}

}
#endif