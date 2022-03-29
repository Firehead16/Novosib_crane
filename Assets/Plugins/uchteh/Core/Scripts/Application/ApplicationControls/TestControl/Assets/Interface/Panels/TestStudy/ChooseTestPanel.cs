using System;
using System.Collections.Generic;
using Core.Extensions;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class ChooseTestPanel : Panel, ITestPanel
	{
		public event Action<bool, Test> OnChooseTestingButtonClick;
		public event Action OnExitClick;

		[SerializeField]
		private Table table = null;

		[SerializeField]
		private Button studyButton = null;

		[SerializeField]
		private Button testButton = null;

		[SerializeField]
		private Button exitButton = null;

		private List<Test> tests;

		public override void Initialize()
		{
			base.Initialize();

			studyButton.onClick.AddListener(() =>
			{
				if (table.CurLine != null)
				{
					OnChooseTestingButtonClick?.Invoke(true, table.CurLine.TableObject);
				}
			});
			testButton.onClick.AddListener(() =>
			{
				if (table.CurLine != null)
				{
					OnChooseTestingButtonClick?.Invoke(true, table.CurLine.TableObject);
				}
			});
			exitButton.onClick.AddListener(() => { OnExitClick?.Invoke(); });
		}

		public void Show(List<Test> availableTests)
		{
			base.Show();

			tests = availableTests;
			FillTable();
		}

		/// <summary>
		/// Заполнить таблицу с тестами
		/// </summary>
		private void FillTable()
		{
			table.Clear();

			if (tests.Count != 0)
			{
				int count = 0;

				foreach (var test in tests)
				{
					var buttonCoount = UiBuilder.CreateButton(table.transform.GetChild(0).transform, Vector3.one, count.ToString());
					buttonCoount.GetComponentInChildren<RectTransform>().SetHeight(100);
					buttonCoount.transform.localPosition = Vector3.ProjectOnPlane(buttonCoount.transform.localPosition, Vector3.forward);
					buttonCoount.transform.localRotation = Quaternion.identity;

					var buttonName = UiBuilder.CreateButton(table.transform.GetChild(1).transform, Vector3.one, test.Name);
					buttonName.GetComponentInChildren<RectTransform>().SetHeight(100);
					buttonName.transform.localPosition = Vector3.ProjectOnPlane(buttonName.transform.localPosition, Vector3.forward);
					buttonName.transform.localRotation = Quaternion.identity;


					var buttonDesc = UiBuilder.CreateButton(table.transform.GetChild(2).transform, Vector3.one, test.Description);
					buttonDesc.GetComponentInChildren<RectTransform>().SetHeight(100);
					buttonDesc.transform.localPosition = Vector3.ProjectOnPlane(buttonDesc.transform.localPosition, Vector3.forward);
					buttonDesc.transform.localRotation = Quaternion.identity;


					var buttonQuestionCount = UiBuilder.CreateButton(table.transform.GetChild(3).transform, Vector3.one, test.Questions.Count.ToString());
					buttonQuestionCount.GetComponentInChildren<RectTransform>().SetHeight(100);
					buttonQuestionCount.transform.localPosition = Vector3.ProjectOnPlane(buttonQuestionCount.transform.localPosition, Vector3.forward);
					buttonQuestionCount.transform.localRotation = Quaternion.identity;


					TableLine tableLine = new TableLine(table, test, new List<Button>()
					{
						buttonCoount,
						buttonName,
						buttonDesc,
						buttonQuestionCount
					});
					table.AddLine(tableLine);
					count++;
				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}
	}
}
