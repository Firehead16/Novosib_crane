using System;
using UnityEngine;

/// <summary>
/// Специальность
/// </summary>
[Serializable]
public class Speciality
{
	[SerializeField] 
	private int specialityId;
	public int SpecialityId { get => specialityId; set => specialityId = value; }


	[SerializeField]
	private string name;
	public string Name { get => name; set => name = value; }
}