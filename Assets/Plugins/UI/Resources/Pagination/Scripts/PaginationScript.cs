using Core.Ui.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Ui.Examples
{
    public class PaginationScript : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
	    private HorizontalScrollSnap hss = null;
        
	    public int Page;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (hss != null)
            {
                hss.GoToScreen(Page);
            }
        }
    }
}