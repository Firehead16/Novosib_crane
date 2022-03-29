using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class F3CameraItself : MonoBehaviour
{
    public bool isCollided;
    
    private List<Transform> triggerVisitors = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerVisitors.Contains(other.transform)) triggerVisitors.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerVisitors.Contains(other.transform)) triggerVisitors.Remove(other.transform);
    }

    private void Update() => isCollided = triggerVisitors.Any();
}
