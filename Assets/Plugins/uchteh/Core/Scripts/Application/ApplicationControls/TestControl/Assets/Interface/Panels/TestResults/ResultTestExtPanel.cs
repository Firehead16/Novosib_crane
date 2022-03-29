using System;
using System.Collections.Generic;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class ResultTestExtPanel : Panel, ITestPanel
	{
		public event Action OnResultTextExtButtonClick;

		[SerializeField]
		private Text infoText = null;

		[SerializeField] 
		private Table table = null;

		[SerializeField]
		private Button closeButton = null;


		private PassedTest curPassedTest;

		public override void Initialize()
		{
			base.Initialize();

			closeButton.onClick.AddListener(Hide);
		}

		public void Show(PassedTest test)
		{
			curPassedTest = test;

			FillTestParams();
			base.Show();
		}

		public override void Hide()
		{
			OnResultTextExtButtonClick?.Invoke();
			base.Hide();
		}

		/// <summary>
		/// Заполнить параметры теста
		/// </summary>
		private void FillTestParams()
		{
			table.Clear();
			
			infoText.text = curPassedTest.Name;

			foreach (var passedQuestion in curPassedTest.PassedQuestions)
			{
				TableLine line = new TableLine(table, passedQuestion, new List<Button>()
				{
					UiBuilder.CreateButton(table.transform.GetChild(0), Vector3.one, passedQuestion.QuestionId.ToString()),
					UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, passedQuestion.Name),
					UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, passedQuestion.Answer),
					UiBuilder.CreateButton(table.transform.GetChild(3), Vector3.one, passedQuestion.IsRight? "Да": "Нет"),
				});
				table.AddLine(line);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}
	}
}