using System;
using UnityEngine;

/// <summary>
/// Студент
/// </summary>
[Serializable]
public class Student
{
	[SerializeField] 
	private int studentId;
	public int StudentId { get => studentId; set => studentId = value; }


	[SerializeField] 
	private Person person;
	public Person Person { get => person; set => person = value; }


	[SerializeField] 
	private int personId;
	public int PersonId { get => personId; set => personId = value; }
}