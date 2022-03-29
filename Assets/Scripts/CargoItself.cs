using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class CargoItself : MonoBehaviour
{
    public List<Transform> alreadyHit = new List<Transform>();

    [HideInInspector]
    public RaycastHit Hit;

    [SerializeField] public float startRay;

    private void OnCollisionEnter(Collision other)
    {
        if (GetComponentInParent<Cargo>().IsGrabbed && !alreadyHit.Contains(other.transform) &&
            other.transform != Hit.transform && !other.transform.CompareTag("Untagged") &&
            !other.transform.CompareTag("Ship"))
        {
            Debug.Log("Hit something");
            
            if (other.transform.CompareTag("Cargo") && !QuestControl.Instance.placeTrigger.ContainsCargo)
                QuestStorage.Instance.AddMistake("������������ � ������ ������ ");

            if (other.transform.CompareTag("Obstacle"))
                QuestStorage.Instance.AddMistake("������������ � ������������ ");
            
            if (other.transform.CompareTag("Crane"))
                QuestStorage.Instance.AddMistake("������������ � ���������� ����� ");

            StartCoroutine(GetComponentInParent<Cargo>().cargoMistake());
            alreadyHit.Add(other.transform);
        }
    }

    private void Update()
    {
        
        if (GetComponentInParent<Cargo>().IsAvaliable || GetComponentInParent<Cargo>().IsGrabbed)
        {
            Debug.DrawRay(transform.position + Vector3.down * startRay, Vector3.down * 2);
            Physics.Raycast(transform.position + Vector3.down * startRay, Vector3.down * 2, out Hit, 2.0f);
            //Debug.Log("hit_d " + Hit.distance);
        }
        
        if (GetComponentInParent<Cargo>().IsGrabbed && GetComponentInParent<Cargo>().AlreadyLifted &&
            !Hit.transform.SafeIsUnityNull() && !QuestControl.Instance.placeTrigger.ContainsCargo)
        { 
            if (!Hit.collider.gameObject.GetComponent<PlaceTrigger>() &&
                !QuestControl.Instance.placeTrigger.ContainsCargo &&
                !alreadyHit.Contains(Hit.transform)) //���� ��� ������������ 
            {
                alreadyHit.Add(Hit.transform);
                Debug.Log("����������� ����� ����� ��� ������������");
                //QuestStorage.Instance.AddMistake("����������� ����� ����� ��� ������������"); ?
            }
        }

        if (GetComponentInParent<Cargo>().IsGrabbed && !GetComponentInParent<Cargo>().AlreadyLifted &&
            Hit.transform.SafeIsUnityNull())
        {
            GetComponentInParent<Cargo>().AlreadyLifted = true;
        }
    }
}
