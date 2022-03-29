using System;
using Core.Extensions;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Нужен, чтобы при реализации IControl, IInitable и ICanInstance не требовалось реализовывать их методы каждый раз. (+ позволяет упростить дебаг) 
/// </summary>
public abstract class BaseControlMethods : SerializedMonoBehaviour
{
	public GameObject GameObject => gameObject;

	[SerializeField, ReadOnly, BoxGroup("Параметры инициализации")] 
	private bool isNewInstanse;

	public bool IsNewInstanse
	{
		get => isNewInstanse;
		set => isNewInstanse = value;
	}

	[SerializeField, ReadOnly, BoxGroup("Параметры инициализации")] 
	private bool isLoaded;

	public bool IsLoaded
	{
		get => isLoaded;
		set => isLoaded = value;
	}


	public virtual void Load()
	{
		this.DebugLoad();
	}


	public virtual void Initialize()
	{
		this.DebugInitialize();
	}

	
	public virtual void Unload()
	{
		this.DebugUnload();
	}

	public virtual void Send(Type typeControlMessageFor, Message message)
	{
		ControlExtention.GameplaySend(typeControlMessageFor, message);
		ControlExtention.ApplicationSend(typeControlMessageFor, message);
	}


	public virtual void Notify(Message message)
	{
	}

}