using System;
using System.Collections.Generic;
using System.Linq;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class ResultTestPanel : Panel, ITestPanel
	{
		public event Action<PassedTest> OnLogExtButtonClick;
		public event Action<PassedTest> OnPrintButtonClick;
		public event Action OnFinishTestResultButtonClick;

		[SerializeField]
		private Table table = null;

		[SerializeField] 
		private Dropdown userDropdown = null;
		
		[SerializeField]
		private Button showExtButton = null;

		[SerializeField]
		private Button printButton = null;

		[SerializeField]
		private Button closeButton = null;

		private List<PassedTest> passedTests;
		private List<Person> users = new List<Person>();

		private Person currentUser;
		private int currentUserIndex;

		public override void Initialize()
		{
			base.Initialize();

			userDropdown.onValueChanged.AddListener(ChangeUser);
			showExtButton.onClick.AddListener(ShowExt);
			printButton.onClick.AddListener(Print);
			closeButton.onClick.AddListener(Hide);
		}

		public override void Show(bool isTimeShow = false)
		{
			FillUser();
			FillPassedTest();
			base.Show(isTimeShow);
		}

		public override void Hide()
		{
			OnFinishTestResultButtonClick?.Invoke();
			base.Hide();
		}

		/// <summary>
		/// Заполнить таблицу с пользователями
		/// </summary>
		private void FillUser()
		{
			users = TestControl.GetPersons().Where(person => person.Role == Role.Student).ToList();
			userDropdown.ClearOptions();

			if (users.Count != 0)
			{
				List<string> buttonNames = new List<string>();

				foreach (var user in users)
				{
					buttonNames.Add(user.Surname + " " + user.Name + " " + user.Patronymic);
				}

				userDropdown.AddOptions(buttonNames);
				ChangeUser(currentUserIndex < users.Count ? currentUserIndex : 0);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(userDropdown.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Заполнить таблицу со сложностями
		/// </summary>
		private void FillPassedTest()
		{
			table.Clear();

			passedTests = TestControl.GetPassedTests(currentUser.PersonId);

			if (passedTests.Count != 0)
			{
				foreach (var passedTest in passedTests)
				{
					TableLine line = new TableLine(table, passedTest, new List<Button>()
				{
					UiBuilder.CreateButton(table.transform.GetChild(0), Vector3.one, passedTest.LogId.ToString()),
					UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, passedTest.Date.ToString("dd.MM.yyyy")),
					UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, passedTest.Name),
					UiBuilder.CreateButton(table.transform.GetChild(3), Vector3.one, passedTest.PassedQuestions.Count.ToString()),
					UiBuilder.CreateButton(table.transform.GetChild(4), Vector3.one, passedTest.PassedQuestions.Count(p => p.IsRight).ToString()),
					UiBuilder.CreateButton(table.transform.GetChild(5), Vector3.one, passedTest.Grade.ToString()),
					UiBuilder.CreateButton(table.transform.GetChild(6), Vector3.one, passedTest.Complication.Name),
				});
					table.AddLine(line);
				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Сменить пользователя
		/// </summary>
		/// <param name="id"></param>
		private void ChangeUser(int id)
		{
			userDropdown.value = id;
			currentUser = users[id];
			currentUserIndex = id;
			FillPassedTest();
		}

		/// <summary>
		/// Отобразить дополнительную информацию
		/// </summary>
		private void ShowExt()
		{
			if (table.CurLine != null)
			{
				OnLogExtButtonClick?.Invoke(table.CurLine.TableObject);
				Hide();
			}
		}

		/// <summary>
		/// Печатать логи прохождения
		/// </summary>
		private void Print()
		{
			if (table.CurLine != null)
			{
				OnPrintButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}
	}

}