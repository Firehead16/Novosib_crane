using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerSlider : UiManagerObject
	{
		public bool hasLabel;
		public bool hasPopupLabel;

		[Header("RESOURCES")]
		public Image background;
		public Image bar;
		public Image handle;
		[HideInInspector] public TextMeshProUGUI label;
		[HideInInspector] public TextMeshProUGUI popupLabel;

		protected override void UpdateTheme()
		{
			if (ThemeSettings.SliderTheme == ThemeSettings.SliderThemeType.Basic)
			{
				background.color = ThemeSettings.SliderBackgroundColor;
				bar.color = ThemeSettings.SliderColor;
				handle.color = ThemeSettings.SliderColor;

				if (hasLabel)
				{
					label.color = new Color(ThemeSettings.SliderColor.r, ThemeSettings.SliderColor.g, ThemeSettings.SliderColor.b, label.color.a);
					label.font = ThemeSettings.SliderLabelFont;
					label.fontSize = ThemeSettings.SliderLabelFontSize;
				}

				if (hasPopupLabel)
				{
					popupLabel.color = new Color(ThemeSettings.SliderPopupLabelColor.r, ThemeSettings.SliderPopupLabelColor.g, ThemeSettings.SliderPopupLabelColor.b, popupLabel.color.a);
					popupLabel.font = ThemeSettings.SliderLabelFont;
				}
			}

			else if (ThemeSettings.SliderTheme == ThemeSettings.SliderThemeType.Custom)
			{
				background.color = ThemeSettings.SliderBackgroundColor;
				bar.color = ThemeSettings.SliderColor;
				handle.color = ThemeSettings.SliderHandleColor;

				if (hasLabel)
				{
					label.color = new Color(ThemeSettings.SliderLabelColor.r, ThemeSettings.SliderLabelColor.g, ThemeSettings.SliderLabelColor.b, label.color.a);
					label.font = ThemeSettings.SliderLabelFont;
					label.font = ThemeSettings.SliderLabelFont;
				}

				if (hasPopupLabel)
				{
					popupLabel.color = new Color(ThemeSettings.SliderPopupLabelColor.r, ThemeSettings.SliderPopupLabelColor.g, ThemeSettings.SliderPopupLabelColor.b, popupLabel.color.a);
					popupLabel.font = ThemeSettings.SliderLabelFont;
				}
			}
		}
	}
}