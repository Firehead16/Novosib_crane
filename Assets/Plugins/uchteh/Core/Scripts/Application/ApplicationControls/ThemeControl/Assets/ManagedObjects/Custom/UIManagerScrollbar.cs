using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerScrollbar : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image background;
		public Image bar;

		protected override void UpdateTheme()
		{
			background.color = ThemeSettings.ScrollbarBackgroundColor;
			bar.color = ThemeSettings.ScrollbarColor;
		}
	}
}