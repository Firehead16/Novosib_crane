using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DemoPanel : MonoBehaviour
{
	public event Action OnEndedButtonClick;

	#region Интерфейс

	[SerializeField]
	private Text demoTextName = null;

	[SerializeField]
	private Dropdown dropDown = null;

	[SerializeField]
	private Text description = null;

	[SerializeField]
	private Image demoImage = null;

	[SerializeField]
	private Button prevDemoButton = null, nextDemoButton = null, prevStepButton = null, nextStepButton = null;

	[SerializeField]
	private List<Button> moveButtons = null;

	[SerializeField]
	private Button exitButton = null;

	#endregion

	#region Свойства

	/// <summary>
	/// Все полученные и выполняемые квесты
	/// </summary>
	[SerializeField]
	private List<Demo> demos = null;

	/// <summary>
	/// Текущая демонстрация
	/// </summary>
	[ShowInInspector, ReadOnly]
	private Demo demo;

	/// <summary>
	/// Индекс текущей демонстрации
	/// </summary>
	[ShowInInspector, ReadOnly]
	private int currentDemoIndex;

	private DemoStep CurrentStepIndex => demo.StepList[dropDown.value];
	
	#endregion

	private void Start()
	{
		exitButton.onClick.AddListener(FinishDemo);
		dropDown.onValueChanged.AddListener(ChangeDropDown);
		prevDemoButton.onClick.AddListener(MovePrevDemo);
		nextDemoButton.onClick.AddListener(MoveNextDemo);
		prevStepButton.onClick.AddListener(MovePrevStep);
		nextStepButton.onClick.AddListener(MoveNextStep);
	}

	/// <summary>
	/// Загрузить демонстрацию
	/// </summary>
	public void StartDemo(DemoType demoType)
	{
		gameObject.SetActive(true);
		demoImage.sprite = null;


		currentDemoIndex = demos.FindIndex(d => d.DemoType == demoType);
		ChangeDemo(currentDemoIndex);
	}

	/// <summary>
	/// Сменить демонстрацию
	/// </summary>
	/// <param name="index"></param>
	private void ChangeDemo(int index)
	{
		demo = demos[index];
		demoTextName.text = demo.Name;

		FillDropDown();
		SetStep();
	}

	/// <summary>
	/// Закончить демонстрацию
	/// </summary>
	private void FinishDemo()
	{
		dropDown.ClearOptions();
		demoTextName.text = "";
		description.text = "";
		demoImage.sprite = null;

		gameObject.SetActive(false);


		OnEndedButtonClick?.Invoke();
	}

	/// <summary>
	/// Заполнить выпадающий список
	/// </summary>
	private void FillDropDown()
	{
		List<string> dropdownOptions = new List<string>();

		for (var index = 0; index < demo.StepList.Count; index++)
		{
			string count = (index + 1) + " из " + demo.StepList.Count;
			dropdownOptions.Add(count);
		}
		dropDown.ClearOptions();
		dropDown.AddOptions(dropdownOptions);
	}

	/// <summary>
	/// Изменить описание демонстрации
	/// </summary>
	private void SetStep()
	{
		var currentStepInDemoPart = CurrentStepIndex;
		description.text = "\n <color=yellow>" + currentStepInDemoPart.Name + "</color>" + "\n \n";

		if (!string.IsNullOrEmpty(currentStepInDemoPart.Description))
		{
			description.text += currentStepInDemoPart.Description + "\n";
		}

		demoImage.sprite = CurrentStepIndex.Image != null ? CurrentStepIndex.Image : null;
	}

	#region Движение по элементам

	/// <summary>
	/// Изменить значения в описании
	/// </summary>
	private void ChangeDropDown(int value)
	{
		SetStep();
	}

	/// <summary>
	/// Выбрать следующий элемент в списке
	/// </summary>
	private void MoveNextStep()
	{
		if (dropDown.options.Count  != dropDown.value + 1)
		{
			dropDown.value += 1;
		}
	}

	/// <summary>
	/// Выбрать предыдущий элемент в списке
	/// </summary>
	private void MovePrevStep()
	{
		if (dropDown.value != 0)
		{
			dropDown.value -= 1;
		}

	}

	/// <summary>
	/// Перейти на предыдущую часть демонстрации
	/// </summary>
	private void MovePrevDemo()
	{
		if (currentDemoIndex - 1 >= 0)
		{
			currentDemoIndex--;
			ChangeDemo(currentDemoIndex);
		}
	}

	/// <summary>
	/// Перейти на следующую часть демонстрации
	/// </summary>
	private void MoveNextDemo()
	{
		if (currentDemoIndex + 1 < demos.Count)
		{
			currentDemoIndex++;
			ChangeDemo(currentDemoIndex);
		}
	}


	#endregion
	
	#region Состояния кнопок
	
	private void SetInteractMoveButtons(bool isShow)
	{
		foreach (var moveButton in moveButtons)
		{
			moveButton.interactable = isShow;
			Color current = moveButton.transform.GetChild(1).GetComponent<Image>().color;

			if (isShow)
			{
				moveButton.transform.GetChild(1).GetComponent<Image>().color = new Color(current.r, current.g, current.b, 1);
			}
			else
			{
				moveButton.transform.GetChild(1).GetComponent<Image>().color = new Color(current.r, current.g, current.b, 0.4f);
			}
		}

		dropDown.interactable = isShow;
	}

	private void SetInteractMoveButton(int number, bool isShow)
	{

		var moveButton = moveButtons[number];
		moveButton.interactable = isShow;

		Color current = moveButton.transform.GetChild(1).GetComponent<Image>().color;

		if (isShow)
		{
			moveButton.transform.GetChild(1).GetComponent<Image>().color = new Color(current.r, current.g, current.b, 1);
		}
		else
		{
			moveButton.transform.GetChild(1).GetComponent<Image>().color = new Color(current.r, current.g, current.b, 0.4f);
		}

	}

	#endregion
}
