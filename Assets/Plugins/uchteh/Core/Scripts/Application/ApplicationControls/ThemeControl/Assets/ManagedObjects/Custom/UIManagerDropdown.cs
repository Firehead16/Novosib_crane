using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerDropdown : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image background;
		public Image contentBackground;
		public Image mainIcon;
		public TextMeshProUGUI mainText;
		public Image expandIcon;
		public Image itemBackground;
		public Image itemIcon;
		public TextMeshProUGUI itemText;

		CustomDropdown dropdownMain;
		DropdownMultiSelect dropdownMulti;

		protected new void OnEnable()
		{
			dropdownMain = gameObject.GetComponent<CustomDropdown>();
			if (dropdownMain == null)
				dropdownMulti = gameObject.GetComponent<DropdownMultiSelect>();

			base.OnEnable();
		}

		protected override void UpdateTheme()
		{
			if (ThemeSettings.ButtonType == ThemeSettings.ButtonThemeType.Basic)
			{
				background.color = ThemeSettings.DropdownColor;
				contentBackground.color = ThemeSettings.DropdownColor;
				mainIcon.color = ThemeSettings.DropdownTextColor;
				mainText.color = ThemeSettings.DropdownTextColor;
				expandIcon.color = ThemeSettings.DropdownTextColor;
				itemBackground.color = ThemeSettings.DropdownItemColor;
				itemIcon.color = ThemeSettings.DropdownTextColor;
				itemText.color = ThemeSettings.DropdownTextColor;
				mainText.font = ThemeSettings.DropdownFont;
				mainText.fontSize = ThemeSettings.DropdownFontSize;
				itemText.font = ThemeSettings.DropdownFont;
				itemText.fontSize = ThemeSettings.DropdownFontSize;
			}

			else if (ThemeSettings.ButtonType == ThemeSettings.ButtonThemeType.Custom)
			{
				background.color = ThemeSettings.DropdownColor;
				contentBackground.color = ThemeSettings.DropdownColor;
				mainIcon.color = ThemeSettings.DropdownIconColor;
				mainText.color = ThemeSettings.DropdownTextColor;
				expandIcon.color = ThemeSettings.DropdownIconColor;
				itemBackground.color = ThemeSettings.DropdownItemColor;
				itemIcon.color = ThemeSettings.DropdownItemIconColor;
				itemText.color = ThemeSettings.DropdownItemTextColor;
				mainText.font = ThemeSettings.DropdownFont;
				mainText.fontSize = ThemeSettings.DropdownFontSize;
				itemText.font = ThemeSettings.DropdownItemFont;
				itemText.fontSize = ThemeSettings.DropdownItemFontSize;
			}

			if (dropdownMain != null)
			{
				if (ThemeSettings.DropdownAnimation == ThemeSettings.DropdownAnimationType.Fading)
					dropdownMain.animationType = CustomDropdown.AnimationType.FADING;

				else if (ThemeSettings.DropdownAnimation == ThemeSettings.DropdownAnimationType.Sliding)
					dropdownMain.animationType = CustomDropdown.AnimationType.SLIDING;

				else if (ThemeSettings.DropdownAnimation == ThemeSettings.DropdownAnimationType.Stylish)
					dropdownMain.animationType = CustomDropdown.AnimationType.STYLISH;
			}

			else if (dropdownMulti != null)
			{
				if (ThemeSettings.DropdownAnimation == ThemeSettings.DropdownAnimationType.Fading)
					dropdownMulti.animationType = DropdownMultiSelect.AnimationType.FADING;

				else if (ThemeSettings.DropdownAnimation == ThemeSettings.DropdownAnimationType.Sliding)
					dropdownMulti.animationType = DropdownMultiSelect.AnimationType.SLIDING;

				else if (ThemeSettings.DropdownAnimation == ThemeSettings.DropdownAnimationType.Stylish)
					dropdownMulti.animationType = DropdownMultiSelect.AnimationType.STYLISH;
			}

		}
	}
}