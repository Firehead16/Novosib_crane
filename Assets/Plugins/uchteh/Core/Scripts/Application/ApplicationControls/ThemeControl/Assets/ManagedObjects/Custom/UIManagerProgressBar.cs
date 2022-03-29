using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerProgressBar : UiManagerObject
	{
		public Image bar;
		public Image background;
		public TextMeshProUGUI label;

		protected override void UpdateTheme()
		{
			bar.color = ThemeSettings.ProgressBarColor;
			background.color = ThemeSettings.ProgressBarBackgroundColor;
			label.color = ThemeSettings.ProgressBarLabelColor;
			label.font = ThemeSettings.ProgressBarLabelFont;
			label.fontSize = ThemeSettings.ProgressBarLabelFontSize;
		}
	}
}