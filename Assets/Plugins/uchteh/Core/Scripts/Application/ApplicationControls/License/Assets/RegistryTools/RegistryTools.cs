using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedVariable")]
public static class RegistryTools
{
	[DllImport("advapi32.dll",
		CharSet = CharSet.Unicode,
		EntryPoint = "RegQueryValueExW", SetLastError = true)]
	public static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName,
		int lpReserved, out uint lpType, byte[] lpData, ref int lpcbData);

	[DllImport("advapi32.dll",
		CharSet = CharSet.Unicode,
		EntryPoint = "RegOpenKeyExW", SetLastError = true)]
	public static extern int RegOpenKeyEx(UIntPtr hKey, string subKey,
		uint options, int sam, out UIntPtr phkResult);

	public static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
	public static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
	public static int KEY_QUERY_VALUE = 0x0001;
	public static int KEY_SET_VALUE = 0x0002;
	public static int KEY_CREATE_SUB_KEY = 0x0004;
	public static int KEY_ENUMERATE_SUB_KEYS = 0x0008;
	public static int KEY_WOW64_64KEY = 0x0100;
	public static int KEY_WOW64_32KEY = 0x0200;

	/// <summary>
	/// Прочитать в реестре значение строкового параметра.
	/// </summary>
	/// <param name="rootRegKey">Куст. RegistryTools.HKEY_LOCAL_MACHINE 
	/// или RegistryTools.HKEY_CURRENT_USER.</param>
	/// <param name="regKeyPath">Путь к разделу, в котором находится искомый параметр. 
	/// В пути не указывается куст.</param>
	/// <param name="propName">Наименование искомого строкового параметра.</param>
	/// <returns>Возвращается значение параметра или null, если указанный параметр не найден.</returns>
	public static String GetStringPropertyValue(UIntPtr rootRegKey, String regKeyPath, String propName)
	{
		UIntPtr regKeyHandle;
		if (RegOpenKeyEx(rootRegKey, regKeyPath, 0,
			    KEY_QUERY_VALUE | KEY_WOW64_64KEY, out regKeyHandle) == 0)
		{
			Int32 cbData = 2048;
			Byte[] buf = new Byte[cbData];
			if (RegQueryValueEx(regKeyHandle, propName, 0, out uint type, buf, ref cbData) == 0)
			{
				Encoding encoding = Encoding.ASCII;
				buf = Encoding.Convert(Encoding.Unicode, encoding, buf);
				buf = buf.TakeWhile(n => n != 0).ToArray();
				String text = encoding.GetString(buf, 0, buf.Length);
				return text;
			}
		}
		return null;
	}
}