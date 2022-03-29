using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	[CreateAssetMenu(fileName = "New Theme", menuName = "StyleManager/UserInterfaceTheme")]
	public class ThemeSettings : SerializedScriptableObject
	{
		/// <summary>
		/// Динамическое обновление объектов
		/// </summary>
		[HideInInspector]
		public bool enableDynamicUpdate = true;

		/// <summary>
		/// Включить расширенный Color Picker
		/// </summary>
		[HideInInspector]
		public bool enableExtendedColorPicker = true;

		/// <summary>
		/// Включить подсказки в теме
		/// </summary>
		[HideInInspector]
		public bool editorHints = true;

		#region Элементы

		#region Panels

		[HideInInspector]
		public Color PanelColor = new Color(255, 255, 255, 255);

		#endregion

		#region Animated Icon

		[HideInInspector]
		public Color AnimatedIconColor = new Color(255, 255, 255, 255);

		#endregion

		#region Button

		[HideInInspector]
		public ButtonThemeType ButtonType;

		[HideInInspector]
		public TMP_FontAsset ButtonFont;

		[HideInInspector]
		public float ButtonFontSize = 22.5f;

		[HideInInspector]
		public Color ButtonBorderColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonFilledColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonTextBasicColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonTextColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonTextHighlightedColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonIconBasicColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonIconColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ButtonIconHighlightedColor = new Color(255, 255, 255, 255);

		#endregion

		#region Dropdown

		[HideInInspector]
		public TMP_FontAsset DropdownItemFont;

		[HideInInspector]
		public float DropdownItemFontSize = 22.5f;

		[HideInInspector]
		public DropdownThemeType DropdownTheme;

		[HideInInspector]
		public DropdownAnimationType DropdownAnimation;

		[HideInInspector]
		public TMP_FontAsset DropdownFont;

		[HideInInspector]
		public float DropdownFontSize = 22.5f;

		[HideInInspector]
		public Color DropdownColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color DropdownTextColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color DropdownIconColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color DropdownItemColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color DropdownItemTextColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color DropdownItemIconColor = new Color(255, 255, 255, 255);

		#endregion

		#region Horizontal Selector

		[HideInInspector]
		public TMP_FontAsset SelectorFont;

		[HideInInspector]
		public float HSelectorFontSize = 28;

		[HideInInspector]
		public Color SelectorColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SelectorHighlightedColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public bool HSelectorInvertAnimation = false;

		[HideInInspector]
		public bool HSelectorLoopSelection = false;

		#endregion

		#region InputField

		[HideInInspector]
		public TMP_FontAsset InputFieldFont;

		[HideInInspector]
		public float InputFieldFontSize = 28;

		[HideInInspector]
		public Color InputFieldColor = new Color(255, 255, 255, 255);

		#endregion

		#region Modal

		[HideInInspector]
		public TMP_FontAsset ModalWindowTitleFont;

		[HideInInspector]
		public TMP_FontAsset ModalWindowContentFont;

		[HideInInspector]
		public DropdownThemeType ModalTheme;

		[HideInInspector]
		public Color ModalWindowTitleColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ModalWindowDescriptionColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ModalWindowIconColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ModalWindowBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ModalWindowContentPanelColor = new Color(255, 255, 255, 255);

		#endregion

		#region Notification

		[HideInInspector]
		public TMP_FontAsset NotificationTitleFont;

		[HideInInspector]
		public float NotificationTitleFontSize = 22.5f;

		[HideInInspector]
		public TMP_FontAsset NotificationDescriptionFont;

		[HideInInspector]
		public float NotificationDescriptionFontSize = 18;

		[HideInInspector]
		public NotificationThemeType NotificationTheme;

		[HideInInspector]
		public Color NotificationBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color NotificationTitleColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color NotificationDescriptionColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color NotificationIconColor = new Color(255, 255, 255, 255);

		#endregion

		#region Progress Bar

		[HideInInspector]
		public TMP_FontAsset ProgressBarLabelFont;

		[HideInInspector]
		public float ProgressBarLabelFontSize = 25;

		[HideInInspector]
		public Color ProgressBarColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ProgressBarBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ProgressBarLoopBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ProgressBarLabelColor = new Color(255, 255, 255, 255);

		#endregion

		#region ScrollBar

		[HideInInspector]
		public Color ScrollbarColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ScrollbarBackgroundColor = new Color(255, 255, 255, 255);

		#endregion

		#region Slider

		[HideInInspector]
		public TMP_FontAsset SliderLabelFont;

		[HideInInspector]
		public float SliderLabelFontSize = 24;

		[HideInInspector]
		public SliderThemeType SliderTheme;

		[HideInInspector]
		public Color SliderColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SliderBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SliderLabelColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SliderPopupLabelColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SliderHandleColor = new Color(255, 255, 255, 255);

		#endregion

		#region Switch

		[HideInInspector]
		public Color SwitchBorderColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SwitchBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SwitchHandleOnColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color SwitchHandleOffColor = new Color(255, 255, 255, 255);

		#endregion

		#region Toggle

		[HideInInspector]
		public TMP_FontAsset ToggleFont;

		[HideInInspector]
		public float ToggleFontSize = 35;

		[HideInInspector]
		public ToggleThemeType ToggleTheme;

		[HideInInspector]
		public Color ToggleTextColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ToggleBorderColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ToggleBackgroundColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color ToggleCheckColor = new Color(255, 255, 255, 255);

		#endregion

		#region Tooltip

		[HideInInspector]
		public TMP_FontAsset TooltipFont;

		[HideInInspector]
		public float TooltipFontSize = 22;

		[HideInInspector]
		public Color TooltipTextColor = new Color(255, 255, 255, 255);

		[HideInInspector]
		public Color TooltipBackgroundColor = new Color(255, 255, 255, 255);

		#endregion

		#endregion

		#region Перечисления

		public enum ButtonThemeType
		{
			Basic,
			Custom
		}

		public enum DropdownThemeType
		{
			Basic,
			Custom
		}

		public enum DropdownAnimationType
		{
			Fading,
			Sliding,
			Stylish
		}

		public enum ModalWindowThemeType
		{
			Basic,
			Custom
		}

		public enum NotificationThemeType
		{
			Basic,
			Custom
		}

		public enum SliderThemeType
		{
			Basic,
			Custom
		}

		public enum ToggleThemeType
		{
			Basic,
			Custom
		}

		#endregion


		[Header("Префаб выпадающего меню")]
		[SerializeField] private DropMenu dropMenuPrefab = null;
		public DropMenu DropMenuPrefab => dropMenuPrefab;

		[Header("Префаб подсказок")]
		[SerializeField] private Tooltip toolTipPrefab = null;
		public Tooltip ToolTipPrefab => toolTipPrefab;

		[Header("Префаб текста")]
		[SerializeField] private Text textPrefab = null;
		public Text TextPrefab => textPrefab;

		[Header("Префаб изображений")]
		[SerializeField] private Image imagePrefab = null;
		public Image ImagePrefab => imagePrefab;

		[Header("Префаб кнопок")]
		[SerializeField] private Button buttonPrefab = null;
		public Button ButtonPrefab => buttonPrefab;

		[Header("Префаб тоглов")]
		[SerializeField] private Toggle togglePrefab = null;
		public Toggle TogglePrefab => togglePrefab;

		[Header("Префаб слайдеров")]
		[SerializeField] private Slider sliderPrefab = null;
		public Slider SliderPrefab => sliderPrefab;

		[Header("Префаб скролбаров")]
		[SerializeField] private Scrollbar scrollbarPrefab = null;
		public Scrollbar ScrollbarPrefab => scrollbarPrefab;

		[Header("Префаб выпадающего меню")]
		[SerializeField] private Dropdown dropdownPrefab = null;
		public Dropdown DropdownPrefab => dropdownPrefab;

		[Header("Префаб выпадающего меню")]
		[SerializeField] private InputField inputFieldPrefab = null;
		public InputField InputFieldPrefab => inputFieldPrefab;

		[Header("Префаб прокручеваемой области")]
		[SerializeField] private ScrollRect scrollViewPrefab = null;
		public ScrollRect ScrollViewPrefab => scrollViewPrefab;

		[Header("Префаб перехода прозрачности панели")]
		[SerializeField] private float panelShowAndHideDuration = default;
		public float PanelShowAndHideDuration => panelShowAndHideDuration;

		[Header("Подсветка")]
		[SerializeField] private float fadeDuration = default;
		public float FadeDuration => fadeDuration;


		[Header("Подсветка в таблице")]
		[SerializeField] private Color tableLineColor = default;
		public Color TableLineColor => tableLineColor;

		[Button]
		public void SetTheme()
		{
			SettingsStorage.ThemeSettings = this;
		}
	}
}