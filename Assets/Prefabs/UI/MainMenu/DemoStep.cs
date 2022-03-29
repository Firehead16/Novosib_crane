using System;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
public class DemoStep
{
    [LabelText("Название")]
    public string Name;

    [LabelText("Описание"), TextArea(1, 5)]
    public string Description;

    [LabelText("Позиция камеры")]
    public Sprite Image;

    [LabelText("Позиция камеры")]
    public Vector3 MovePoint;

    [LabelText("Поворот камеры")]
    public Quaternion RotationPoint;
}