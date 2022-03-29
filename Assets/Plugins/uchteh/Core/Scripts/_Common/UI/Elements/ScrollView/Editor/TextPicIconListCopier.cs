#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Core.Ui.Extensions
{

	public class TextPicIconListCopier : EditorWindow
	{
		[MenuItem("Window/UI/Extensions/TextPic Copy Icon Lists")]
		protected static void ShowTextPicIconListCopier()
		{
			var wnd = GetWindow<TextPicIconListCopier>();
			wnd.titleContent.text = "Copy Icons in TextPic";
			wnd.Show();
		}

		private List<TextPic> textPicList = new List<TextPic>();

		void OnSelectionChange()
		{
			if (Selection.objects.Length > 1)
			{
				Debug.Log("Length? " + Selection.objects.Length);
				textPicList.Clear();

				foreach (Object o in Selection.objects)
				{
					if (o is GameObject)
					{
						TextPic tp = ((GameObject)o).GetComponent<TextPic>();
						if (tp != null)
						{
							textPicList.Add(tp);
						}
					}
				}
			}
			else if (Selection.activeObject is GameObject)
			{
				textPicList.Clear();
				TextPic tp = ((GameObject)Selection.activeObject).GetComponent<TextPic>();
				if (tp != null)
				{
					textPicList.Add(tp);
				}
			}
			else
			{
				textPicList.Clear();
			}

			this.Repaint();
		}

		private static int columnWidth = 300;

		private TextPic textPic;

		public void Copy()
		{
			foreach (TextPic tp in textPicList)
			{
				if (tp != null)
				{
					tp.inspectorIconList = new TextPic.IconName[textPic.inspectorIconList.Length];
					textPic.inspectorIconList.CopyTo(tp.inspectorIconList, 0);

					tp.ResetIconList();

					Debug.Log("Copied icons to " + tp.name);
				}
			}
		}

		public void OnGUI()
		{
			GUILayout.Label("TextPic to copy icons", EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			GUILayout.Label("TextPic", EditorStyles.boldLabel);

			EditorGUI.BeginChangeCheck();

			textPic = EditorGUILayout.ObjectField(textPic, typeof(TextPic), true) as TextPic;
			EditorGUI.EndChangeCheck();

			if (textPicList.Count > 0)
			{
				if (textPicList.Count == 1)
				{
					textPicList[0] = ((TextPic)EditorGUILayout.ObjectField(
						textPicList[0],
						typeof(TextPic),
						true,
						GUILayout.Width(columnWidth))
						);
				}
				else
				{
					GUILayout.Label("Multiple TextPic: " + textPicList.Count, GUILayout.Width(columnWidth));
				}

				if (textPic != null)
				{

					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Copy Icons"))
					{
						Copy();
					}

					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Separator();
				}
			}
			else
			{
				GUILayout.Label("Please select objects that have a TextPic component", EditorStyles.boldLabel);
			}
		}
	}

}
#endif