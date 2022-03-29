using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using Core.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionAssetsPreviewPanel : Panel, IRebindMenu
{
    [SerializeField, Tooltip("Scroll view content")]
    private Transform content = null;

    private Dictionary<string, InputActionAsset> originalAssets;
    private Dictionary<string, InputActionAsset> changedAssets = new Dictionary<string, InputActionAsset>();
    private List<InputActionAssetPreview> assets;

    [SerializeField] 
    private Button QuitButton= null;

    [SerializeField] 
    private Button SaveButton = null;

    [SerializeField] 
    private Button DeviceButton = null;


    public event Action<BindingButton> OnBindingButtonClick;
    public event Action OnQuitButtonClick;
    public event Action OnEditDeviceButtonClick;

    public event Action<Dictionary<string, InputActionAsset>> OnSaveButtonClick;
    //todo кнопка по умолчанию

    private Dictionary<string, InputActionAsset> ChangedAssets => changedAssets;
    
    public override void Initialize()
    {
        base.Initialize();

        QuitButton.onClick.AddListener(() => OnQuitButtonClick?.Invoke());
        SaveButton.onClick.AddListener(() => OnSaveButtonClick?.Invoke(ChangedAssets));
        DeviceButton.onClick.AddListener(() => OnEditDeviceButtonClick?.Invoke());
    }

    public override void Notify(Message message)
    {
        base.Notify(message);
        switch (message.Type)
        {
	        case Messages.RebindControl.ShowRebindAssets:
                originalAssets = new Dictionary<string, InputActionAsset>();

                foreach (var asset in RebindAssetsSettings.Default().Assets)
                {
                    originalAssets.Add(asset.name, asset);
                }
                PrepareAssetPreview(originalAssets);
                break;
        }
    }


    private void PrepareAssetPreview(Dictionary<string, InputActionAsset> inputActionAssets)
    {
        foreach (Transform contentChild in content)
        {
            Destroy(contentChild.gameObject);
        }

        assets = new List<InputActionAssetPreview>();
        foreach (var asset in inputActionAssets)
        {
            var assetPreview = Instantiate(RebindPanelSettings.Default().ActionAssetPreview,
                Vector3.zero, Quaternion.identity, content).GetComponent<InputActionAssetPreview>();
            assetPreview.transform.GetChild(0).GetComponent<Text>().text = asset.Key;
            assetPreview.PrepareMapPreview(asset.Value);
            assets.Add(assetPreview);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());


        foreach (var bingingButton in GetButtons())
        {
            bingingButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnBindingButtonClick?.Invoke(bingingButton);
            });
            bingingButton.OnUpdated +=()=> UpdateAsset(bingingButton);
        }

        LayoutRebuilder.MarkLayoutForRebuild(content.GetComponent<RectTransform>());

    }

    private void UpdateAsset(BindingButton bingingButton)
    {
        var overridedAsset = new InputActionAsset();
        overridedAsset.name = originalAssets[bingingButton.Asset].name;
        foreach (var map in originalAssets[bingingButton.Asset].actionMaps)
        {
            var tempMap = new InputActionMap(map.name);
            //bool isUpdated = false;
            foreach (var action in map.actions)
            {
                tempMap.AddAction(action.name);
            }

            foreach (var binding in map.bindings)
            {
                var changedBinding = GetBindings().First(x => x.InputBinding.id == binding.id);
                tempMap.AddBinding(changedBinding.InputBinding);
            }
            overridedAsset.AddActionMap(tempMap);
        }
        foreach (var ctrlScheme in originalAssets[bingingButton.Asset].controlSchemes)
        {
            overridedAsset.AddControlScheme(ctrlScheme);
        }
    
        if (changedAssets.ContainsKey(bingingButton.Asset))
        {
            changedAssets[bingingButton.Asset] = overridedAsset;
        }
        else { changedAssets.Add(bingingButton.Asset, overridedAsset); }
    }

    private List<BindingButton> GetButtons() => assets
	    .SelectMany(ass => ass.Maps
		    .SelectMany(m => m.Actions)
		    .SelectMany(act => act.BindingButtons)).ToList();

    private List<IBindingPreview> GetBindings() => assets
	    .SelectMany(ass => ass.Maps
		    .SelectMany(m => m.Actions)
		    .SelectMany(act => act.Bindings)).ToList();
}