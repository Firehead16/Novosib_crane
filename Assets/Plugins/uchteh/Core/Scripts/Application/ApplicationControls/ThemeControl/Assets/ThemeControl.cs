using System.Collections.Generic;
using System.Linq;

namespace Core.Ui
{
	public class ThemeControl : CanvasControl<IThemePanel> ,ITheming
	{
		public bool IsInitialized { get; set; }

		private List<ThemeSettings> themes;

		private ThemeSettings currentTheme;

		protected override void OnItemsInitialised()
		{
			themes = ThemeCotrolSettings.Default().Themes;
			currentTheme = themes.First();
			currentTheme.SetTheme();
		}
	}
}