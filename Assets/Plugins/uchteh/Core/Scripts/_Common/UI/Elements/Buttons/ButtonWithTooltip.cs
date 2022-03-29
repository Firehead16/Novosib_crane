using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Ui
{
	public class ButtonWithTooltip : Button 
	{
		public string TooltipText = "";

		private Tooltip toolTip;


		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (toolTip == null)
			{
				toolTip = UiBuilder.CreateTooltip(transform, Vector3.one, TooltipText);
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			CloseTooltip();
		}

		private void CloseTooltip()
		{
			if (toolTip != null)
			{
				Destroy(toolTip.gameObject);
			}
		}
	}
}