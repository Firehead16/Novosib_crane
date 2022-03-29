using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RansContainersStacks : MonoBehaviour
{
    [SerializeField] private List<Transform> UpperContainers = new List<Transform>();
    void Start()
    {
        foreach (var container in UpperContainers)
        {
            container.gameObject.SetActive(Random.value > 0.4f);
        }
    }
}
