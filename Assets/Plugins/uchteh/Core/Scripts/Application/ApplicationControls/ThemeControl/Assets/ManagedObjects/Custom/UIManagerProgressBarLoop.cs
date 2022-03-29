using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerProgressBarLoop : UiManagerObject
	{
		public bool hasBackground;
		public bool useRegularBackground;

		[Header("RESOURCES")]
		public Image bar;
		[HideInInspector] public Image background;

		protected override void UpdateTheme()
		{
			bar.color = ThemeSettings.ProgressBarColor;

			if (hasBackground)
			{
				if (useRegularBackground)
					background.color = ThemeSettings.ProgressBarBackgroundColor;
				else
					background.color = ThemeSettings.ProgressBarLoopBackgroundColor;
			}
		}
	}
}