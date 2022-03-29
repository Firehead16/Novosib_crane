using System.Collections.Generic;

/// <summary>
///  Интерфейс менеджера для инициализвции объектов в коллекции
/// </summary>
/// <typeparam name="TCtrl"></typeparam>
public interface IAbstractManager<TCtrl>
	where TCtrl : class, IControl
{
	/// <summary>
	/// Коллекция объектов
	/// </summary>
	Dictionary<string, TCtrl> Controls { get; }

	/// <summary>
	/// Получить список объектов из коллекции
	/// </summary>
	/// <returns></returns>
	IReadOnlyCollection<TCtrl> GetChildControls();

	/// <summary>
	/// Загрузить список объектов в коллекцию
	/// </summary>
	/// <param name="controls"></param>
	void LoadControls(IReadOnlyCollection<TCtrl> controls);

	/// <summary>
	/// Добавление объекта в коллекцию
	/// </summary>
	/// <param name="loadedControl"></param>
	void AddControlOnLoad(TCtrl loadedControl);

	/// <summary>
	/// Получить ключ объекта
	/// </summary>
	/// <param name="loadedControl"></param>
	/// <returns></returns>
	string GetControlKey(TCtrl loadedControl);

	/// <summary>
	/// Получить объект по типу
	/// </summary>
	/// <returns></returns>
	T GetControl<T>() where T : class, TCtrl;

	void ControlsLoaded();
}