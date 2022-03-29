using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{

	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Tab Navigation Helper")]
	public class TabNavigationHelper : MonoBehaviour
	{
		private EventSystem system;
		private Selectable lastObject;

		private InputActionAsset actionAsset;

		private InputAction tabClick;
		private InputAction shiftClick;

		private void Start()
		{
			system = GetComponent<EventSystem>();
			if (system == null)
			{
				Debug.LogError("Нужно добавить на Event System на сцене");
			}
			else
			{
				actionAsset = system.GetComponent<InputSystemUIInputModule>().actionsAsset;

				if (actionAsset != null)
				{
					tabClick = actionAsset.FindAction("MainNavigation");
					tabClick.Enable();

					shiftClick = actionAsset.FindAction("SecondNavigation");
					shiftClick.Enable();
				}
				else
				{
					Debug.LogError("Не найден InputActionAsset");
				}
			}
		}

		public void Update()
		{
			Selectable next = null;

			if (lastObject == null && system.currentSelectedGameObject != null)
			{
				lastObject = system.currentSelectedGameObject.GetComponent<Selectable>();
			}

			if (tabClick.triggered && shiftClick.ReadValue<float>() > 0)
			{
				if (system.currentSelectedGameObject != null)
				{
					next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
				}
				else
				{
					SelectDefaultObject(out next);
				}

			}
			else if (tabClick.triggered)
			{
				if (system.currentSelectedGameObject != null)
				{
					next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
				}
				else
				{
					SelectDefaultObject(out next);
				}
			}
			else if (system.currentSelectedGameObject == null)
			{
				SelectDefaultObject(out next);
			}

			SelectGameObject(next);
		}

		private void SelectDefaultObject(out Selectable next)
		{
			next = system.firstSelectedGameObject ? system.firstSelectedGameObject.GetComponent<Selectable>() : null;
		}

		private void SelectGameObject(Selectable selectable)
		{
			if (selectable != null)
			{
				InputField inputfield = selectable.GetComponent<InputField>();
				if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

				system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(system));
			}
		}
	}
}