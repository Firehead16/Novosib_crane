using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerModalWindow : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image background;
		public Image contentBackground;
		public Image icon;
		public TextMeshProUGUI title;
		public TextMeshProUGUI description;

		protected override void UpdateTheme()
		{
			background.color = ThemeSettings.ModalWindowBackgroundColor;
			contentBackground.color = ThemeSettings.ModalWindowContentPanelColor;
			icon.color = ThemeSettings.ModalWindowIconColor;
			title.color = ThemeSettings.ModalWindowTitleColor;
			description.color = ThemeSettings.ModalWindowDescriptionColor;
			title.font = ThemeSettings.ModalWindowTitleFont;
			description.font = ThemeSettings.ModalWindowContentFont;
		}
	}
}