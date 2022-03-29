using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
    [ExecuteInEditMode]
    public class UIManagerAnimatedIcon : UiManagerObject
    {
	    [Header("RESOURCES")]
        public List<GameObject> images = new List<GameObject>();
        public List<GameObject> imagesWithAlpha = new List<GameObject>();

        protected override void UpdateTheme()
        {
			for (int i = 0; i < images.Count; ++i)
			{
				Image currentImage = images[i].GetComponent<Image>();
				currentImage.color = ThemeSettings.AnimatedIconColor;
			}

			for (int i = 0; i < imagesWithAlpha.Count; ++i)
			{
				Image currentAlphaImage = imagesWithAlpha[i].GetComponent<Image>();
				currentAlphaImage.color = new Color(ThemeSettings.AnimatedIconColor.r, ThemeSettings.AnimatedIconColor.g, ThemeSettings.AnimatedIconColor.b, currentAlphaImage.color.a);
			}
		}
    }
}