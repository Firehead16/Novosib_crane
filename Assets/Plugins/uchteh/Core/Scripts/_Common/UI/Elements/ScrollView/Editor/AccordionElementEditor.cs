#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

namespace Core.Ui
{
	[CustomEditor(typeof(AccordionElement), true)]
	public class AccordionElementEditor : ToggleEditor
	{

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_MinHeight"));
			this.serializedObject.ApplyModifiedProperties();

			base.serializedObject.Update();
			EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_IsOn"));
			EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_Interactable"));
			base.serializedObject.ApplyModifiedProperties();
		}
	}
} 
#endif