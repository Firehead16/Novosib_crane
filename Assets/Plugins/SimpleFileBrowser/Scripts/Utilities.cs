using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GracesGames.Common.Scripts {

	public class Utilities : MonoBehaviour {

		// Проверка наличия объекта 
		public static GameObject FindGameObjectOrError(string objectName) {
			GameObject foundGameObject = GameObject.Find(objectName);
			return foundGameObject;
		}

		// Проверка наличия кнопки и добавление к ней события
		public static GameObject FindButtonAndAddOnClickListener(string buttonName, UnityAction listenerAction) {
			GameObject button = FindGameObjectOrError(buttonName);
			button?.GetComponent<Button>().onClick.AddListener(listenerAction);
			return button;
		}
	}
}