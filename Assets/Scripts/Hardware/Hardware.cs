using System;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using TMPro;

public class Hardware : MonoBehaviour
{
    public enum Contacts
    {
        //Buffer[0]
        Pd0 = 0,
        Pd1 = 1,
        Pd2 = 2,
        Pd3 = 3,
        Pd4 = 4,
        Pd5 = 5,
        Pd6 = 6,
        Pd7 = 7,

        //Buffer[1]
        Pd8 = 8,
        Pd9 = 9,
        Pd10 = 10,
        Pd11 = 11,
        Pd12 = 12,
        Pd13 = 13,
        Pd14 = 14,
        Pd15 = 15,

        //Buffer[2]
        Pc0 = 16,
        Pc1 = 17,
        Pc2 = 18,
        Pc3 = 19,
        Pc4 = 20,
        Pc5 = 21,
        Pc6 = 22,
        Pc7 = 23,

        //Buffer[3]
        Pc8 = 24,
        Pc9 = 25,
        Pc10 = 26,
        Pc11 = 27,
        Pc12 = 28,
        Pf9 = 29,
        Pf10 = 30,
        Pf4 = 31,

        //Buffer[4]
        Pb0 = 32,
        Pb1 = 33,
        Pb2 = 34,
        Pb4 = 35,
        Pb5 = 36,
        Pb8 = 37,
        Pb9 = 38,
        Pb10 = 39,


        //Buffer[5]
        Pb11 = 40,
        Pb12 = 41,
        Pb13 = 42,
        Pb14 = 43,
        Pb15 = 44,
        Pf2 = 45,
        Pc13 = 46,

        //Buffer[6]
        Pa1 = 47,
        Pa2 = 48,
        Pa4 = 49,
        Pa6 = 50,
        Pa7 = 51,
        Pa8 = 52,
        Pa9 = 53,
        Pa10 = 54,
    }

    IniReader aIniReader;
    IniWriter aIniWriter;

    public static Hardware Instance;
    
    //IMPORTANT
    private bool isNewPlate = true;

    SerialPort myDevice;
    string ComPort = "";
    int SerialSpeed = 19200;
    string AppPath = "";
    byte[] buffer;
    byte[] oldbuffer;

    int BitCraneControlEnabled = 0;
    int BitCraneReadyEnabled = 1;
    int BitCraneBrakeFail = 2;
    int BitCraneOverWeight = 3;
    int BitCraneOverWeight2 = 4;
    int BitCraneMoveEnabled = 5;
    int BitCraneTowerEnabled = 6;
    int BitCraneArrowEnabled = 7;
    int BitCraneRopeEnabled = 8;
    int BitCraneRailEnabled = 9;
    int BitCraneControlerFail = 10;
    int BitCraneRopeFail = 11;
    int BitCraneArrowFail = 12;
    int BitCraneTowerFail = 13;
    int BitCraneRailFail = 14;
    int BitCraneSlab1Green = 15;
    int BitCraneSlab2Green = 16;
    int BitCraneSlab1Red = 17;
    int BitCraneSlab2Red = 18;

    [SerializeField]
    TMP_Text PultConnected;
    // [SerializeField]
    // private CraneMovement Control;

    //public bool testEnabled = false;
    public bool testSend = false;

    List<bool[]> bitTable = new List<bool[]>();
    
    ////////////////////////
    /// для новой платы
    
    TcpClient client = new TcpClient();
    int PultPort = 65000;
    //Form1 parentForm;
    //public byte[] Buffer; //Уже есть выше
    
    public bool IsConnected()
    {
        return client.Connected;
    }

    /*public Network(int Port)//, Form1 parent) // Инициализация
    {
        //parentForm = parent;
        PultPort = Port;
        Buffer = new byte[9];
    }*/
    
    public bool Connect() // Подключение
    {
        Debug.Log("Connect_new");
        
        bool result = false;

        client.Connect("127.0.0.1", PultPort);

        result = client.Connected;

        Debug.Log("Connected " + client.Connected);

        return result;
    }

    public bool Disconnect() // Отключение
    {
        bool result = false;

        client.Close();

        result = !client.Connected;

        Debug.Log("Disconnected " + result);

        return result;
    }
    
    public bool Send() // Посылка пакета
    {
        bool result = false; //exception - disconnect - connect

        NetworkStream serverStream = client.GetStream();
        if (serverStream != null && client.Connected)
        {
            buffer[8] = 0x0d; //important
            serverStream.Write(buffer, 0, 9);
            result = true;
        }
        //Debug.Log("Sended something");
        return result;

        //try
        //{
        //    NetworkStream serverStream = client.GetStream();
        //    if (serverStream != null && client.Connected)
        //    {
        //        buffer[8] = 0x0d; //тестовое значение
        //        serverStream.Write(buffer, 0, 9);
        //        result = true;
        //    }
        //    //Debug.Log("Sended something");
        //    return result;
        //}
        //catch (Exception e)
        //{
        //    result = false;
        //    Debug.Log(e);
        //    //throw;
        //    Disconnect();
        //    Connect();
        //    return result;
        //}
    }
    
