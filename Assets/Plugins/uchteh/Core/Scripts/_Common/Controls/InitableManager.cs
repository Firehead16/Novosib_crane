using System;
using System.Collections.Generic;
using Core.Settings;

public abstract class InitableManager<TInitable> : BaseManager<TInitable>, IManager
	where TInitable : class, IInitialize
{
	public bool IsRoot { get; set; }

	public override void Initialize()
	{
		base.Initialize();
		if (IsRoot) this.InitializeItems(Controls.Values);
		OnItemsInitialised();
	}

	protected abstract void OnItemsInitialised();
}


public static class ControlExtention
{
	/// <summary>
	/// Выполняет инициализацию всех элементов в переданном списке Controls
	/// </summary>
	/// <typeparam name="TInitable"></typeparam>
	/// <param name="_"></param>
	/// <param name="controls">Объекты, у которых будет подтверждена загрузка и выполнена инициализация</param>

	public static void InitializeItems<TInitable>(this IManager _, IReadOnlyCollection<TInitable> controls)
	where TInitable : class, IInitialize
	{
		{
			foreach (var initableItem in controls)
			{
				if (initableItem is IManager manager) manager.IsRoot = true;
				if (!initableItem.IsLoaded) initableItem.IsLoaded = true;
				if (!initableItem.IsInitialized) { initableItem.Initialize(); }
			}
		}
	}

	public static void Send(Type typeControlMessageFor, Message message)
	{
		GameplaySend(typeControlMessageFor, message);
		ApplicationSend(typeControlMessageFor, message);
	}

	public static void ApplicationSend(Type typeControlMessageFor, Message message)
	{
		GlobalLinkStorage.Application?.Send(typeControlMessageFor, message);
	}

	public static void GameplaySend(Type tyControlMessageFor, Message message)
	{
		GlobalLinkStorage.Gameplay?.Send(tyControlMessageFor, message);
	}
}