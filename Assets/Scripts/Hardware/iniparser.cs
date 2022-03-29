using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

public class IniReader
{

    public string m_szFileName;
    public string temp;

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileInt(String Section, String Key, int Default, String FilePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(String Section, String Key, String Default, StringBuilder retVal, int Size, String FilePath);


        public IniReader(string szFileName)
        {
            m_szFileName = szFileName;
        }

        public void SetPath(string szFileName)
        {
            m_szFileName = szFileName;
        }

        public int ReadInteger(string szSection, string szKey, int iDefaultValue)
        {
            int iResult = GetPrivateProfileInt(szSection,  szKey, iDefaultValue, m_szFileName); 
            return iResult;
        }

        public float ReadFloat(string szSection, string szKey, float fltDefaultValue)
        {
            StringBuilder szResult = new StringBuilder(500);
            string szDefault;
            float fltResult = 0.0f;
            szDefault = string.Format("{0}", fltDefaultValue);
            GetPrivateProfileString(szSection,  szKey, szDefault, szResult, 255, m_szFileName);
            fltResult = (float)Convert.ToDouble(szResult.ToString());
            return fltResult;
        }

        public bool ReadBoolean(string szSection, string szKey, bool bolDefaultValue)
        {
            StringBuilder szResult = new StringBuilder(500);
            string  szDefault, result;
            bool bolResult;
            if(bolDefaultValue) szDefault = "true";
            else szDefault = "false";
            GetPrivateProfileString(szSection, szKey, szDefault, szResult, 255, m_szFileName);
            result = szResult.ToString();
            temp = result;
            if (string.Compare(result, "True") == 0 || string.Compare(result, "true") == 0) bolResult = true;
            else bolResult = false;   
            return bolResult;
        }

        public string ReadString(string szSection, string szKey, string szDefaultValue)
        {
            StringBuilder szResult = new StringBuilder(500);

            GetPrivateProfileString(szSection,  szKey, szDefaultValue, szResult, 255, m_szFileName); 
            string str = szResult.ToString();


            return szResult.ToString();

        }
}


class IniWriter
{
        string m_szFileName;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public void SetPath(string szFileName)
        {
            m_szFileName = szFileName;
        }

        public IniWriter(string szFileName)
        {
            m_szFileName = szFileName;
        }

        public void WriteInteger(string szSection, string szKey, int iValue)
        {
         string szValue;
         szValue = string.Format("{0}", iValue);
         WritePrivateProfileString(szSection,  szKey, szValue, m_szFileName); 
        }
    
        public void WriteFloat(string szSection, string szKey, float fltValue)
        {
         string szValue;
         szValue = string.Format("{0}", fltValue);
         WritePrivateProfileString(szSection,  szKey, szValue, m_szFileName); 
        }

        public void WriteBoolean(string szSection, string szKey, bool bolValue)
        {
         string szValue;
         if(bolValue) szValue = "true";
         else szValue = "false";
         WritePrivateProfileString(szSection,  szKey, szValue, m_szFileName); 
        }

        public void WriteString(string szSection, string szKey, string szValue)
        {
         WritePrivateProfileString(szSection,  szKey, szValue, m_szFileName);
        }

}