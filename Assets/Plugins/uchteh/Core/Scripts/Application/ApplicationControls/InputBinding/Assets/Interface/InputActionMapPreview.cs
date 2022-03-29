using System.Collections.Generic;
using Core.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionMapPreview : MonoBehaviour
{
    private List<InputActionPreview> actions = new List<InputActionPreview>();
    [SerializeField] private Text inputMapText;
    public List<InputActionPreview> Actions => actions;

    public void PrepareMapPreview(InputActionMap inputActionMap, string asset)
    {
        foreach (var action in inputActionMap.actions)
        {
            var actionPreview = Instantiate(RebindPanelSettings.Default().ActionPreview, Vector3.zero, Quaternion.identity, transform).GetComponent<InputActionPreview>();
            actionPreview.transform.GetChild(0).GetComponent<Text>().text = action.name;
            actionPreview.PrepareActionPreview(action,asset);
            Actions.Add(actionPreview);
        }

        if (!inputMapText) inputMapText = GetComponentInChildren<Text>();
        inputMapText.text = inputActionMap.name;

        LayoutRebuilder.MarkLayoutForRebuild(transform.GetComponent<RectTransform>());

    }

}