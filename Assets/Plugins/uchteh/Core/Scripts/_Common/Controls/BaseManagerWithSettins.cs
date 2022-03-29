using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

public interface IManager
{
	/// <summary>
	/// Показывает, корневой ли это менеджер. Если да, то должен проинициализироваться сам. Если нет, то должен подождать родительского.
	/// </summary>
	bool IsRoot { get; set; }
}
/// <summary>
/// Класс менеджера с настройками. Входящие в него менеджеры будут проинициализированы только если они сами являются InitableManager. Сам проинициализирован не будет
/// </summary>
/// <typeparam name="TCtrl">Объекты, которые данный менеджер контролирует. </typeparam> 
/// <typeparam name="TCtrlsSettings">Файл, в котором хранится список контролов</typeparam>
public abstract class BaseManagerWithSettins<TCtrl, TCtrlsSettings> : Manager<TCtrl>, IManager
	where TCtrl : class, IControl, ICanInstanse
	where TCtrlsSettings : SubSettings<TCtrlsSettings>, IControlsList<TCtrl>
{

	[SerializeField, BoxGroup("Параметры инициализации")] private bool isRoot;
	public bool IsRoot { get => isRoot; set => isRoot = value; }


	protected TCtrlsSettings Settings;

	public override void Load()
	{
	//	base.Load();

		Controls = LoadControlsFromSettings(out Settings, GetChildControls());
		OnControlsLoaded();
		RemoveUnusedControls();
	}

	public override void Unload()
	{
		base.Unload();

		foreach (var control in Controls.Values)
		{
			control.Unload();
			Destroy(control.GameObject);
		}
		Controls.Clear();
	}


	/// <summary>
	/// Загружает контролы из списка настроек на сцену, отфильтровывая те что не указаны в списке настроек
	/// </summary>
	/// <param name="controlsFromSettings"></param>
	/// <param name="controlsAlreadyExist"></param>
	/// <returns></returns>
	public Dictionary<string, TCtrl> LoadControlsFromSettings(out TCtrlsSettings controlsFromSettings, IReadOnlyCollection<TCtrl> controlsAlreadyExist)
	{
		controlsFromSettings = SubSettings<TCtrlsSettings>.Default();
		return FilterControls(controlsFromSettings, controlsAlreadyExist);

	}

	protected virtual IEnumerable<TCtrl> GetControls()
	{
		return GetComponentsInChildren<TCtrl>();
	}

	/// <summary>
	/// Добавляет недостающие и удаляет лишние контролы 
	/// </summary>
	/// <param name="controlsFromSettings">файл настроек с контролами</param>
	/// <param name="controlsAlreadyExist">контролы на сцене</param>
	/// <returns></returns>
	private Dictionary<string, TCtrl> FilterControls(TCtrlsSettings controlsFromSettings, IEnumerable<TCtrl> controlsAlreadyExist)
	{
		Dictionary<string, TCtrl> result = new Dictionary<string, TCtrl>();
		foreach (TCtrl requiredControl in controlsFromSettings.Controls)
		{
			// ReSharper disable once PossibleMultipleEnumeration
			if (requiredControl == null)
			{
				Debug.LogError("В файле настроек " + controlsFromSettings.GetType() + ": " + controlsFromSettings.name + " Отсутствует ссылка на один из объектов");
			}
			var existedControl = controlsAlreadyExist.FirstOrDefault(x => x.GetType() == requiredControl.GetType());


			if (existedControl != null)
			{
				result.Add(
					requiredControl.GetType().ToString(),
					LoadControl(existedControl, false) //загрузка контрола, уже существующего на сцене
				);
			}
			else
			{
				result.Add
				(
					requiredControl.GetType().ToString(),
					LoadControl(Instantiate(requiredControl.GameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<TCtrl>(),
						true)//создание и загрузка контрола из файла настроек
				);
			}
		}
		return result;
	}


	/// <summary>
	/// Инициализация переданного контрола
	/// </summary>
	private static TCtrl LoadControl(TCtrl control, bool isNewInstanse)
	{
		control.Load();
		control.IsNewInstanse = isNewInstanse;
		control.IsLoaded = true;
		return control;
	}

	/// <summary>
	/// Удаление дубликатов или контролов, неуказанных в настройках
	/// </summary>
	public void RemoveUnusedControls()
	{
		foreach (var controlInScene in GetComponentsInChildren<TCtrl>())
		{
			if (!controlInScene.IsLoaded)
				controlInScene.Unload();
		}
	}

}
