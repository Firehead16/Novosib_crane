using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{
	/// <summary>
	/// UIButton
	/// </summary>
	[AddComponentMenu("UI/Extensions/UI Selectable Extension")]
	[RequireComponent(typeof(Selectable))]
	public class UISelectableExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		#region Sub-Classes
		[System.Serializable]
		public class UIButtonEvent : UnityEvent<PointerEventData.InputButton> { }
		#endregion

		#region Events
		[Tooltip("Event that fires when a button is initially pressed down")]
		public UIButtonEvent OnButtonPress;
		[Tooltip("Event that fires when a button is released")]
		public UIButtonEvent OnButtonRelease;
		[Tooltip("Event that continually fires while a button is held down")]
		public UIButtonEvent OnButtonHeld;
		#endregion
		
		private bool _pressed;
		private PointerEventData _heldEventData;

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			//Can't set the state as it's too locked down.
			//DoStateTransition(SelectionState.Pressed, false);

			if (OnButtonPress != null)
			{
				OnButtonPress.Invoke(eventData.button);
			}
			_pressed = true;
			_heldEventData = eventData;
		}


		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			//DoStateTransition(SelectionState.Normal, false);

			if (OnButtonRelease != null)
			{
				OnButtonRelease.Invoke(eventData.button);
			}
			_pressed = false;
			_heldEventData = null;
	   }
	   
		void Update()
		{
			if (!_pressed)
				return;
			
			if (OnButtonHeld != null)
			{
				OnButtonHeld.Invoke(_heldEventData.button);
			}
		}
		
		/// <summary>
		/// Test method to verify a control has been clicked
		/// </summary>
		public void TestClicked()
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("Control Clicked");
			#endif
		}
		
		/// <summary>
		/// Test method to verify a control is pressed
		/// </summary>
		public void TestPressed()
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("Control Pressed");
			#endif
		}
		
		/// <summary>
		/// est method to verify if a control is released
		/// </summary>
		public void TestReleased()
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("Control Released");
			#endif
		}
		
		/// <summary>
		/// est method to verify if a control is being held
		/// </summary>
		public void TestHold()
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("Control Held");
			#endif
		}

		//Fixed UISelectableExtension inactive bug (if gameObject becomes inactive while button is held down it never goes back to _pressed = false)
		void OnDisable()
		{
			_pressed = false;
		}
	}
}