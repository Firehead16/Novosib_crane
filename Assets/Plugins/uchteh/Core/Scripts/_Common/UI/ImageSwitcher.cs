using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSwitcher : MonoBehaviour
{
	public Sprite StartSprite, SecondSprite;

	private Image targetGraphics;
	private bool isSecond;

	
	private void Start()
	{
		targetGraphics = GetComponent<Image>();
		targetGraphics.sprite = StartSprite;
	}

	public void Switch()
	{
		targetGraphics.sprite = isSecond ? StartSprite : SecondSprite;
		isSecond = !isSecond;
	}

}