    //////////////////////////
    /// ниже - для старой платы

    /// <summary>
    /// Установить данные в буфер
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="state"></param>
    private void SetBuffer(int pin, bool state)
    {
        bitTable.Clear();

        for (int i = 0; i < buffer.Length; i++)
        {
            bool[] bits = GetBits(buffer[i]);
            bitTable.Add(bits);
        }

        //for (int i = 0; i < buffer.Length; i++)
        //{
        //    //TODO сравнивать и вызывать ChangeBuffer здесь

        //    if (oldbuffer[i] != buffer[i]) 
        //    { 
        //        oldbuffer[i] = buffer[i];

        //    }

        //    bool[] bits = GetBits(buffer[i]);
        //    bitTable.Add(bits);
        //}

        if (pin >= 0 && pin <= 7)
        {
            BitArray bitArray = new BitArray(bitTable[0]);
            bitArray.Set(pin, state);
            buffer[0] = ConvertToByte(bitArray);
        }

        if (pin >= 8 && pin <= 15)
        {
            BitArray bitArray = new BitArray(bitTable[1]);
            bitArray.Set(pin - 8, state);
            buffer[1] = ConvertToByte(bitArray);
        }

        if (pin >= 16 && pin <= 23)
        {
            BitArray bitArray = new BitArray(bitTable[2]);
            bitArray.Set(pin - 16, state);
            buffer[2] = ConvertToByte(bitArray);
        }

        if (pin >= 24 && pin <= 31)
        {
            BitArray bitArray = new BitArray(bitTable[3]);
            bitArray.Set(pin - 24, state);
            buffer[3] = ConvertToByte(bitArray);
        }

        if (pin >= 32 && pin <= 39)
        {
            BitArray bitArray = new BitArray(bitTable[4]);
            bitArray.Set(pin - 32, state);
            buffer[4] = ConvertToByte(bitArray);
        }

        if (pin >= 40 && pin <= 46)
        {
            BitArray bitArray = new BitArray(bitTable[5]);
            bitArray.Set(pin - 40, state);
            buffer[5] = ConvertToByte(bitArray);
        }

        if (pin >= 47 && pin <= 54)
        {
            BitArray bitArray = new BitArray(bitTable[6]);
            bitArray.Set(pin - 47, state);
            buffer[6] = ConvertToByte(bitArray);
        }
        
        if (pin >= 55 && pin <= 62)
        {
            BitArray bitArray = new BitArray(bitTable[7]);
            bitArray.Set(pin - 55, state);
            buffer[7] = ConvertToByte(bitArray);
        }
        
        if (pin >= 63 && pin <= 70)
        {
            BitArray bitArray = new BitArray(bitTable[8]);
            bitArray.Set(pin - 63, state);
            buffer[8] = ConvertToByte(bitArray);
        }
        
        if (pin >= 71 && pin <= 78)
        {
            BitArray bitArray = new BitArray(bitTable[9]);
            bitArray.Set(pin - 71, state);
            buffer[9] = ConvertToByte(bitArray);
        }

        //buffer[1] |= (byte)(1 << pin);
    }

    void SetOldBuffer()
    {
        for (int i = 0; i < buffer.Length; i++)
        {
            //TODO сравнивать и вызывать ChangeBuffer здесь
            oldbuffer[i] = buffer[i];
            bool[] bits = GetBits(buffer[i]);
            bitTable.Add(bits);
        }
    }

    void Start()
    {
        Instance = this;
        
        if (isNewPlate)
        {
            AppPath = Path.GetDirectoryName(Application.dataPath);
            aIniReader = new IniReader(AppPath + "\\Config\\crane.ini");
            aIniWriter = new IniWriter(AppPath + "\\Config\\crane.ini");
            
            buffer = new byte[9];
            oldbuffer = new byte[9];
            LoadConfig();
            Connect();
        }
        else
        {
            AppPath = Path.GetDirectoryName(Application.dataPath);
            aIniReader = new IniReader(AppPath + "\\Config\\crane.ini");
            aIniWriter = new IniWriter(AppPath + "\\Config\\crane.ini");

            buffer = new byte[7];
            oldbuffer = new byte[7];

            LoadConfig();
            InitHardware();

            Debug.Log("Путь: " + AppPath);
            if (CraneControl.Instance == null) Debug.Log("Crane Control: не найден");
        }

        Debug.Log("Buffer initiated " + buffer.Length);
        ClearBuffer();
        SendBuffer();
    }

