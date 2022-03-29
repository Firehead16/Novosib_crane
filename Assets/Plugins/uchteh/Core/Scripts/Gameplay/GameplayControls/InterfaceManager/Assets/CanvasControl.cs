using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Ui
{
	public abstract class CanvasControl<TDefault> : CanvasControl
		where TDefault : class, IPanel
	{
		public virtual TDefault GetDefaultPanel()
		{
			return Controls.Values.FirstOrDefault() as TDefault;
		}
	}

	/// <summary>
	/// Менеджер, который загружает панели и применяет к ним тему
	/// </summary>
	public abstract class CanvasControl : InitableManager<IPanel>, ICanvasControl, IMessageObserver,
		IHideAndShowBehavior
	{
		[SerializeField] private bool isNeedDisable = false;

		public override void Load()
		{
			base.Load();

			if (isNeedDisable)
			{
				Hide();
			}
		}

		protected void DisableControls()
		{
			foreach (var panel in Controls)
			{
				panel.Value.GameObject.GetComponent<IHideAndShowBehavior>().Hide();
			}
		}

		[Button]
		public void Show(bool isTimeShow = false)
		{
			gameObject.SetActive(true);
		}

		[Button]
		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}