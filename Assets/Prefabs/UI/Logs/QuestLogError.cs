using System;
using Sirenix.OdinInspector;

/// <summary>
/// Ошибка
/// </summary>
[Serializable]
public class QuestLogError
{
	[ShowInInspector,ReadOnly]
	private string errorName;
	public string ErrorName => errorName;

	[ShowInInspector,ReadOnly]
	private string errorTime;
	public string ErrorTime => errorTime;

	/// <summary>
	/// Ошибка
	/// </summary>
	/// <param name="name">Название ошибки</param>
	/// <param name="time">Время происхождения ошибки</param>
	public QuestLogError(string name, string time)
	{
		errorName = name;
		errorTime = time;
	}
}