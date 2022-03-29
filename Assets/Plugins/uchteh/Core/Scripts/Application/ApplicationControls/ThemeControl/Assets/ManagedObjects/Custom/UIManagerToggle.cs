using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerToggle : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image border;
		public Image background;
		public Image check;
		public TextMeshProUGUI onLabel;
		public TextMeshProUGUI offLabel;

		protected override void UpdateTheme()
		{
			border.color = ThemeSettings.ToggleBorderColor;
			background.color = ThemeSettings.ToggleBackgroundColor;
			check.color = ThemeSettings.ToggleCheckColor;
			onLabel.color = new Color(ThemeSettings.ToggleTextColor.r, ThemeSettings.ToggleTextColor.g, ThemeSettings.ToggleTextColor.b, onLabel.color.a);
			onLabel.font = ThemeSettings.ToggleFont;
			onLabel.fontSize = ThemeSettings.ToggleFontSize;
			offLabel.color = new Color(ThemeSettings.ToggleTextColor.r, ThemeSettings.ToggleTextColor.g, ThemeSettings.ToggleTextColor.b, offLabel.color.a);
			offLabel.font = ThemeSettings.ToggleFont;
			offLabel.fontSize = ThemeSettings.ToggleFontSize;
		}
	}
}