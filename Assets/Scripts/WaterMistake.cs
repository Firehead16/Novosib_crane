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
            QuestStorage.Instance.AddMistake((isCargo ? "���� " : "����������� ") + "��� �������� � ����");
            Debug.Log((isCargo ? "���� " : "����������� ") + "��� �������� � ����");
        }
    }
}
