using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Global.Administration
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class AboutPanel : Panel, IAdminPanel
	{
		[SerializeField]
		private Button closeButton = null;

		public override void Initialize()
		{
			base.Initialize();

			closeButton.onClick.AddListener(Hide);
		}
	}
}