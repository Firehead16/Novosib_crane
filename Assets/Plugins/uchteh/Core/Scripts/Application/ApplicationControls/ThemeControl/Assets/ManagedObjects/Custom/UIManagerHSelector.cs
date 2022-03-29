using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerHSelector : UiManagerObject
	{
		[Header("RESOURCES")]
		public List<GameObject> images = new List<GameObject>();
		public List<GameObject> imagesHighlighted = new List<GameObject>();
		public List<GameObject> texts = new List<GameObject>();

		HorizontalSelector hSelector;

		protected new void OnEnable()
		{
			hSelector = gameObject.GetComponent<HorizontalSelector>();

			base.OnEnable();
		}

		protected override void UpdateTheme()
		{
			for (int i = 0; i < images.Count; ++i)
			{
				Image currentImage = images[i].GetComponent<Image>();
				currentImage.color = new Color(ThemeSettings.SelectorColor.r, ThemeSettings.SelectorColor.g, ThemeSettings.SelectorColor.b, currentImage.color.a);
			}

			for (int i = 0; i < imagesHighlighted.Count; ++i)
			{
				Image currentAlphaImage = imagesHighlighted[i].GetComponent<Image>();
				currentAlphaImage.color = new Color(ThemeSettings.SelectorHighlightedColor.r, ThemeSettings.SelectorHighlightedColor.g, ThemeSettings.SelectorHighlightedColor.b, currentAlphaImage.color.a);
			}

			for (int i = 0; i < texts.Count; ++i)
			{
				TextMeshProUGUI currentText = texts[i].GetComponent<TextMeshProUGUI>();
				currentText.color = new Color(ThemeSettings.SelectorColor.r, ThemeSettings.SelectorColor.g, ThemeSettings.SelectorColor.b, currentText.color.a);
				currentText.font = ThemeSettings.SelectorFont;
				currentText.fontSize = ThemeSettings.HSelectorFontSize;
			}

			if (hSelector != null)
			{
				hSelector.invertAnimation = ThemeSettings.HSelectorInvertAnimation;
				hSelector.loopSelection = ThemeSettings.HSelectorLoopSelection;
			}
		}
	}
}