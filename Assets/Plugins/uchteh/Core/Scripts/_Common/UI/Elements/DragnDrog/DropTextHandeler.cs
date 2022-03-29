using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Ui
{
	public class DropTextHandeler : MonoBehaviour, IDropHandler
	{
		//класс для корректной работы пустых мест, отображаемых на экране

		public int Placeholdernumber;

		private int answerSetInPlaceholder = -1;


		public int GetAnswer()
		{
			return answerSetInPlaceholder;
		}

		public void SetAnswerinPlaceholder(int someNumber)
		{
			answerSetInPlaceholder = someNumber;
		}

		/// <summary>
		/// Проверка, заполнен ли пропуск
		/// </summary>
		/// <returns></returns>
		public bool IsPlaceholderFilled()
		{
		    if (answerSetInPlaceholder != -1) return true;
		    return false;
		}

		/// <summary>
		/// Проверка, находится ли на этом месте правильный ответ 
		/// </summary>
		/// <returns></returns>
		public bool CheckPlaceholder()
		{
			return Placeholdernumber == answerSetInPlaceholder;
		}

		/// <summary>
		/// Вызывается, когда объект опускается на DropTextHandeler (перед IEndDrag)
		/// </summary>
		/// <param name="eventSystem"></param>
		public void OnDrop(PointerEventData eventSystem)
		{
			if (eventSystem.pointerDrag.GetComponent<DragTextHandeler>())
			{
				eventSystem.pointerDrag.transform.position = transform.position;
				eventSystem.pointerDrag.GetComponent<DragTextHandeler>().SetParent(GetComponent<RectTransform>());
				DragTextHandeler.IsDropped = true;
				SetAnswerinPlaceholder(DragTextHandeler.DraggedItemId);
			}
		}
	} 
}
