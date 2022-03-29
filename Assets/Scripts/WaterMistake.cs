using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMistake : MonoBehaviour
{
    private bool alreadyDid;

    [SerializeField] private bool isCargo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && !alreadyDid)
        {
            QuestStorage.Instance.AddMistake((isCargo ? "Груз " : "Грузозахват ") + "был погружен в воду");
            Debug.Log((isCargo ? "Груз " : "Грузозахват ") + "был погружен в воду");
        }
    }
}
