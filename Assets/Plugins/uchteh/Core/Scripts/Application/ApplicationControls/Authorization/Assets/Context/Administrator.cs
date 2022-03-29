using System;
using UnityEngine;

/// <summary>
/// Администратор
/// </summary>
[Serializable]
public class Administrator
{
	[SerializeField] 
	private int adminId;
	public int AdminId { get => adminId; set => adminId = value; }


	[SerializeField] 
	private Person person;
	public Person Person { get => person; set => person = value; }


	[SerializeField] 
	private int personId;
	public int PersonId { get => personId; set => personId = value; }

}