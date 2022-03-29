using Core.Ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

	public class ConfirmPanel : Panel
	{
		// Текст подтверждения
		public Text ActionText;

		public Text ConfirmButton;
		public Text DeclineButton;

		public UnityEvent ConfirmEvent;
		public UnityEvent DeclineEvent;

		[SerializeField]
		private string actionString;
		public string ActionString
		{
			get
			{
				return actionString;
			}

			set
			{
				ActionText.text = value;
				actionString = value;
			}
		}
		
		public void YesPressed()
		{
			ConfirmEvent?.Invoke();
			DeclineEvent.RemoveAllListeners();
			ConfirmEvent?.RemoveAllListeners();
			gameObject.SetActive(false);
		}

		public void NoPressed()
		{
			DeclineEvent?.Invoke();
			DeclineEvent?.RemoveAllListeners();
			ConfirmEvent.RemoveAllListeners();
			gameObject.SetActive(false);
		}
	} 
