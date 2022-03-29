using System;
using UnityEngine;

public class TorusCloneMovement : MonoBehaviour
{
    [SerializeField] private Transform torus;

    private void LateUpdate() => transform.position = Vector3.Lerp(transform.position, torus.position, Time.deltaTime * 10);
}