    /// <summary>
    /// Получить массив битов из числа
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool[] GetBits(int value)
    {
        bool[] bits = new bool[8];
        for (int j = 7; j >= 0; j--)
        {
            byte bit = (byte)((value >> j) & 1u);
            bits[j] = bit == 1;
        }
        return bits;
    }

    /// <summary>
    /// Конвертировать массив битов в байт
    /// </summary>
    /// <param name="bits"></param>
    /// <returns></returns>
    private byte ConvertToByte(BitArray bits)
    {
        if (bits.Count != 8)
        {
            throw new ArgumentException("Не то количество битов " + bits.Count);
        }
        byte[] bytes = new byte[1];
        bits.CopyTo(bytes, 0);
        return bytes[0];
    }

    /// <summary>
    /// Поменялись ли передаваемые данные в буфере
    /// </summary>
    /// <returns></returns>
    private bool BufferChanged()
    {
        bool result = false;
        for (int i = 0; i < buffer.Length; i++)
        {
            if (oldbuffer[i] != buffer[i])
            {
                result = true;

                break;
            }
        }
        //Debug.Log("BufferChanged" + result);
        return result;
    }


    public IEnumerator Test()
    {
        testSend = true;

        SetBuffer(BitCraneControlEnabled, true);
        SetBuffer(BitCraneReadyEnabled, true);
        SetBuffer(BitCraneBrakeFail, true);
        SetBuffer(BitCraneOverWeight, true);
        SetBuffer(BitCraneOverWeight2, true);
        SetBuffer(BitCraneMoveEnabled, true);
        SetBuffer(BitCraneTowerEnabled, true);
        SetBuffer(BitCraneArrowEnabled, true);
        SetBuffer(BitCraneRopeEnabled, true);
        SetBuffer(BitCraneRailEnabled, true);
        SetBuffer(BitCraneControlerFail, true);

        SetBuffer(BitCraneRopeFail, true);
        SetBuffer(BitCraneArrowFail, true);
        SetBuffer(BitCraneTowerFail, true);
        SetBuffer(BitCraneRailFail, true);

        SetBuffer(BitCraneSlab1Green, true);
        SetBuffer(BitCraneSlab2Green, true);
        SetBuffer(BitCraneSlab1Red, true);
        SetBuffer(BitCraneSlab2Red, true);
        SendBuffer();
        yield return new WaitForSeconds(2);
        ClearBuffer();
        SendBuffer();
        testSend = false;
    }

    /// <summary>
    /// Очистка буфера данных
    /// </summary>
    private void ClearBuffer()
    {
        if (isNewPlate)
        {
            buffer[0] = 0;
            buffer[1] = 0;
            buffer[2] = 0;
            buffer[3] = 0;
            buffer[4] = 0;
            buffer[5] = 0;
            buffer[6] = 0;
            buffer[7] = 0;
            //buffer[8] = 0;
            //buffer[9] = 0;
        }
        else
        {
            buffer[0] = 0;
            buffer[1] = 0;
            buffer[2] = 0;
            buffer[3] = 0;
            buffer[4] = 0;
            //buffer[5] = 0;
            buffer[6] = 0;
        }
    }

    void InitHardware()
    {
        if (isNewPlate)
        {
            //TODO ?
        }
        else
        {
            myDevice = new SerialPort(ComPort, SerialSpeed, Parity.None, 8, StopBits.One);
            try
            {
                Debug.Log(myDevice.PortName + " CONNECTED");
                myDevice.Open();
            }
            catch (Exception e)
            {
                Debug.Log("COM not found");
            }
        }
    }

    /// <summary>
    /// Отправка данных из буфера на порт
    /// </summary>
    private void SendBuffer()
    {
        if (isNewPlate)
        {
            if (IsConnected())
            {
                Send();
                Debug.Log("SendBuffer");
            }
            else
                Debug.Log("LEDs not connected");
        }
        else
        {
            if (myDevice.IsOpen) myDevice.Write(buffer, 0, 6); //7?
        }
    }

