using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.Ui
{
	[ExecuteInEditMode]
	public class UIManagerButton : UiManagerObject
	{
		public ButtonType buttonType;

		// Basic Resources
		[HideInInspector] public Image basicFilled;
		[HideInInspector] public TextMeshProUGUI basicText;

		// Basic Only Icon Resources
		[HideInInspector] public Image basicOnlyIconFilled;
		[HideInInspector] public Image basicOnlyIconIcon;

		// Basic With Icon Resources
		[HideInInspector] public Image basicWithIconFilled;
		[HideInInspector] public Image basicWithIconIcon;
		[HideInInspector] public TextMeshProUGUI basicWithIconText;

		// Basic Outline Resources
		[HideInInspector] public Image basicOutlineBorder;
		[HideInInspector] public Image basicOutlineFilled;
		[HideInInspector] public TextMeshProUGUI basicOutlineText;
		[HideInInspector] public TextMeshProUGUI basicOutlineTextHighligted;

		// Basic Outline Only Icon Resources
		[HideInInspector] public Image basicOutlineOOBorder;
		[HideInInspector] public Image basicOutlineOOFilled;
		[HideInInspector] public Image basicOutlineOOIcon;
		[HideInInspector] public Image basicOutlineOOIconHighlighted;

		// Basic Outline With Icon Resources
		[HideInInspector] public Image basicOutlineWOBorder;
		[HideInInspector] public Image basicOutlineWOFilled;
		[HideInInspector] public Image basicOutlineWOIcon;
		[HideInInspector] public Image basicOutlineWOIconHighlighted;
		[HideInInspector] public TextMeshProUGUI basicOutlineWOText;
		[HideInInspector] public TextMeshProUGUI basicOutlineWOTextHighligted;

		// Radial Only Icon Resources
		[HideInInspector] public Image radialOOBackground;
		[HideInInspector] public Image radialOOIcon;

		// Radial Outline Only Icon Resources
		[HideInInspector] public Image radialOutlineOOBorder;
		[HideInInspector] public Image radialOutlineOOFilled;
		[HideInInspector] public Image radialOutlineOOIcon;
		[HideInInspector] public Image radialOutlineOOIconHighlighted;

		// Rounded Resources
		[HideInInspector] public Image roundedBackground;
		[HideInInspector] public TextMeshProUGUI roundedText;

		// Rounded Outline Resources
		[HideInInspector] public Image roundedOutlineBorder;
		[HideInInspector] public Image roundedOutlineFilled;
		[HideInInspector] public TextMeshProUGUI roundedOutlineText;
		[HideInInspector] public TextMeshProUGUI roundedOutlineTextHighligted;

		public enum ButtonType
		{
			BASIC,
			BASIC_ONLY_ICON,
			BASIC_WITH_ICON,
			BASIC_OUTLINE,
			BASIC_OUTLINE_ONLY_ICON,
			BASIC_OUTLINE_WITH_ICON,
			RADIAL_ONLY_ICON,
			RADIAL_OUTLINE_ONLY_ICON,
			ROUNDED,
			ROUNDED_OUTLINE,
		}

		protected override void UpdateTheme()
		{
			if (ThemeSettings.ButtonType == ThemeSettings.ButtonThemeType.Basic)
			{
				if (buttonType == ButtonType.BASIC)
				{
					basicFilled.color = ThemeSettings.ButtonBorderColor;
					basicText.color = ThemeSettings.ButtonFilledColor;
					basicText.font = ThemeSettings.ButtonFont;
					basicText.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.BASIC_ONLY_ICON)
				{
					basicOnlyIconFilled.color = ThemeSettings.ButtonBorderColor;
					basicOnlyIconIcon.color = ThemeSettings.ButtonFilledColor;
				}
				else if (buttonType == ButtonType.BASIC_WITH_ICON)
				{
					basicWithIconFilled.color = ThemeSettings.ButtonBorderColor;
					basicWithIconIcon.color = ThemeSettings.ButtonFilledColor;
					basicWithIconText.color = ThemeSettings.ButtonFilledColor;
					basicWithIconText.font = ThemeSettings.ButtonFont;
					basicWithIconText.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.BASIC_OUTLINE)
				{
					basicOutlineBorder.color = ThemeSettings.ButtonBorderColor;
					basicOutlineFilled.color = ThemeSettings.ButtonBorderColor;
					basicOutlineText.color = ThemeSettings.ButtonBorderColor;
					basicOutlineTextHighligted.color = ThemeSettings.ButtonFilledColor;
					basicOutlineText.font = ThemeSettings.ButtonFont;
					basicOutlineTextHighligted.font = ThemeSettings.ButtonFont;
					basicOutlineText.fontSize = ThemeSettings.ButtonFontSize;
					basicOutlineTextHighligted.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.BASIC_OUTLINE_ONLY_ICON)
				{
					basicOutlineOOBorder.color = ThemeSettings.ButtonBorderColor;
					basicOutlineOOFilled.color = ThemeSettings.ButtonBorderColor;
					basicOutlineOOIcon.color = ThemeSettings.ButtonBorderColor;
					basicOutlineOOIconHighlighted.color = ThemeSettings.ButtonFilledColor;
				}
				else if (buttonType == ButtonType.BASIC_OUTLINE_WITH_ICON)
				{
					basicOutlineWOBorder.color = ThemeSettings.ButtonBorderColor;
					basicOutlineWOFilled.color = ThemeSettings.ButtonBorderColor;
					basicOutlineWOIcon.color = ThemeSettings.ButtonBorderColor;
					basicOutlineWOIconHighlighted.color = ThemeSettings.ButtonFilledColor;
					basicOutlineWOText.color = ThemeSettings.ButtonBorderColor;
					basicOutlineWOTextHighligted.color = ThemeSettings.ButtonFilledColor;
					basicOutlineWOText.font = ThemeSettings.ButtonFont;
					basicOutlineWOTextHighligted.font = ThemeSettings.ButtonFont;
					basicOutlineWOText.fontSize = ThemeSettings.ButtonFontSize;
					basicOutlineWOTextHighligted.fontSize = ThemeSettings.ButtonFontSize;

				}
				else if (buttonType == ButtonType.RADIAL_ONLY_ICON)
				{
					radialOOBackground.color = ThemeSettings.ButtonBorderColor;
					radialOOIcon.color = ThemeSettings.ButtonFilledColor;
				}
				else if (buttonType == ButtonType.RADIAL_OUTLINE_ONLY_ICON)
				{
					radialOutlineOOBorder.color = ThemeSettings.ButtonBorderColor;
					radialOutlineOOFilled.color = ThemeSettings.ButtonBorderColor;
					radialOutlineOOIcon.color = ThemeSettings.ButtonIconColor;
					radialOutlineOOIconHighlighted.color = ThemeSettings.ButtonFilledColor;
				}
				else if (buttonType == ButtonType.ROUNDED)
				{
					roundedBackground.color = ThemeSettings.ButtonBorderColor;
					roundedText.color = ThemeSettings.ButtonFilledColor;
					roundedText.font = ThemeSettings.ButtonFont;
					roundedText.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.ROUNDED_OUTLINE)
				{
					roundedOutlineBorder.color = ThemeSettings.ButtonBorderColor;
					roundedOutlineFilled.color = ThemeSettings.ButtonBorderColor;
					roundedOutlineText.color = ThemeSettings.ButtonBorderColor;
					roundedOutlineTextHighligted.color = ThemeSettings.ButtonFilledColor;
					roundedOutlineText.font = ThemeSettings.ButtonFont;
					roundedOutlineTextHighligted.font = ThemeSettings.ButtonFont;
					roundedOutlineText.fontSize = ThemeSettings.ButtonFontSize;
					roundedOutlineTextHighligted.fontSize = ThemeSettings.ButtonFontSize;
				}
			}

			else if (ThemeSettings.ButtonType == ThemeSettings.ButtonThemeType.Custom)
			{
				if (buttonType == ButtonType.BASIC)
				{
					basicFilled.color = ThemeSettings.ButtonFilledColor;
					basicText.color = ThemeSettings.ButtonTextBasicColor;
					basicText.font = ThemeSettings.ButtonFont;
					basicText.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.BASIC_ONLY_ICON)
				{
					basicOnlyIconFilled.color = ThemeSettings.ButtonFilledColor;
					basicOnlyIconIcon.color = ThemeSettings.ButtonIconBasicColor;
				}
				else if (buttonType == ButtonType.BASIC_WITH_ICON)
				{
					basicWithIconFilled.color = ThemeSettings.ButtonFilledColor;
					basicWithIconIcon.color = ThemeSettings.ButtonIconBasicColor;
					basicWithIconText.color = ThemeSettings.ButtonTextBasicColor;
					basicWithIconText.font = ThemeSettings.ButtonFont;
					basicWithIconText.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.BASIC_OUTLINE)
				{
					basicOutlineBorder.color = ThemeSettings.ButtonBorderColor;
					basicOutlineFilled.color = ThemeSettings.ButtonFilledColor;
					basicOutlineText.color = ThemeSettings.ButtonTextColor;
					basicOutlineTextHighligted.color = ThemeSettings.ButtonTextHighlightedColor;
					basicOutlineText.font = ThemeSettings.ButtonFont;
					basicOutlineTextHighligted.font = ThemeSettings.ButtonFont;
					basicOutlineText.fontSize = ThemeSettings.ButtonFontSize;
					basicOutlineTextHighligted.fontSize = ThemeSettings.ButtonFontSize;
				}
				else if (buttonType == ButtonType.BASIC_OUTLINE_ONLY_ICON)
				{
					basicOutlineOOBorder.color = ThemeSettings.ButtonBorderColor;
					basicOutlineOOFilled.color = ThemeSettings.ButtonFilledColor;
					basicOutlineOOIcon.color = ThemeSettings.ButtonBorderColor;
					basicOutlineOOIconHighlighted.color = ThemeSettings.ButtonFilledColor;
				}

				else if (buttonType == ButtonType.BASIC_OUTLINE_WITH_ICON)
				{
					basicOutlineWOBorder.color = ThemeSettings.ButtonBorderColor;
					basicOutlineWOFilled.color = ThemeSettings.ButtonFilledColor;
					basicOutlineWOIcon.color = ThemeSettings.ButtonIconColor;
					basicOutlineWOIconHighlighted.color = ThemeSettings.ButtonIconHighlightedColor;
					basicOutlineWOText.color = ThemeSettings.ButtonTextColor;
					basicOutlineWOTextHighligted.color = ThemeSettings.ButtonTextHighlightedColor;
					basicOutlineWOText.font = ThemeSettings.ButtonFont;
					basicOutlineWOTextHighligted.font = ThemeSettings.ButtonFont;
					basicOutlineWOText.fontSize = ThemeSettings.ButtonFontSize;
					basicOutlineWOTextHighligted.fontSize = ThemeSettings.ButtonFontSize;
				}

				else if (buttonType == ButtonType.RADIAL_ONLY_ICON)
				{
					radialOOBackground.color = ThemeSettings.ButtonFilledColor;
					radialOOIcon.color = ThemeSettings.ButtonIconBasicColor;
				}

				else if (buttonType == ButtonType.RADIAL_OUTLINE_ONLY_ICON)
				{
					radialOutlineOOBorder.color = ThemeSettings.ButtonBorderColor;
					radialOutlineOOFilled.color = ThemeSettings.ButtonFilledColor;
					radialOutlineOOIcon.color = ThemeSettings.ButtonIconColor;
					radialOutlineOOIconHighlighted.color = ThemeSettings.ButtonIconHighlightedColor;
				}

				else if (buttonType == ButtonType.ROUNDED)
				{
					roundedBackground.color = ThemeSettings.ButtonFilledColor;
					roundedText.color = ThemeSettings.ButtonTextBasicColor;
					roundedText.font = ThemeSettings.ButtonFont;
					roundedText.fontSize = ThemeSettings.ButtonFontSize;
				}

				else if (buttonType == ButtonType.ROUNDED_OUTLINE)
				{
					roundedOutlineBorder.color = ThemeSettings.ButtonBorderColor;
					roundedOutlineFilled.color = ThemeSettings.ButtonFilledColor;
					roundedOutlineText.color = ThemeSettings.ButtonTextColor;
					roundedOutlineTextHighligted.color = ThemeSettings.ButtonTextHighlightedColor;
					roundedOutlineText.font = ThemeSettings.ButtonFont;
					roundedOutlineTextHighligted.font = ThemeSettings.ButtonFont;
					roundedOutlineText.fontSize = ThemeSettings.ButtonFontSize;
					roundedOutlineTextHighligted.fontSize = ThemeSettings.ButtonFontSize;
				}
			}
		}
	}
}