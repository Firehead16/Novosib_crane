using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Presets;
#endif

#if UNITY_EDITOR

namespace Core.Ui
{
	[CustomEditor(typeof(ThemeSettings))]
	[System.Serializable]
	public class ThemeSettingsEditor : Editor
	{
		protected static bool ShowPanel;
		protected static bool ShowAnimatedIcon;
		protected static bool ShowButton;
		protected static bool ShowDropdown;
		protected static bool ShowHorSelector;
		protected static bool ShowInputField;
		protected static bool ShowModalWindow;
		protected static bool ShowNotification;
		protected static bool ShowProgressBar;
		protected static bool ShowScrollbar;
		protected static bool ShowSlider;
		protected static bool ShowSwitch;
		protected static bool ShowToggle;
		protected static bool ShowTooltip;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			// GUI skin variables
			GUISkin customSkin = (GUISkin)Resources.Load("Editor\\Custom Skin Light");

			// Foldout style
			GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
			foldoutStyle.font = customSkin.font;
			foldoutStyle.fontStyle = FontStyle.Normal;
			foldoutStyle.fontSize = 15;
			foldoutStyle.margin = new RectOffset(12, 55, 6, 6);
			Vector2 contentOffset = foldoutStyle.contentOffset;
			contentOffset.y = -1;
			contentOffset.x = 5;
			foldoutStyle.contentOffset = contentOffset;

			GUILayout.Space(6);

			#region Элементы

			#region Panel

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var panelColor = serializedObject.FindProperty("PanelColor");
			ShowPanel = EditorGUILayout.Foldout(ShowPanel, "Panels", foldoutStyle);

