using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ToggleScene
{
    public Toggle Toggle;
    public string SceneName;

    [TextArea(5, 50)]
    public string QuestDescription;
    
    public Sprite Image;
}
