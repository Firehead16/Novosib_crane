using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Ui
{
	[CreateAssetMenu(fileName = "New Theme", menuName = "StyleManager/ThemeCotrolSettings")]
	public class ThemeCotrolSettings : SubSettings<ThemeCotrolSettings>
	{
		[SerializeField] 
		private List<ThemeSettings> themes = null;

		public List<ThemeSettings> Themes => themes;
	}
}