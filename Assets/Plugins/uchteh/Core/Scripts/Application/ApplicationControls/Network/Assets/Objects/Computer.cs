using System;
using UnityEngine;

namespace Core.Global.Network
{
    /// <summary>
    /// Компьютер
    /// </summary>
    [Serializable]
    public class Computer
    {
        [SerializeField] public int ComputerId;
        [SerializeField] public string Name;
        [SerializeField] public string IpAddress;
    }
}