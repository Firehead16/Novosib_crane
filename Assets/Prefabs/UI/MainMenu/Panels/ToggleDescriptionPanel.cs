using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDescriptionPanel : MonoBehaviour
{
    [SerializeField]
    public Text descriptionText = null;

    [SerializeField]
    public Image image = null;

    /*private ToggleScene currToggleScene;

    internal void ChangeDescription(ToggleScene toggleScene, bool isToggleEnabled)
    {
        if (isToggleEnabled)
        {
            currToggleScene = toggleScene;
            descriptionText.text = toggleScene.QuestDescription;
            image.sprite = toggleScene.Image; 
            gameObject.SetActive(true);
        }
        else
        {
            if (currToggleScene == toggleScene)
            {
                gameObject.SetActive(false);
            }
        }
    }*/

    [SerializeField] private Toggle quest1, quest2, quest3, quest4;
    
    [SerializeField] private List<Sprite> screens = new List<Sprite>();
    [SerializeField] private List<string> descriptions = new List<string>();

    private void Update()
    {
        if (quest1.isOn) SetDescription(0);
        if (quest2.isOn) SetDescription(1);
        if (quest3.isOn) SetDescription(2);
        if (quest4.isOn) SetDescription(3);
    }

    void SetDescription(int i)
    {
        //Debug.Log(image.sprite);
        image.sprite = screens[i];
        //Debug.Log(image.sprite + " " + screens[i]);
        descriptionText.text = descriptions[i];
    }
}