			if (ShowPanel)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(panelColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Animated Icon

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var animatedIconColor = serializedObject.FindProperty("AnimatedIconColor");
			ShowAnimatedIcon = EditorGUILayout.Foldout(ShowAnimatedIcon, "Animated Icon", foldoutStyle);

			if (ShowAnimatedIcon)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(animatedIconColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Button

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var buttonTheme = serializedObject.FindProperty("ButtonType");
			var buttonFont = serializedObject.FindProperty("ButtonFont");
			var buttonFontSize = serializedObject.FindProperty("ButtonFontSize");
			var buttonBorderColor = serializedObject.FindProperty("ButtonBorderColor");
			var buttonFilledColor = serializedObject.FindProperty("ButtonFilledColor");
			var buttonTextBasicColor = serializedObject.FindProperty("ButtonTextBasicColor");
			var buttonTextColor = serializedObject.FindProperty("ButtonTextColor");
			var buttonTextHighlightedColor = serializedObject.FindProperty("ButtonTextHighlightedColor");
			var buttonIconBasicColor = serializedObject.FindProperty("ButtonIconBasicColor");
			var buttonIconColor = serializedObject.FindProperty("ButtonIconColor");
			var buttonIconHighlightedColor = serializedObject.FindProperty("ButtonIconHighlightedColor");
			ShowButton = EditorGUILayout.Foldout(ShowButton, "Button", foldoutStyle);

			if (ShowButton && buttonTheme.enumValueIndex == 0)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonTheme, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonBorderColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonFilledColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			if (ShowButton && buttonTheme.enumValueIndex == 1)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonTheme, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonBorderColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Filled Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonFilledColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Text Basic Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonTextBasicColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonTextColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Text Hover Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonTextHighlightedColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Icon Basic Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonIconBasicColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonIconColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Icon Hover Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(buttonIconHighlightedColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region  Dropdown

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var dropdownTheme = serializedObject.FindProperty("DropdownTheme");
			var dropdownAnimationType = serializedObject.FindProperty("DropdownAnimation");
			var dropdownFont = serializedObject.FindProperty("DropdownFont");
			var dropdownFontSize = serializedObject.FindProperty("DropdownFontSize");
			var dropdownItemFont = serializedObject.FindProperty("DropdownItemFont");
			var dropdownItemFontSize = serializedObject.FindProperty("DropdownItemFontSize");
			var dropdownColor = serializedObject.FindProperty("DropdownColor");
			var dropdownTextColor = serializedObject.FindProperty("DropdownTextColor");
			var dropdownIconColor = serializedObject.FindProperty("DropdownIconColor");
			var dropdownItemColor = serializedObject.FindProperty("DropdownItemColor");
			var dropdownItemTextColor = serializedObject.FindProperty("DropdownItemTextColor");
			var dropdownItemIconColor = serializedObject.FindProperty("DropdownItemIconColor");
			ShowDropdown = EditorGUILayout.Foldout(ShowDropdown, "Dropdown", foldoutStyle);

			if (ShowDropdown && dropdownTheme.enumValueIndex == 0)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownTheme, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Animation Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownAnimationType, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownTextColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Item Background"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownItemColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				EditorGUILayout.HelpBox("Item values will be applied at start.", MessageType.Info);
			}

			if (ShowDropdown && dropdownTheme.enumValueIndex == 1)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownTheme, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Animation Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownAnimationType, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Item Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownItemFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Item Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownItemFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownTextColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownIconColor, new GUIContent(""));

				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Item Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownItemColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Item Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownItemTextColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Item Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(dropdownItemIconColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				EditorGUILayout.HelpBox("Item values will be applied at start.", MessageType.Info);
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Horizontal Selector

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var selectorFont = serializedObject.FindProperty("SelectorFont");
			var hSelectorFontSize = serializedObject.FindProperty("HSelectorFontSize");
			var selectorColor = serializedObject.FindProperty("SelectorColor");
			var selectorHighlightedColor = serializedObject.FindProperty("SelectorHighlightedColor");
			var hSelectorInvertAnimation = serializedObject.FindProperty("HSelectorInvertAnimation");
			var hSelectorLoopSelection = serializedObject.FindProperty("HSelectorLoopSelection");
			ShowHorSelector = EditorGUILayout.Foldout(ShowHorSelector, "Horizontal Selector", foldoutStyle);

			if (ShowHorSelector)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(selectorFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(hSelectorFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(selectorColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Highlighted Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(selectorHighlightedColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Invert Animation"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				hSelectorInvertAnimation.boolValue = GUILayout.Toggle(hSelectorInvertAnimation.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle"));
				hSelectorInvertAnimation.boolValue = GUILayout.Toggle(hSelectorInvertAnimation.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Loop Selection"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				hSelectorLoopSelection.boolValue = GUILayout.Toggle(hSelectorLoopSelection.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle"));
				hSelectorLoopSelection.boolValue = GUILayout.Toggle(hSelectorLoopSelection.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Input Field

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var inputFieldFont = serializedObject.FindProperty("InputFieldFont");
			var inputFieldFontSize = serializedObject.FindProperty("InputFieldFontSize");
			var inputFieldColor = serializedObject.FindProperty("InputFieldColor");
			ShowInputField = EditorGUILayout.Foldout(ShowInputField, "Input Field", foldoutStyle);

			if (ShowInputField)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(inputFieldFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(inputFieldFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(inputFieldColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Modal Window

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var modalWindowTitleFont = serializedObject.FindProperty("ModalWindowTitleFont");
			var modalWindowContentFont = serializedObject.FindProperty("ModalWindowContentFont");
			var modalWindowTitleColor = serializedObject.FindProperty("ModalWindowTitleColor");
			var modalWindowDescriptionColor = serializedObject.FindProperty("ModalWindowDescriptionColor");
			var modalWindowIconColor = serializedObject.FindProperty("ModalWindowIconColor");
			var modalWindowBackgroundColor = serializedObject.FindProperty("ModalWindowBackgroundColor");
			var modalWindowContentPanelColor = serializedObject.FindProperty("ModalWindowContentPanelColor");
			ShowModalWindow = EditorGUILayout.Foldout(ShowModalWindow, "Modal Window", foldoutStyle);

			if (ShowModalWindow)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Title Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowTitleFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Content Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowContentFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Title Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowTitleColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Description Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowDescriptionColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowIconColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Content Panel Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(modalWindowContentPanelColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				EditorGUILayout.HelpBox("These values will only affect 'Style 1 - Standard' window.", MessageType.Info);
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Notification

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var notificationTitleFont = serializedObject.FindProperty("NotificationTitleFont");
			var notificationTitleFontSize = serializedObject.FindProperty("NotificationTitleFontSize");
			var notificationDescriptionFont = serializedObject.FindProperty("NotificationDescriptionFont");
			var notificationDescriptionFontSize = serializedObject.FindProperty("NotificationDescriptionFontSize");
			var notificationBackgroundColor = serializedObject.FindProperty("NotificationBackgroundColor");
			var notificationTitleColor = serializedObject.FindProperty("NotificationTitleColor");
			var notificationDescriptionColor = serializedObject.FindProperty("NotificationDescriptionColor");
			var notificationIconColor = serializedObject.FindProperty("NotificationIconColor");
			ShowNotification = EditorGUILayout.Foldout(ShowNotification, "Notification", foldoutStyle);

			if (ShowNotification)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Title Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationTitleFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Title Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationTitleFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Description Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationDescriptionFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Description Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationDescriptionFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Title Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationTitleColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Description Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationDescriptionColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(notificationIconColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Progress Bar

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var progressBarLabelFont = serializedObject.FindProperty("ProgressBarLabelFont");
			var progressBarLabelFontSize = serializedObject.FindProperty("ProgressBarLabelFontSize");
			var progressBarColor = serializedObject.FindProperty("ProgressBarColor");
			var progressBarBackgroundColor = serializedObject.FindProperty("ProgressBarBackgroundColor");
			var progressBarLoopBackgroundColor = serializedObject.FindProperty("ProgressBarLoopBackgroundColor");
			var progressBarLabelColor = serializedObject.FindProperty("ProgressBarLabelColor");
			ShowProgressBar = EditorGUILayout.Foldout(ShowProgressBar, "Progress Bar", foldoutStyle);

			if (ShowProgressBar)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(progressBarLabelFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(progressBarLabelFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(progressBarColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(progressBarLabelColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(progressBarBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Loop BG Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(progressBarLoopBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Scrollbar

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var scrollbarColor = serializedObject.FindProperty("ScrollbarColor");
			var scrollbarBackgroundColor = serializedObject.FindProperty("ScrollbarBackgroundColor");
			ShowScrollbar = EditorGUILayout.Foldout(ShowScrollbar, "Scrollbar", foldoutStyle);

			if (ShowScrollbar)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Bar Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(scrollbarColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(scrollbarBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Slider

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var sliderThemeType = serializedObject.FindProperty("SliderTheme");
			var sliderLabelFont = serializedObject.FindProperty("SliderLabelFont");
			var sliderLabelFontSize = serializedObject.FindProperty("SliderLabelFontSize");
			var sliderColor = serializedObject.FindProperty("SliderColor");
			var sliderLabelColor = serializedObject.FindProperty("SliderLabelColor");
			var sliderPopupLabelColor = serializedObject.FindProperty("SliderPopupLabelColor");
			var sliderHandleColor = serializedObject.FindProperty("SliderHandleColor");
			var sliderBackgroundColor = serializedObject.FindProperty("SliderBackgroundColor");
			ShowSlider = EditorGUILayout.Foldout(ShowSlider, "Slider", foldoutStyle);

			if (ShowSlider && sliderThemeType.enumValueIndex == 0)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderThemeType, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderLabelFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderLabelFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Popup Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderLabelColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			if (ShowSlider && sliderThemeType.enumValueIndex == 1)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderThemeType, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderLabelFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderLabelFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderLabelColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Label Popup Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderPopupLabelColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Handle Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderHandleColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(sliderBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Switch

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var switchBorderColor = serializedObject.FindProperty("SwitchBorderColor");
			var switchBackgroundColor = serializedObject.FindProperty("SwitchBackgroundColor");
			var switchHandleOnColor = serializedObject.FindProperty("SwitchHandleOnColor");
			var switchHandleOffColor = serializedObject.FindProperty("SwitchHandleOffColor");
			ShowSwitch = EditorGUILayout.Foldout(ShowSwitch, "Switch", foldoutStyle);

			if (ShowSwitch)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(switchBorderColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(switchBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Handle On Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(switchHandleOnColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Handle Off Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(switchHandleOffColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Toggle

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var toggleFont = serializedObject.FindProperty("ToggleFont");
			var toggleFontSize = serializedObject.FindProperty("ToggleFontSize");
			var toggleTextColor = serializedObject.FindProperty("ToggleTextColor");
			var toggleBorderColor = serializedObject.FindProperty("ToggleBorderColor");
			var toggleBackgroundColor = serializedObject.FindProperty("ToggleBackgroundColor");
			var toggleCheckColor = serializedObject.FindProperty("ToggleCheckColor");
			ShowToggle = EditorGUILayout.Foldout(ShowToggle, "Toggle", foldoutStyle);

			if (ShowToggle)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(toggleFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(toggleFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(toggleTextColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(toggleBorderColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(toggleBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Check Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(toggleCheckColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#region Tooltip

			GUILayout.BeginVertical(EditorStyles.helpBox);

			var tooltipFont = serializedObject.FindProperty("TooltipFont");
			var tooltipFontSize = serializedObject.FindProperty("TooltipFontSize");
			var tooltipTextColor = serializedObject.FindProperty("TooltipTextColor");
			var tooltipBackgroundColor = serializedObject.FindProperty("TooltipBackgroundColor");
			ShowTooltip = EditorGUILayout.Foldout(ShowTooltip, "Tooltip", foldoutStyle);

			if (ShowTooltip)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(tooltipFont, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Font Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(tooltipFontSize, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(tooltipTextColor, new GUIContent(""));

				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.helpBox);

				EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
				EditorGUILayout.PropertyField(tooltipBackgroundColor, new GUIContent(""));

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);

			#endregion

			#endregion

			GUILayout.Space(5);

			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Space(6);

			#region Настройки

			#region Обновлять значения элементов

			var enableDynamicUpdate = serializedObject.FindProperty("enableDynamicUpdate");

			GUILayout.BeginHorizontal(EditorStyles.helpBox);

			enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent("Обновить значения"), customSkin.FindStyle("Toggle"));
			enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

			GUILayout.EndHorizontal();

			#endregion

			#region Расширенный выбор цветов

			var enableExtendedColorPicker = serializedObject.FindProperty("enableExtendedColorPicker");

			GUILayout.BeginHorizontal(EditorStyles.helpBox);

			enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent("Расширенный Color Picker"), customSkin.FindStyle("Toggle"));
			enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

			GUILayout.EndHorizontal();

			if (enableExtendedColorPicker.boolValue)
				EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 1);

			else
				EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 0);



			#endregion

			#region Подсказки

			var editorHints = serializedObject.FindProperty("editorHints");

			GUILayout.BeginHorizontal(EditorStyles.helpBox);

			editorHints.boolValue = GUILayout.Toggle(editorHints.boolValue, new GUIContent("Подсказки"), customSkin.FindStyle("Toggle"));
			editorHints.boolValue = GUILayout.Toggle(editorHints.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

			GUILayout.EndHorizontal();

			if (editorHints.boolValue)
			{
				EditorGUILayout.HelpBox("Эти значения универсальны и будут влиять на любой объект, содержащий компонент 'UI Manager'.", MessageType.Info);
				EditorGUILayout.HelpBox("Удалите из объекта компонент 'UI Manager', чтобы получить уникальные значения.", MessageType.Info);
				EditorGUILayout.HelpBox("Вы можете нажать 'CTRL + SHIFT + M', чтобы быстро открыть настройки темы.", MessageType.Info);
			}

			serializedObject.ApplyModifiedProperties();

			#endregion

			#endregion

			GUILayout.Space(12);

			#region Сбросить настройки

			//GUILayout.BeginHorizontal();

			//if (GUILayout.Button("Сбросить настройки", customSkin.button))
			//	ResetToDefaults();

			//GUILayout.EndHorizontal();

			#endregion

			#region Установить тему

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Установить тему", customSkin.button))
			{
				ThemeSettings theme = (ThemeSettings) target;
				theme.SetTheme();
			}
			
			GUILayout.EndHorizontal();

			#endregion

			GUILayout.Space(5);
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			// Apply & Update button
			GUILayout.Space(4);
		}


		private void ResetToDefaults()
		{
			if (EditorUtility.DisplayDialog("Тема", "Сбросить настройки?", "Да", "Нет"))
			{
				try
				{
					Preset defaultPreset = Resources.Load<Preset>("Presets/Default");
					defaultPreset.ApplyTo(Resources.Load("MUIP Manager"));
					Selection.activeObject = null;
					Debug.Log("Настройки темы - успешно сброшены.");
				}

				catch
				{
					Debug.LogWarning("Настройки темы - ошибка сброса.");
				}
			}
		}
	}
}

#endif