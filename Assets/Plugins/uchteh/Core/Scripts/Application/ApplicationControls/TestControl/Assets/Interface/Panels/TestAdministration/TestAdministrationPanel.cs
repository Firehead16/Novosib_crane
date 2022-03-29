using System;
using System.Collections.Generic;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class TestAdministrationPanel : Panel, ITestPanel
	{
		public event Action<Test, bool> OnChangeTestButtonClick;
		public event Action<Test> OnDeleteTestButtonClick;
		public event Action<Test> OnCopyTestButtonClick;

		[SerializeField]
		private Table table = null;

		[SerializeField]
		private Dropdown complicationDropdown = null;

		[SerializeField]
		private Button addButton = null, editButton = null, deleteButton = null, copyButton = null;

		[SerializeField]
		private Button closeButton = null;

		private List<Test> tests;
		private List<Complication> complications;
		private Complication curComplication;
		private int curComplicationNumber;

		public override void Initialize()
		{
			base.Initialize();

			complicationDropdown.onValueChanged.AddListener(ChangeComplication);

			addButton.onClick.AddListener(Add);
			editButton.onClick.AddListener(Edit);
			deleteButton.onClick.AddListener(Delete);
			copyButton.onClick.AddListener(Copy);

			closeButton.onClick.AddListener(Hide);
		}

		public override void Show(bool isTimeShow = false)
		{
			FillComplications();
			FillTests();
			base.Show(isTimeShow);
		}

		/// <summary>
		/// Заполнить сложности
		/// </summary>
		private void FillComplications()
		{
			curComplication = null;
			complications = TestControl.GetComplications();

			complicationDropdown.ClearOptions();

			if (complications.Count != 0)
			{
				List<string> buttonNames = new List<string>();

				foreach (var complication in complications)
				{
					buttonNames.Add(complication.Name);
				}

				complicationDropdown.AddOptions(buttonNames);
				ChangeComplication(curComplicationNumber);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(complicationDropdown.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Заполнить таблицу со сложностями
		/// </summary>
		private void FillTests()
		{
			table.Clear();

			if (curComplication != null)
			{
				tests = TestControl.GetTests(curComplication);

				if (tests.Count != 0)
				{
					foreach (var test in tests)
					{
						TableLine line = new TableLine(table, test, new List<Button>()
						{
							UiBuilder.CreateButton(table.transform, Vector3.one, test.Name)
						});
						table.AddLine(line);
					}
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Изменить сложность
		/// </summary>
		/// <param name="id"></param>
		private void ChangeComplication(int id)
		{
			complicationDropdown.value = id;
			curComplicationNumber = id;
			curComplication = complications[id];

			FillTests();
		}

		/// <summary>
		/// Добавить тест
		/// </summary>
		private void Add()
		{
			Test test = new Test()
			{
				Name = "",
				Description = "",
				Complication = curComplication,
				ComplicationId = curComplication.ComplicationId
			};
			OnChangeTestButtonClick?.Invoke(test, true);
			Hide();
		}

		/// <summary>
		/// Редактировать тест
		/// </summary>
		private void Edit()
		{
			if (table.CurLine != null)
			{
				OnChangeTestButtonClick?.Invoke(table.CurLine.TableObject, false);
				Hide();
			}
		}

		/// <summary>
		/// Удалить тест
		/// </summary>
		private void Delete()
		{
			if (table.CurLine != null)
			{
				OnDeleteTestButtonClick?.Invoke(table.CurLine.TableObject);
				FillTests();
			}
		}

		/// <summary>
		/// Копировать тест
		/// </summary>
		private void Copy()
		{
			if (table.CurLine != null)
			{
				OnCopyTestButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}
	}
}
