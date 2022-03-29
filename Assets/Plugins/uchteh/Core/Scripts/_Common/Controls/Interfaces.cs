using System;
using System.Collections.Generic;
using Core.Gameplay.Modul;
using Core.Global.Network;
using Core.Settings;
using Core.Ui;
using UnityEngine;


#region Базовые интерфейсы

public interface IMessageObserver
{
	/// <summary>
	/// Отправить уведомление
	/// </summary>
	/// <param name="typeControlMessageFor"> Используйте null  чтобы оповестить всех </param> 
	/// <param name="message">Сообщение</param>
	void Send(Type typeControlMessageFor, Message message);

	/// <summary>
	/// Получить уведомление
	/// </summary>
	/// <param name="message"></param>
	void Notify(Message message);
}

public interface ISubscribe
{
	/// <summary>
	/// Подписаться 
	/// </summary>
	void Subscribe();

	/// <summary>
	/// Отписаться
	/// </summary>
	void UnSubscribe();
}

public interface IHideAndShowBehavior
{
	/// <summary>
	/// Отобразить
	/// </summary>
	void Show(bool isTimeShow = false);

	/// <summary>
	/// Скрыть
	/// </summary>
	void Hide();
}

public interface ILoadable
{
	void Load();
}


#endregion

#region Базовые интерфейсы контролов

/// <summary>
/// Интерфейс объектов, которые будут загружены при запуске приложения/сцены
/// </summary>
public interface IControl
{
	bool IsNewInstanse { get; set; }
	bool IsLoaded { get; set; }
	void Load();
	void Unload();
}

/// <summary>
/// Интерфейс объектов, которые будут проинициализированы после загрузки
/// </summary>
public interface IInitialize : IControl
{
	bool IsInitialized { get; set; }
	void Initialize();
}

public interface IControlsList<T> where T : IControl
{
	List<T> Controls { get; }
}

public interface IInitializebleList<T> : IControlsList<T>
	where T : IControl, IInitialize
{

}

#endregion


public interface ICanInstanse
{
	GameObject GameObject { get; }
}


public interface IApplication : IMessageObserver
{

}


public interface IGameplayRestartControl
{

}

public interface IApplicationRestartControl
{ }



#region Интерфейсы контролов приложения
/// <summary>
/// Интерфейс объектов для работы приложения
/// </summary>
public interface IApplicationControl : IControl, ICanInstanse, IMessageObserver
{


}

public interface IDialog : IApplicationControl,
	IApplicationRestartControl
{

}

public interface ILisense : IInitialize,
	IApplicationControl
{

}


public interface INetwork : IInitialize,
	IApplicationControl,
	IApplicationRestartControl
{
	ConnectionType NetworkConfig { get; }
	Computer Computer { get; }
}

public interface ILoading : IApplicationControl,
	IApplicationRestartControl
{

}



#endregion

#region Интерфейсы контролов геймплея

/// <summary>
/// Интерфейс объектов, которые влияют на геймплей
/// </summary>
public interface IGameplayControl : IInitialize, ICanInstanse, IMessageObserver
{
}


#region Интерфейсы контроллеров модулей

public interface IModulControl : IGameplayControl, IGameplayRestartControl
{
    T[] GetControls<T>();
}


#endregion

#region Интерфейсы контроллеров канвасов

public interface IGameplayInterface : IGameplayControl,
	IGameplayRestartControl,
	IChangeStatusHandler
{

}


public interface IGameplayPanel : IPanel
{

}

public interface IInterfaceControl : ICanInstanse, IMessageObserver, IInitialize
{

}

public interface ICanvasControl : IControl
{

}

public interface IGameplayCanvasControl : ICanvasControl, IGameplayControl, IHasSaddle
{

}

public interface IDesktopCanvasControl : IGameplayCanvasControl
{

}

public interface IVrCanvasControl : IGameplayCanvasControl,IHideAndShowBehavior
{
    bool IsShowed { get;  }
    void ShowCanvas(bool needShow);
}


#endregion


public interface ITwoPointsEffect
{
    void SetActive(bool active);
    void SetPoints(Transform start, Transform target);
}

public interface IVrBrigde : IGameplayControl, IGameplayRestartControl
{
    Renderer LeftHandRenderer { get; }
    Renderer RightHandRenderer { get; }
    ITwoPointsEffect GrabEffect { get; }

}

#endregion





