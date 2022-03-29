using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class QuestStorage : MonoBehaviour
{
	public static QuestStorage Instance;

	public QuestCargoPlace QuestCargoPlace;

	public LiftingDevice LiftingDevice;

	public CargoType CargoType;

	public Quest QuestType;

	public QuestMode questMode;

	public int Timer;

	public float fog, wind, waves;

	public bool RecordThis;
	
	[ShowInInspector, ReadOnly]
	public List<string> Mistakes = new List<string>();

	[HideInInspector]
	public int MainScene = 0;
	
	[HideInInspector]
	public int QuestScene = 1;
	
	[HideInInspector]
	public int ResultScene = 2;

	[ShowInInspector, ReadOnly]
	public QuestLog QuestLog = new QuestLog();

	public string UserName;

	private void Awake()
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(this);
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public void AddMistake(string mistake){
		Mistakes.Add(mistake + " " + QuestControl.GetTime());
	}
}



public enum QuestMode
{
	Learn,
	Exam
}

public enum Quest
{
	Pipes,				// Перемещение труб
	Containers,			// Перемещение контейнеров
	Auto,				// Перемещение автотранспорта
	Bulk				// Перемещение сыпучих грузов
}

public enum QuestCargoPlace
{
	Auto,				// Автотранспорт
	RailwayPlatform,    // Ж/Д платформа / открытый вагон
	Pontoon,			// Плашкоут (выделенное место на палубе)
	AnotherContainer,	// Другой контейнер
	SecondVessel,		// Второе судно
	HoldToPontoon,		// Из трюма на плашкоут
	PontoonToHold		// С плашкоута в трюм
}

public enum LiftingDevice
{
	Greifer,            // Грейфер
	Traverse,           // Траверса
	AutoTraverse,		// Автотраверса
	Spreader,            // Спредер
	TextileSlings,      // Текстильные стропы
	LineSlings,			// Линейные стропы
	RopeSlings,         // Канатные стропы
}

public enum CargoType
{
	GenCargo,			// Ген. грузы
	Container20Ft,		// 20 футовый контейнер
	Container40Ft		// 40 футовый контейнер
}