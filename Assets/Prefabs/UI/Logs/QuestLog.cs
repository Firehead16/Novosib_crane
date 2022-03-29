using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

/// <summary>
/// Информация о прохождении
/// </summary>
[Serializable]
public class QuestLog
{
	/// <summary>
	/// Сцены для заданий
	/// </summary>
	[ShowInInspector, ReadOnly]
	public Queue<string> SceneList = new Queue<string>();

	/// <summary>
	/// Элементы заданий
	/// </summary>
	[ShowInInspector, ReadOnly]
	public List<QuestLogItem> LogItems = new List<QuestLogItem>();

	/// <summary>
	/// Общее время всех заданий
	/// </summary>
	[ShowInInspector, ReadOnly]
	public float SummaryTime;

	/// <summary>
	/// Нужны ли подсказки для экзамена
	/// </summary>
	public bool IsNeedAdvice;

	/// <summary>
	/// Текущий режим работы заданий
	/// </summary>
    public QuestMode questMode;

	public void Clear()
	{
		IsNeedAdvice = false;
		SummaryTime = 0;

		SceneList.Clear();
		LogItems.Clear();
	}
}