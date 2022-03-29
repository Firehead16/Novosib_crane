using System;
using Sirenix.OdinInspector;
using UnityEngine;


/// <summary>
/// Элемент с баллом
/// </summary>
[Serializable]
public class QuestLogPoint
{
	[ShowInInspector,ReadOnly]
	private string pointName;
	public string PointName => pointName;

	[ShowInInspector,ReadOnly]
	private int maxPointCount;
	public int MaxPointCount => maxPointCount;

	[SerializeField]
	private int currentPointCount;
	public int CurrentPointCount
	{
		get => currentPointCount;
		set => currentPointCount = value;
	}

	[ShowInInspector,ReadOnly]
	private bool isCritical;
	public bool IsCritical => isCritical;
}

