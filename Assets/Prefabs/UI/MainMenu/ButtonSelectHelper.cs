using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private GameObject selectObject = null;

	[SerializeField]
	private Color exitColor = Color.white;

	[SerializeField]
	private Color enterColor = Color.white;

	private Button button;

	private Text buttonText;

	private void Start()
	{
		button = GetComponent<Button>();
		buttonText = button.GetComponentInChildren<Text>();
		buttonText.color = exitColor;
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		EnableSelect();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		DisableSelect();
	}

	private void OnDisable()
	{
		DisableSelect();
	}


	private void EnableSelect()
	{
		selectObject?.SetActive(true);
		buttonText.color = enterColor;
	}

	private void DisableSelect()
	{
		selectObject?.SetActive(false);
		buttonText.color = exitColor;
	}
}
