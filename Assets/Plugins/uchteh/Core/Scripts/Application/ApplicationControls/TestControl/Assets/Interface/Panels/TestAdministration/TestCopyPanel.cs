using System;
using System.Collections.Generic;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class TestCopyPanel : Panel, ITestPanel
	{
		public event Action<Test, int> OnCopyTestButtonClick;

		[SerializeField]
		private Dropdown complicationDropdown = null;

		[SerializeField]
		private Button copyButton = null;

		[SerializeField]
		private Button closeButton = null;

		private Test currentTest;
		private List<Complication> complicationsLeft;
		
		public override void Initialize()
		{
			base.Initialize();

			copyButton.onClick.AddListener(Copy);
			closeButton.onClick.AddListener(Hide);
		}

		public void Show(Test test, List<Complication> complications)
		{
			currentTest = test;

			// Удаляем существующию сложность
			complicationsLeft = new List<Complication>();
			if (complications != null)
			{
				foreach (var complication in complications)
				{
					if (complication != currentTest.Complication)
						complicationsLeft.Add(complication);
				}
			}
			
			// Заполнить список доступных сложностей
			complicationDropdown.ClearOptions();
			if (complicationsLeft.Count != 0)
			{
				List<string> buttonNames = new List<string>();

				foreach (var complication in complicationsLeft)
				{
					buttonNames.Add(complication.Name);
				}
				complicationDropdown.AddOptions(buttonNames);
			}

			base.Show();
		}

		/// <summary>
		/// Копирование теста
		/// </summary>
		private void Copy()
		{
			if (currentTest == null)
			{
				Debug.LogError("Ошибка! Тест не найден!!!");
				return;
			}

			OnCopyTestButtonClick?.Invoke(currentTest, complicationsLeft[complicationDropdown.value].ComplicationId);
			Hide();
		}
	} 
}
