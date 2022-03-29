using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
    [ExecuteInEditMode]
    public class UIManagerInputField : UiManagerObject
    {
	    [Header("RESOURCES")]
        public List<GameObject> images = new List<GameObject>();
        public List<GameObject> texts = new List<GameObject>();
        
        protected override void UpdateTheme()
        {
            for (int i = 0; i < images.Count; ++i)
            {
                Image currentImage = images[i].GetComponent<Image>();
                currentImage.color = new Color(ThemeSettings.InputFieldColor.r, ThemeSettings.InputFieldColor.g, ThemeSettings.InputFieldColor.b, currentImage.color.a);
            }

            for (int i = 0; i < texts.Count; ++i)
            {
                TextMeshProUGUI currentText = texts[i].GetComponent<TextMeshProUGUI>();
                currentText.color = new Color(ThemeSettings.InputFieldColor.r, ThemeSettings.InputFieldColor.g, ThemeSettings.InputFieldColor.b, currentText.color.a);
                currentText.font = ThemeSettings.InputFieldFont;
                currentText.fontSize = ThemeSettings.InputFieldFontSize;
            }
        }
    }
}