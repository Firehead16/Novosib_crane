using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicBackground : MonoBehaviour
{
    [SerializeField]
    private Image backgroundImage = null;

    [SerializeField]
    private List<Sprite> sprites = null;

    [SerializeField]
    private float repeateTime = 10;

    private int imageIndex;

    private void Awake()
    {
        InvokeRepeating("ChangeImage", 0, repeateTime);
    }

    private void ChangeImage()
    {
        backgroundImage.sprite = sprites[imageIndex];

        if (imageIndex == sprites.Count - 1)
        {
            imageIndex = 0;
        }
        else
        {
            imageIndex++;
        }
        
    }
}
