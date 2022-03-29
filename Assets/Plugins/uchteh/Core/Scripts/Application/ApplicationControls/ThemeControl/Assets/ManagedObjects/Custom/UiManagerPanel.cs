using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UiManagerPanel : UiManagerObject
	{
		private Image image;

		public override void Load()
		{
			base.Load();
			image = GetComponent<Image>();
		}

		protected override void UpdateTheme()
		{
			image.color = ThemeSettings.PanelColor;
		}

	}
}
