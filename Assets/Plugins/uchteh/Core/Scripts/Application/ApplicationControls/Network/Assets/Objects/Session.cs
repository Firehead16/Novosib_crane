using System;
using UnityEngine;

namespace Core.Global.Network
{
    /// <summary>
    /// Сессия
    /// </summary>
    [Serializable]
    public class Session
    {
        [SerializeField] private int sessionId;
        [SerializeField] private string task;
        [SerializeField] private string progress;
        [SerializeField] private Person person;
        [SerializeField] private int personId;
        [SerializeField] private Computer computer;
        [SerializeField] private int computerId;


        public Computer Computer { get => computer; set => computer = value; }
        public int ComputerId { get => computerId; set => computerId = value; }
        public Person Person { get => person; set => person = value; }
        public int PersonId { get => personId; set => personId = value; }
        public string Progress { get => progress; set => progress = value; }
        public string Task { get => task; set => task = value; }
        public int SessionId { get => sessionId; set => sessionId = value; }
    }
}