#if UNITY_EDITOR

using Core.Ui.Extensions;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Core.Ui
{
	[CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
	public class NonDrawingGraphicEditor : GraphicEditor
	{
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
			// skipping AppearanceControlsGUI
			base.RaycastControlsGUI();
			base.serializedObject.ApplyModifiedProperties();
		}
	}
} 
#endif