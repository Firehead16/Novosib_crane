using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChooseParameters : MonoBehaviour
{
    [SerializeField] public List<Transform> Cargo = new List<Transform>();
    [SerializeField] public List<Transform> Cargo20 = new List<Transform>();
    [SerializeField] public List<Transform> PlaceTriggers = new List<Transform>();
    [SerializeField] public List<Transform> PlaceTriggers20 = new List<Transform>();
    [SerializeField] public List<Transform> GenCargo = new List<Transform>();
    [SerializeField] public List<Transform> GenPlaceTriggers = new List<Transform>();
    [SerializeField] public List<Transform> AdditionalObjects = new List<Transform>();
}
