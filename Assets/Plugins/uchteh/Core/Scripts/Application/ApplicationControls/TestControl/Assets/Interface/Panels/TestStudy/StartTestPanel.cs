using System;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class StartTestPanel : Panel, ITestPanel
	{
		public event Action OnStartButtonClick;

		[SerializeField]
		private Text startTitle = null;

		[SerializeField]
		private Text description = null;

		[SerializeField]
		private Text timeText = null;

		[SerializeField]
		private Button startButton = null;

		public override void Initialize()
		{
			base.Initialize();
			startButton.onClick.AddListener(() => OnStartButtonClick?.Invoke());
		}

		/// <summary>
		/// Показать меню
		/// </summary>
		/// <param name="test">Тест на прохождение</param>
		public void Show(Test test)
		{
			startTitle.text = test.Name;
			description.text = test.Description;
			timeText.text = "На прохождение теста дается " + TestControlsSettings.Default().Time + " мин.";

			base.Show();
		}
	}
}