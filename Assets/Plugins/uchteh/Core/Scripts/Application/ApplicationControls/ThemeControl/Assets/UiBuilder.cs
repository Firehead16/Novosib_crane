using Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	public class UiBuilder : MonoBehaviour
	{
		public static DropMenu CreateDropMenu(Transform parent, Vector3 dropMenuPosition, bool isNeedDestroy = false)
		{
			var dropMenu = Instantiate(SettingsStorage.ThemeSettings.DropMenuPrefab);
			dropMenu.transform.SetParent(parent);
			dropMenu.transform.position = dropMenuPosition;
			dropMenu.transform.localScale = Vector3.one;
			dropMenu.IsNeedDestroyOnExit = isNeedDestroy;

			return dropMenu;
		}

		public static Tooltip CreateTooltip(Transform parent, Vector3 localscale, string text)
		{
			var toolTip = Instantiate(SettingsStorage.ThemeSettings.ToolTipPrefab);
			toolTip.transform.SetParent(parent);
			toolTip.transform.localScale = localscale;
			toolTip.transform.position = new Vector3((parent.transform.position.x + 150f), parent.transform.position.y + 20);
			toolTip.GetComponentInChildren<RectTransform>().SetHeight(30);
			toolTip.GetComponentInChildren<Text>().text = text;

			return toolTip;
		}

		public static Image CreateImage(Transform parent, Vector3 localscale, Sprite sprite = null)
		{
			Image image = Instantiate(SettingsStorage.ThemeSettings.ImagePrefab, parent);
			image.transform.SetParent(parent);
			image.transform.localScale = localscale;

			if (sprite != null)
			{
				image.GetComponent<Image>().sprite = sprite;
			}
			image.GetComponent<RectTransform>().SetHeight(40);
			return image;
		}

		public static Button CreateButton(Transform parent, Vector3 localscale, string text, TextAnchor textAlign = TextAnchor.MiddleCenter, Sprite sprite = null, Color color = default, int alpha = 1)
		{

			Button abstractBtn = Instantiate(SettingsStorage.ThemeSettings.ButtonPrefab, parent);
			abstractBtn.transform.SetParent(parent);
			abstractBtn.transform.localScale = localscale;

			if (sprite != null)
			{
				abstractBtn.GetComponentInChildren<Image>().sprite = sprite;
			}

			var tmpText = abstractBtn.GetComponentInChildren<ButtonManagerBasic>();
			if (tmpText)
			{
				tmpText.buttonText = text;
				tmpText.GetComponentInChildren<TMP_Text>().color = color;
				tmpText.GetComponentInChildren<TMP_Text>().alpha = alpha;
			}
			else
			{
				var simpleText = abstractBtn.GetComponentInChildren<Text>();
				if (simpleText)
				{
					simpleText.GetComponentInChildren<Text>().text = text;
					simpleText.GetComponentInChildren<Text>().alignment = textAlign;
				}
			}
			abstractBtn.GetComponentInChildren<RectTransform>().SetHeight(40);

			return abstractBtn;
		}

		public static Text CreateText(Transform parent, Vector3 localscale, string text, TextAnchor textAlign = TextAnchor.MiddleCenter)
		{
			Text abstractTxt = Instantiate(SettingsStorage.ThemeSettings.TextPrefab);
			abstractTxt.transform.SetParent(parent);
			abstractTxt.transform.localScale = localscale;
			abstractTxt.GetComponentInChildren<Text>().text = text;
			abstractTxt.GetComponentInChildren<Text>().alignment = textAlign;
			return abstractTxt;
		}

		public static InputField CreateInputField(Transform parent, Vector3 localscale, string text)
		{
			InputField abstractField = Instantiate(SettingsStorage.ThemeSettings.InputFieldPrefab);
			abstractField.transform.SetParent(parent);
			abstractField.transform.localScale = localscale;
			abstractField.GetComponentInChildren<RectTransform>().SetHeight(40);
			abstractField.text = text;
			return abstractField;
		}

		public static Toggle CreateToggle(Transform parent, Vector3 localscale, bool isOn, string text ="Empty")
		{
			Toggle abstractToggle = Instantiate(SettingsStorage.ThemeSettings.TogglePrefab);
			abstractToggle.transform.SetParent(parent);
			abstractToggle.transform.localScale = localscale;
			abstractToggle.isOn = isOn;
		    abstractToggle.GetComponentInChildren<Text>().text = text;

            return abstractToggle;
		}
	} 
}
