using System.IO;
using Core.Settings;
using Core.Ui;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Инициализирует панели и проверяет серийный ключ 
/// </summary>
public sealed class LicenseControl : CanvasControl<LicensePanel>, ILisense
{

	/// <summary>
	/// Выставляется в инспекторе в зависимости от того, нужна ли лицензия
	/// </summary>
	[SerializeField, BoxGroup("Параметры инициализации")] private bool licenseNeedCheck = default;
	public bool IsInitialized { get; set; }

	private LicensePanel licensePanel;
	private string fileName;
	private string productIdString;
	private char[] productId;
	private char[] currentSerialKey;
	private char[] realSerialKey;
	private bool licenseFail;
	private bool badFile;


	/// <summary>
	/// Загружает LicensePanel, проверяет файл лицензии и в случае, если приложение неактивировано то показывает LicensePanel. Если активировано, то AccessPanel 
	/// </summary>
	protected override void OnItemsInitialised()
	{
		licensePanel = GetDefaultPanel();
		if (!licenseNeedCheck)
		{
			LicenseFail = false;
			return;
		}
		productId = new char[256];
		realSerialKey = new char[1000];
		currentSerialKey = new char[1000];
		fileName = Application.dataPath + "/license.bin";

		if (!File.Exists(fileName))
		{
			File.Create(fileName).Dispose();
		}

		licensePanel.ProductIdText = productIdString = GetPoductIdString();
		licensePanel.OnSaveLicenseButtonClick += CheckSerialKeyText;
		licensePanel.OnCancelLicenseButtonClick += LicenseAbort;
		realSerialKey = GetRealSerialKey();
		currentSerialKey = ReadLicenseFile();
		CheckLicense();
	}

	public override void Unload()
	{
		base.Unload();
		licensePanel.OnSaveLicenseButtonClick -= CheckSerialKeyText;
		licensePanel.OnCancelLicenseButtonClick -= LicenseAbort;
	}


	public bool LicenseFail
	{
		get { return licenseFail; }
		set
		{
			licenseFail = value;
			if (licenseFail)
			{
				licensePanel.LicenseFilePathText = "License fail!";
				Debug.Log("License fail!");
				licensePanel.Show();

				licensePanel.ProductIdText = productIdString;
				licensePanel.SerialKeyText = "";
				badFile = false;
			}
			else
			{
				SaveLicense();
				Debug.Log("License success!");
				Debug.Log("License saved to " + fileName);
				licensePanel.LicenseFilePathText = "License success! License saved to: \n" + fileName;
				licensePanel.Hide();
			}

		}
	}


	private void LicenseAbort()
	{
		LicenseFail = true;
		Application.Quit();
	}


	/// <summary>
	/// Проверить лицензию  из текстого поля на UI
	/// </summary>
	private void CheckSerialKeyText()
	{
		currentSerialKey = licensePanel.SerialKeyText.ToCharArray();
		CheckLicense();
	}


	/// <summary>
	/// Получить серийный ключ
	/// </summary>
	private string GetPoductIdString()
	{
		string id = RegistryTools.GetStringPropertyValue(RegistryTools.HKEY_LOCAL_MACHINE,
			@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductId");

		string installDate = "161226851216";
		return "OP2-" + id + "-" + installDate;
	}


	/// <summary>
	/// Проверить лицензию
	/// </summary>
	private void CheckLicense()
	{
		LicenseFail = !CheckCurrenAndRealSerialKey();
	}


	/// <summary>
	/// Получить Id продукта
	/// </summary>
	private char[] GetRealSerialKey()
	{
		string temp;
		string result = "";
		productId = productIdString.ToCharArray();
		int c = productId.Length;

		for (int i = 0; i < productId.Length; i++)
		{
			temp = string.Format("{0:X4}", (uint)((productId[i] * (c - i)) * 2));
			result += temp;
		}

		for (int i = 0; i < productId.Length; i++)
		{
			temp = string.Format("{0:X4}", (uint)((productId[i] + 31) * 2));
			result += temp;
		}

		for (int i = 0; i < productId.Length; i++)
		{
			temp = string.Format("{0:X4}", (uint)((productId[i] * (c - i)) * 3));
			result += temp;
		}

		for (int i = 0; i < productId.Length; i++)
		{
			temp = string.Format("{0:X4}", (uint)((productId[i] * (c - i)) * 4));
			result += temp;
		}

		for (int i = 0; i < productId.Length; i++)
		{
			temp = string.Format("{0:X4}", (uint)((productId[i] * (c - i)) * 7));
			result += temp;
		}

		for (int i = 0; i < productId.Length; i++)
		{
			temp = string.Format("{0:X4}", (uint)((productId[i] * (c - i))));
			result += temp;
		}
		return result.ToCharArray();
	}


	/// <summary>
	/// Проверить файл активации
	/// </summary>
	private char[] ReadLicenseFile()
	{
		var result = new char[1000];
		if (File.Exists(fileName))
		{
			var sr = new StreamReader(fileName);

			sr.ReadBlock(result, 0, 1000);
			sr.Dispose();
		}
		else
		{
			badFile = true;
		}
		return result;
	}


	/// <summary>
	/// Проверить совпадение серийного ключа и кода активации
	/// </summary>
	/// <returns></returns>
	private bool CheckCurrenAndRealSerialKey()
	{
		bool result = false;
		if (!badFile && realSerialKey.Length >= 759 && currentSerialKey.Length >= 759)
		{
			for (int i = 0; i < 760; i++)
			{
				if (currentSerialKey[i] == realSerialKey[i]) result = true;
				else
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}


	/// <summary>
	/// Записать файл активации
	/// </summary>
	private void SaveLicense()
	{
		StreamWriter wr;
		{
			wr = new StreamWriter(fileName);
			wr.WriteLine(currentSerialKey);
			wr.Close();
		}
	}
}