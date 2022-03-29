using System;
using System.Collections;
using Core.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Ui
{
	public class DragTextHandeler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		public event Action OnDragEvent;
		public event Action OnDropEvent;

		//класс для корректной работы перетаскиваемых ответов, отображаемых на экране.
		public static int DraggedItemId;

		public static bool IsDropped;
		public static bool FirstDrag = true;
	
		public int Number;
		private Vector3 startPosition;
		private Vector3 offset;
		private float duration = 1f;
		private Coroutine coroutine;

		private void Start()
		{
			GetComponent<RectTransform>().SetDefaultScale();
		}

		public int GetAnswerNumber()
		{
			return Number;
		}

		public void OnBeginDrag(PointerEventData eventSystem)
		{
			if (FirstDrag)
			{
				startPosition = GetComponent<RectTransform>().position;
			}
			IsDropped = false;
			if (coroutine != null) StopCoroutine(coroutine);
			GetComponent<CanvasGroup>().blocksRaycasts = false;
			DraggedItemId = GetAnswerNumber();
			offset = transform.position - Input.mousePosition;
		}

		public void OnDrag(PointerEventData eventSystem)
		{
			transform.position = Input.mousePosition + offset;
			OnDragEvent?.Invoke();
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			if (!IsDropped)
			{
				if (coroutine != null) StopCoroutine(coroutine);
				coroutine = StartCoroutine(MoveLerp(startPosition));
			}

			OnDropEvent?.Invoke();
		}

		public void SetParent(RectTransform newParent)
		{
			GetComponent<RectTransform>().SetParent(newParent);
		}

		private IEnumerator MoveLerp(Vector3 newPosition)
		{
			var t = 0f;
			while (t < duration)
			{
				if (t > duration) t = duration;
				else t += Time.deltaTime;
				GetComponent<RectTransform>().position =
					Vector3.Lerp(GetComponent<RectTransform>().position, newPosition, t / duration);
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}
	} 
}