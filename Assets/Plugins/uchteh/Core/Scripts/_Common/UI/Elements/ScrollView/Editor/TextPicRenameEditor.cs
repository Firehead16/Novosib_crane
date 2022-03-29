#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Core.Ui.Extensions;

namespace Core.Ui
{

	public class TextPicRenameEditor : EditorWindow {
		[MenuItem("Window/UI/Extensions/TextPic Rename Icons and Text")]
		protected static void ShowTextPicRenameEditor() {
			var wnd = GetWindow<TextPicRenameEditor>();
			wnd.titleContent.text = "Rename Icon List";
			wnd.Show();
		}

		private GameObject o;

		private static int columnWidth = 300;

		private string prefix;
		private string suffix;
		private string originalText;
		private string replacementText;

		public void Rename(GameObject o) {
			Debug.Log("Changing icons and text for " + o.name);

			TextPic[] children = o.GetComponentsInChildren<TextPic>(true);
			for(int i = 0; i < children.Length; i++) {
				if (children[i] != null) {
					for (int j = 0; j < children[i].inspectorIconList.Length; j++) {
						if (!string.IsNullOrEmpty(originalText) 
						&& children[i].inspectorIconList[j].name.Contains(originalText)) { 
							children[i].text.Replace(originalText, replacementText);
							children[i].inspectorIconList[j].name = children[i].inspectorIconList[j].name.Replace(originalText, replacementText);
							Debug.Log("Renamed icon for " + children[i].inspectorIconList[j].name);
						}

						if (!string.IsNullOrEmpty(prefix) 
						&& !string.IsNullOrEmpty(suffix) 
						&& !children[i].inspectorIconList[j].name.StartsWith(prefix) 
						&& !children[i].inspectorIconList[j].name.EndsWith(suffix)) {
							children[i].text.Replace(children[i].inspectorIconList[j].name, prefix + children[i].inspectorIconList[j].name + suffix);
							children[i].inspectorIconList[j].name = prefix + children[i].inspectorIconList[j].name + suffix;
							Debug.Log("Renamed icon for " + children[i].inspectorIconList[j].name);
						}
					}
					children[i].ResetIconList();

					Debug.Log("Renamed icons for " + children[i].name);
				}
			}
		}

		public void OnGUI() {
			GUILayout.Label("Select a GameObject to rename TextPic icons and text", EditorStyles.boldLabel);
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
				
				GUILayout.Label("Prefix:", GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				
				prefix = EditorGUILayout.TextField(prefix, GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();
				
				GUILayout.Label("Original Text:", GUILayout.Width(columnWidth));
				
				GUILayout.Label("Replacement Text:", GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();

				originalText = EditorGUILayout.TextField(originalText, GUILayout.Width(columnWidth));

				replacementText = EditorGUILayout.TextField(replacementText, GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Suffix:", GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				suffix = EditorGUILayout.TextField(suffix, GUILayout.Width(columnWidth));

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Rename Icons and Text")) {
					#if UNITY_EDITOR
					Rename(o);
					#endif
				}

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
			}
		}
	}

}
#endif
