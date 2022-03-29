using System;
using UnityEngine;

/// <summary>
/// Группа
/// </summary>
[Serializable]
public class Group
{
	[SerializeField] 
	private int groupId;
	public int GroupId { get => groupId; set => groupId = value; }


	[SerializeField] 
	private string name;
	public string Name { get => name; set => name = value; }


	[SerializeField]
	private Speciality speciality;
	public Speciality Speciality { get => speciality; set => speciality = value; }


	[SerializeField] 
	private int specialityId;
	public int SpecialityId { get => specialityId; set => specialityId = value; }
}