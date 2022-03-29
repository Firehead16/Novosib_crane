using Core.Ui;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MainMenu
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class LedPanel : MonoBehaviour
	{
		public event Action OnSaveButtonClick;
		public event Action OnExitButtonClick;

		private Hardware hardware;

		[SerializeField]
		private GameObject itemDropDown = null;

		[SerializeField]
		private Button titleExitButton = null;

		[SerializeField]
		private Button exitButton = null;

		[SerializeField]
		private Button saveButton = null;

		[SerializeField]
		private Button enableAllButtons = null;

		[SerializeField]
		private Button disableAllButtons = null;

		[SerializeField]
		private Text hardwareStateText = null;

		[SerializeField]
		private Text dataText = null;

		[SerializeField]
		private Transform ledContent = null;

		private Hardware.Contacts bitCraneLamp9;
		private Hardware.Contacts bitCraneLamp10;
		private Hardware.Contacts bitCraneLamp14;
		private Hardware.Contacts bitCraneLamp15;
		private Hardware.Contacts bitCraneLamp16;
		private Hardware.Contacts bitCraneLamp17;
		private Hardware.Contacts bitCraneLamp20;
		private Hardware.Contacts bitCraneLamp23;
		private Hardware.Contacts bitCraneLamp24;
		private Hardware.Contacts bitCraneLamp25;
		private Hardware.Contacts bitCraneLamp32;
		private Hardware.Contacts bitCraneLamp33;

		private IniReader aIniReader;
		private IniWriter aIniWriter;

		private string appPath = "";


		private void Awake()
		{
			hardware = FindObjectOfType<Hardware>();

			titleExitButton.onClick.AddListener(() => OnExitButtonClick?.Invoke());
			exitButton.onClick.AddListener(() => OnExitButtonClick?.Invoke());
			saveButton.onClick.AddListener(SaveValues);
			enableAllButtons.onClick.AddListener(EnableAllButtons);
			disableAllButtons.onClick.AddListener(DisableAllButtons);
		}

		private void Update()
		{
			if (hardware)
			{
				/*switch (hardware.HardwareState)
				{
					case HardwareState.NotConnected:
						hardwareStateText.text = "Не подключено";
						break;
					case HardwareState.Connected:
						hardwareStateText.text = "Подключено";
						break;
					case HardwareState.Warning:
						hardwareStateText.text = "Ошибка";
						break;
				}*/

				//dataText.text = hardware.Data;
			}
		}

		private void EnableAllButtons()
		{
			//hardware.EnableAll();
		}

		private void DisableAllButtons()
		{
			//hardware.DisableAll();
		}


		public void Show()
		{
			gameObject.SetActive(true);
			PreparePreview();
		}

		/// <summary>
		/// Приготовить интерфейс
		/// </summary>
		private void PreparePreview()
		{
			SetIniParser();
			ReadValues();

			ClearData();
			CreateData();
		}

		/// <summary>
		/// Установить настройки Ini парсера
		/// </summary>
		private void SetIniParser()
		{
			appPath = Path.GetDirectoryName(Application.dataPath);

			//aIniReader = new IniReader(appPath + Hardware.AppConfigPath);
			//aIniWriter = new IniWriter(appPath + Hardware.AppConfigPath);
		}

		/// <summary>
		/// Очистить данные о лампах
		/// </summary>
		private void ClearData()
		{
			foreach (Transform contentChild in ledContent)
			{
				Destroy(contentChild.gameObject);
			}
		}

		/// <summary>
		/// Заполнить данные о лампах
		/// </summary>
		private void CreateData()
		{
			CreateData(0, "bitCraneLamp9", bitCraneLamp9);
			CreateData(1, "bitCraneLamp10", bitCraneLamp10);
			CreateData(2, "bitCraneLamp14", bitCraneLamp14);
			CreateData(3, "bitCraneLamp15", bitCraneLamp15);
			CreateData(4, "bitCraneLamp16", bitCraneLamp16);
			CreateData(5, "bitCraneLamp17", bitCraneLamp17);
			CreateData(6, "bitCraneLamp20", bitCraneLamp20);
			CreateData(7, "bitCraneLamp23", bitCraneLamp23);
			CreateData(8, "bitCraneLamp24", bitCraneLamp24);
			CreateData(9, "bitCraneLamp25", bitCraneLamp25);
			CreateData(10, "bitCraneLamp32", bitCraneLamp32);
			CreateData(11, "bitCraneLamp33", bitCraneLamp33);

			LayoutRebuilder.ForceRebuildLayoutImmediate(ledContent.GetComponent<RectTransform>());
			LayoutRebuilder.MarkLayoutForRebuild(ledContent.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Заполнить данные о лампе
		/// </summary>
		/// <param name="ledIndex"></param>
		/// <param name="text"></param>
		/// <param name="contact"></param>
		private void CreateData(int ledIndex, string text, Hardware.Contacts contact)
		{
			var ledItem = Instantiate(itemDropDown, Vector3.zero, Quaternion.identity, ledContent);
			ledItem.transform.GetChild(0).GetComponent<Text>().text = text;
			Dropdown dropdown = ledItem.transform.GetChild(1).GetComponent<Dropdown>();
			dropdown.ClearOptions();

			List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();

			foreach (var contactName in Enum.GetNames(typeof(Hardware.Contacts)))
			{
				dropdownOptions.Add(new Dropdown.OptionData(contactName));
			}


			dropdown.AddOptions(dropdownOptions);
			dropdown.SetValueWithoutNotify((int)contact);
			dropdown.onValueChanged.AddListener((contactId) =>
			{
				SetLedValue(ledIndex, contactId);
			});


			var enableButton = ledItem.transform.GetChild(2).GetComponent<Button>();
			//enableButton.onClick.AddListener(() => EnableCurrent(ledIndex));
			var disableButton = ledItem.transform.GetChild(3).GetComponent<Button>();
			//disableButton.onClick.AddListener(() => DisableCurrent(ledIndex));
		}

		/*private void EnableCurrent(int ledIndex)
		{
			switch (ledIndex)
			{
				case 0:
					hardware.SetBufferAndSend((int)bitCraneLamp9,true);
					break;
				case 1:
					hardware.SetBufferAndSend((int)bitCraneLamp10,true);
					break;
				case 2:
					hardware.SetBufferAndSend((int)bitCraneLamp14,true);
					break;
				case 3:
					hardware.SetBufferAndSend((int)bitCraneLamp15,true);
					break;
				case 4:
					hardware.SetBufferAndSend((int)bitCraneLamp16,true);
					break;
				case 5:
					hardware.SetBufferAndSend((int)bitCraneLamp17,true);
					break;
				case 6:
					hardware.SetBufferAndSend((int)bitCraneLamp20,true);
					break;
				case 7:
					hardware.SetBufferAndSend((int)bitCraneLamp23,true);
					break;
				case 8:
					hardware.SetBufferAndSend((int)bitCraneLamp24,true);
					break;
				case 9:
					hardware.SetBufferAndSend((int)bitCraneLamp25,true);
					break;
				case 10:
					hardware.SetBufferAndSend((int)bitCraneLamp32,true);
					break;
				case 11:
					hardware.SetBufferAndSend((int)bitCraneLamp33,true);
					break;
			}
		}

		private void DisableCurrent(int ledIndex)
		{
			switch (ledIndex)
			{
				case 0:
					hardware.SetBufferAndSend((int)bitCraneLamp9,false);
					break;
				case 1:
					hardware.SetBufferAndSend((int)bitCraneLamp10,false);
					break;
				case 2:
					hardware.SetBufferAndSend((int)bitCraneLamp14,false);
					break;
				case 3:
					hardware.SetBufferAndSend((int)bitCraneLamp15,false);
					break;
				case 4:
					hardware.SetBufferAndSend((int)bitCraneLamp16,false);
					break;
				case 5:
					hardware.SetBufferAndSend((int)bitCraneLamp17,false);
					break;
				case 6:
					hardware.SetBufferAndSend((int)bitCraneLamp20,false);
					break;
				case 7:
					hardware.SetBufferAndSend((int)bitCraneLamp23,false);
					break;
				case 8:
					hardware.SetBufferAndSend((int)bitCraneLamp24,false);
					break;
				case 9:
					hardware.SetBufferAndSend((int)bitCraneLamp25,false);
					break;
				case 10:
					hardware.SetBufferAndSend((int)bitCraneLamp32,false);
					break;
				case 11:
					hardware.SetBufferAndSend((int)bitCraneLamp33,false);
					break;
			}
		}
		*/

		/// <summary>
		/// Установить значение для лампы
		/// </summary>
		/// <param name="ledIndex"></param>
		/// <param name="contactId"></param>
		private void SetLedValue(int ledIndex, int contactId)
		{
			switch (ledIndex)
			{
				case 0:
					bitCraneLamp9 = (Hardware.Contacts)contactId;
					break;
				case 1:
					bitCraneLamp10 = (Hardware.Contacts)contactId;
					break;
				case 2:
					bitCraneLamp14 = (Hardware.Contacts)contactId;
					break;
				case 3:
					bitCraneLamp15 = (Hardware.Contacts)contactId;
					break;
				case 4:
					bitCraneLamp16 = (Hardware.Contacts)contactId;
					break;
				case 5:
					bitCraneLamp17 = (Hardware.Contacts)contactId;
					break;
				case 6:
					bitCraneLamp20 = (Hardware.Contacts)contactId;
					break;
				case 7:
					bitCraneLamp23 = (Hardware.Contacts)contactId;
					break;
				case 8:
					bitCraneLamp24 = (Hardware.Contacts)contactId;
					break;
				case 9:
					bitCraneLamp25 = (Hardware.Contacts)contactId;
					break;
				case 10:
					bitCraneLamp32 = (Hardware.Contacts)contactId;
					break;
				case 11:
					bitCraneLamp33 = (Hardware.Contacts)contactId;
					break;
			}
		}
		
		/// <summary>
		/// Прочитать значения из Ini-файла
		/// </summary>
		private void ReadValues()
		{
			if (aIniReader != null)
			{
				bitCraneLamp9 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp9", (int)bitCraneLamp9);
				bitCraneLamp10 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp10", (int)bitCraneLamp10);
				bitCraneLamp14 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp14", (int)bitCraneLamp14);
				bitCraneLamp15 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp15", (int)bitCraneLamp15);
				bitCraneLamp16 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp16", (int)bitCraneLamp16);
				bitCraneLamp17 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp17", (int)bitCraneLamp17);
				bitCraneLamp20 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp20", (int)bitCraneLamp20);
				bitCraneLamp23 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp23", (int)bitCraneLamp23);
				bitCraneLamp24 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp24", (int)bitCraneLamp24);
				bitCraneLamp25 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp25", (int)bitCraneLamp25);
				bitCraneLamp32 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp32", (int)bitCraneLamp32);
				bitCraneLamp33 = (Hardware.Contacts)aIniReader.ReadInteger("Bits", "BitCraneLamp33", (int)bitCraneLamp33);
			}
		}

		/// <summary>
		/// Сохранить значения в Ini-файл
		/// </summary>
		private void SaveValues()
		{
			if (aIniWriter != null)
			{
				aIniWriter.WriteInteger("Bits", "BitCraneLamp9", (int)bitCraneLamp9);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp10", (int)bitCraneLamp10);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp14", (int)bitCraneLamp14);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp15", (int)bitCraneLamp15);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp16", (int)bitCraneLamp16);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp17", (int)bitCraneLamp17);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp20", (int)bitCraneLamp20);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp23", (int)bitCraneLamp23);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp24", (int)bitCraneLamp24);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp25", (int)bitCraneLamp25);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp32", (int)bitCraneLamp32);
				aIniWriter.WriteInteger("Bits", "BitCraneLamp33", (int)bitCraneLamp33);

				OnSaveButtonClick?.Invoke();
			}
		}
	}
}