using System;
using Core.Ui;
using UnityEngine;

namespace Core.Global.Administration
{
	public class ButtonPanel : Panel, IAdminPanel
	{
		public event Action OnAboutButtonClick;
		public event Action OnHelpButtonClick;
		public event Action OnDocumentaryButtonClick;
		public event Action OnInputButtonClick;
		public event Action OnQuitButtonClick;
		public event Action OnSettingsButtonClick;
		public event Action OnTestingButtonClick;

		[SerializeField]
		private ButtonWithTooltip aboutButton = null;

		[SerializeField]
		private ButtonWithTooltip helpButton = null;

		[SerializeField]
		private ButtonWithTooltip documentaryButton = null;

		[SerializeField]
		private ButtonWithTooltip inputButton = null;
		
		[SerializeField]
		private ButtonWithTooltip settingsButton = null;

		[SerializeField]
		private ButtonWithTooltip quitButton = null;

		[SerializeField]
		private ButtonWithTooltip testingButton = null;


		public override void Initialize()
		{
			base.Initialize();

			aboutButton.onClick.AddListener(() => OnAboutButtonClick?.Invoke());
			helpButton.onClick.AddListener(() => OnHelpButtonClick?.Invoke());
			documentaryButton.onClick.AddListener(() => OnDocumentaryButtonClick?.Invoke());
			inputButton.onClick.AddListener(() => OnInputButtonClick?.Invoke());
			settingsButton.onClick.AddListener(() => OnSettingsButtonClick?.Invoke());
			testingButton.onClick.AddListener(() => OnTestingButtonClick?.Invoke());
			quitButton.onClick.AddListener(() => OnQuitButtonClick?.Invoke());
		}
	}
}