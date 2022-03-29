using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "Demo", menuName = "ScriptableObjects/Demo", order = 1)]
public class Demo : ScriptableObject
{
    [LabelText("Название демонстрации")]
    public string Name;

    [LabelText("Описание демонстрации"), TextArea(1, 5)]
    public string Description;

    [LabelText("Тип демонстрации")]
    public DemoType DemoType;

    [SerializeField, LabelText("Шаги демонстрации"), ListDrawerSettings()]
    private List<DemoStep> stepList = new List<DemoStep>();
    public List<DemoStep> StepList => stepList;

}