    void LoadConfig()
    {
        AppPath = Path.GetDirectoryName(Application.dataPath);
        aIniReader.SetPath(AppPath + "\\Config\\crane.ini");

        ComPort = aIniReader.ReadString("Hardware", "Port", "COM1");
        SerialSpeed = aIniReader.ReadInteger("Hardware", "Speed", 19200);

        BitCraneControlEnabled = aIniReader.ReadInteger("Bits", "BitCraneControlEnabled", 0);
        BitCraneReadyEnabled = aIniReader.ReadInteger("Bits", "BitCraneReadyEnabled", 1);
        BitCraneBrakeFail = aIniReader.ReadInteger("Bits", "BitCraneBrakeFail", 2);
        BitCraneOverWeight = aIniReader.ReadInteger("Bits", "BitCraneOverWeight", 3);
        BitCraneOverWeight2 = aIniReader.ReadInteger("Bits", "BitCraneOverWeight2", 4);
        BitCraneMoveEnabled = aIniReader.ReadInteger("Bits", "BitCraneMoveEnabled", 5);
        BitCraneTowerEnabled = aIniReader.ReadInteger("Bits", "BitCraneTowerEnabled", 6);
        BitCraneArrowEnabled = aIniReader.ReadInteger("Bits", "BitCraneArrowEnabled", 7);
        BitCraneRopeEnabled = aIniReader.ReadInteger("Bits", "BitCraneRopeEnabled", 8);
        BitCraneRailEnabled = aIniReader.ReadInteger("Bits", "BitCraneRailEnabled", 9);
        BitCraneControlerFail = aIniReader.ReadInteger("Bits", "BitCraneControlerFail", 10);

        BitCraneRopeFail = aIniReader.ReadInteger("Bits", "BitCraneRopeFail", 11);
        BitCraneArrowFail = aIniReader.ReadInteger("Bits", "BitCraneArrowFail", 12);
        BitCraneTowerFail = aIniReader.ReadInteger("Bits", "BitCraneTowerFail", 13);
        BitCraneRailFail = aIniReader.ReadInteger("Bits", "BitCraneRailFail", 14);

        BitCraneSlab1Green = aIniReader.ReadInteger("Bits", "BitCraneSlab1Green", 15);
        BitCraneSlab2Green = aIniReader.ReadInteger("Bits", "BitCraneSlab2Green", 16);
        BitCraneSlab1Red = aIniReader.ReadInteger("Bits", "BitCraneSlab1Red", 17);
        BitCraneSlab2Red = aIniReader.ReadInteger("Bits", "BitCraneSlab2Red", 18);
    }

    // Update is called once per frame

    void UpdateLamps()
    {
        //Debug.Log("BitCraneControlEnabled " + Control.SystemState.CraneControlEnabled);
        if (CraneControl.Instance != null && !testSend)
        {
            SetBuffer(BitCraneControlEnabled, CraneControl.craneState.CraneControlEnabled);
            SetBuffer(BitCraneReadyEnabled, CraneControl.craneState.CraneReadyEnabled);
            SetBuffer(BitCraneBrakeFail, CraneControl.craneState.CraneBrakeFail);
            SetBuffer(BitCraneOverWeight, CraneControl.craneState.CraneOverWeight);
            SetBuffer(BitCraneOverWeight2, CraneControl.craneState.CraneOverWeight2);
            SetBuffer(BitCraneMoveEnabled, CraneControl.craneState.CraneMoveEnabled);
            SetBuffer(BitCraneTowerEnabled, CraneControl.craneState.CraneTowerEnabled);
            SetBuffer(BitCraneArrowEnabled, CraneControl.craneState.CraneArrowEnabled);
            SetBuffer(BitCraneRopeEnabled, CraneControl.craneState.CraneRopeEnabled);
            SetBuffer(BitCraneRailEnabled, CraneControl.craneState.CraneRailEnabled);
            SetBuffer(BitCraneControlerFail, CraneControl.craneState.CraneControllerFail);

            SetBuffer(BitCraneRopeFail, CraneControl.craneState.CraneRopeFail);
            SetBuffer(BitCraneArrowFail, CraneControl.craneState.CraneArrowFail);
            SetBuffer(BitCraneTowerFail, CraneControl.craneState.CraneTowerFail);
            SetBuffer(BitCraneRailFail, CraneControl.craneState.CraneRailFail);

            SetBuffer(BitCraneSlab1Green, CraneControl.craneState.CraneSlab1Green);
            SetBuffer(BitCraneSlab2Green, CraneControl.craneState.CraneSlab2Green);
            SetBuffer(BitCraneSlab1Red, CraneControl.craneState.CraneSlab1Red);
            SetBuffer(BitCraneSlab2Red, CraneControl.craneState.CraneSlab2Red);
        }

        if (BufferChanged() && !testSend)
        {
            SendBuffer();
        }

        //if (testEnabled) Test();

        SetOldBuffer();
    }
    void Update()
    {
        UpdateLamps();

        //Send();

        if ((isNewPlate && IsConnected() && !testSend) || (!isNewPlate && myDevice.IsOpen && !testSend)) //TODO проверить попадание
        {
            PultConnected.text = "Пульт подключен";
            //SendBuffer();
            //ClearBuffer();
        }
        else PultConnected.text = "Пульт не подключен";
    }

    void OnDestroy()
    {
        ClearBuffer();
        SendBuffer();
        if (isNewPlate)
        {
            Disconnect();
        }
        else
        {
            if (myDevice != null && myDevice.IsOpen)
            {
                Debug.Log("COM отключен: " + ComPort);
                myDevice.Close();
            }
        }
    }
    
    
}
