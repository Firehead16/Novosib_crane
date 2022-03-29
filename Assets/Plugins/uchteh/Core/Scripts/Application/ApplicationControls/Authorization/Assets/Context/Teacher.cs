using System;
using UnityEngine;

/// <summary>
/// Преподаватель
/// </summary>
[Serializable]
public class Teacher
{
	[SerializeField] private int teacherId;
	[SerializeField] private Person person;
	[SerializeField] private int personId;

	public int TeacherId { get => teacherId; set => teacherId = value; }
	public Person Person { get => person; set => person = value; }
	public int PersonId { get => personId; set => personId = value; }
}