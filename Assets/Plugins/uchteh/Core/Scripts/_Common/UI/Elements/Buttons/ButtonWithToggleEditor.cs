#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

namespace Core.Ui
{
	[CustomEditor(typeof(ButtonWithTooltip))]
	public class ButtonWithToggleEditor : ButtonEditor
	{
		public override void OnInspectorGUI()
		{
			ButtonWithTooltip targetMenuButton = (ButtonWithTooltip)target;

			targetMenuButton.TooltipText = EditorGUILayout.TextField("TooltipText", targetMenuButton.TooltipText);

			// Show default inspector property editor
			base.OnInspectorGUI();
		}
	}
}

#endif