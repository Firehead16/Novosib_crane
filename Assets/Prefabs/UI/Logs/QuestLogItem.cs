using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

/// <summary>
/// Элемент задания
/// </summary>
[Serializable]
public class QuestLogItem
{
	[ShowInInspector,ReadOnly]
	private string questName;
	public string QuestName => questName;

	[ShowInInspector,ReadOnly]
	public string SummaryTime;

	[ShowInInspector,ReadOnly]
	public int CurrentPoints
	{
		get
		{
			if (QuestPoints.Any())
			{
				int points = 0;
				foreach (var questPoint in QuestPoints)
				{
					points += questPoint.CurrentPointCount;
				}
				return points;
			}
			else
			{
				return 0;
			}
		}
	}

	[ShowInInspector,ReadOnly]
	public int SummaryPoints
	{
		get
		{
			if (QuestPoints.Any())
			{
				int points = 0;
				foreach (var questPoint in QuestPoints)
				{
					points += questPoint.MaxPointCount;
				}
				return points;
			}
			else
			{
				return 0;
			}
		}
	}

	[ShowInInspector, ReadOnly]
	public List<QuestLogPoint> QuestPoints = new List<QuestLogPoint>();

	[ShowInInspector, ReadOnly]
	public List<QuestLogError> QuestErrors = new List<QuestLogError>();

	/// <summary>
	/// Элемент задания
	/// </summary>
	/// <param name="name">Название задания</param>
	public QuestLogItem(string name)
	{
		questName = name;
	}
}