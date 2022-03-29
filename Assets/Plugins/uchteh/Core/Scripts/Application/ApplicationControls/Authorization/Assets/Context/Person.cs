using System;
using UnityEngine;

/// <summary>
/// Человек
/// </summary>
[Serializable]
public class Person
{
	[SerializeField] 
	private int personId;
	public int PersonId { get => personId; set => personId = value; }


	[SerializeField] 
	private string login;
	public string Login { get => login; set => login = value; }


	[SerializeField] 
	private string password;
	public string Password { get => password; set => password = value; }


	[SerializeField] 
	private string name;
	public string Name { get => name; set => name = value; }


	[SerializeField] 
	private string patronymic;
	public string Patronymic { get => patronymic; set => patronymic = value; }


	[SerializeField] 
	private string surname;
	public string Surname { get => surname; set => surname = value; }


	[SerializeField] 
	private bool isActive;
	public bool IsActive { get => isActive; set => isActive = value; }


	[SerializeField]
	private Role role;
	public Role Role { get => role; set => role = value; }
}