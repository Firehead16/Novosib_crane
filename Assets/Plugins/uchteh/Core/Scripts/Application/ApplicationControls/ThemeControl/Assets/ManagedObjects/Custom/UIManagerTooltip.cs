using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerTooltip : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image background;
		public TextMeshProUGUI text;

		protected override void UpdateTheme()
		{
			background.color = ThemeSettings.TooltipBackgroundColor;
			text.color = ThemeSettings.TooltipTextColor;
			text.font = ThemeSettings.TooltipFont;
			text.fontSize = ThemeSettings.TooltipFontSize;
		}
	}
}