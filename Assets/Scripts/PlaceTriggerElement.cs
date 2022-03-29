using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Collections;
using UnityEngine;

public class PlaceTriggerElement : MonoBehaviour
{
	[ReadOnly]
	public List<GameObject> TriggerVisitors = new List<GameObject>();

	[ReadOnly]
	public bool IsTrigger;

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Cargo")
		{
			if (!TriggerVisitors.Contains(other.gameObject) && other.GetComponent<Rigidbody>().velocity.magnitude < 0.02f)
			{
				TriggerVisitors.Add(other.gameObject);
				IsTrigger = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Cargo")
		{
			TriggerVisitors.Remove(other.gameObject);

			if (!TriggerVisitors.Any())
			{
				IsTrigger = false;
			}
		}
	}
}