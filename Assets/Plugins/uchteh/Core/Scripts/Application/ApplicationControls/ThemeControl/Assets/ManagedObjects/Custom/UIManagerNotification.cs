using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerNotification : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image background;
		public Image icon;
		public TextMeshProUGUI title;
		public TextMeshProUGUI description;

		protected override void UpdateTheme()
		{
			background.color = ThemeSettings.NotificationBackgroundColor;
			icon.color = ThemeSettings.NotificationIconColor;
			title.color = ThemeSettings.NotificationTitleColor;
			description.color = ThemeSettings.NotificationDescriptionColor;
			title.font = ThemeSettings.NotificationTitleFont;
			title.fontSize = ThemeSettings.NotificationTitleFontSize;
			description.font = ThemeSettings.NotificationDescriptionFont;
			description.fontSize = ThemeSettings.NotificationDescriptionFontSize;
		}
	}
}