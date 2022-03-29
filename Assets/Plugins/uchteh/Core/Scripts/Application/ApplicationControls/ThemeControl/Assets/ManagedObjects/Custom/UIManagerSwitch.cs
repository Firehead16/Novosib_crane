using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerSwitch : UiManagerObject
	{
		[Header("RESOURCES")]
		public Image border;
		public Image background;
		public Image handleOn;
		public Image handleOff;

		protected override void UpdateTheme()
		{
			border.color = new Color(ThemeSettings.SwitchBorderColor.r, ThemeSettings.SwitchBorderColor.g, ThemeSettings.SwitchBorderColor.b, border.color.a);
			background.color = new Color(ThemeSettings.SwitchBackgroundColor.r, ThemeSettings.SwitchBackgroundColor.g, ThemeSettings.SwitchBackgroundColor.b, background.color.a);
			handleOn.color = new Color(ThemeSettings.SwitchHandleOnColor.r, ThemeSettings.SwitchHandleOnColor.g, ThemeSettings.SwitchHandleOnColor.b, handleOn.color.a);
			handleOff.color = new Color(ThemeSettings.SwitchHandleOffColor.r, ThemeSettings.SwitchHandleOffColor.g, ThemeSettings.SwitchHandleOffColor.b, handleOff.color.a);
		}
	}
}