using System.Collections.Generic;
using Core.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionAssetPreview : MonoBehaviour
{
    private List<InputActionMapPreview> maps= new List<InputActionMapPreview>();
    [SerializeField] private Text inputAssetText;
    public List<InputActionMapPreview> Maps => maps;

    public void PrepareMapPreview(InputActionAsset asset)
    {
        foreach (var actionMap in asset.actionMaps)
        {
            var actionMapPreview = Instantiate(RebindPanelSettings.Default().ActionMapPreview,
                Vector3.zero, Quaternion.identity, transform).GetComponent<InputActionMapPreview>();
            actionMapPreview.transform.GetChild(0).GetComponent<Text>().text = actionMap.name;
            actionMapPreview.PrepareMapPreview(actionMap,asset.name);
            Maps.Add(actionMapPreview);
        }

        if (!inputAssetText) inputAssetText = GetComponentInChildren<Text>();
        inputAssetText.text = asset.name;

        LayoutRebuilder.MarkLayoutForRebuild(transform.GetComponent<RectTransform>());
       
    }
}