using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionPreview : MonoBehaviour
{
    private List<IBindingPreview> bindings = new List<IBindingPreview>();
    [SerializeField] private Text inputActionText;
    public List<IBindingPreview> Bindings => bindings;
    public List<BindingButton> BindingButtons => bindings.Where(x=>x is BindingButton).Cast<BindingButton>().ToList();
    public InputAction Action { get; set; }

    public void PrepareActionPreview(InputAction action, string asset)
    { 
        Transform compositeTransform = transform;
        bool isFat = true;
        for (var index = 0; index < action.bindings.Count; index++)
        {
            var binding = action.bindings[index];
            IBindingPreview bindingPreview;
            BindingButton bindingButton;

            if (binding.isComposite)
            {
                isFat = false;
                   compositeTransform =
                    CreateBindingPreview(RebindPanelSettings.Default().CompositBindingPreview, transform).transform;
                   bindingPreview = new CompositBindingPreview();
                   bindingPreview.InputBinding = binding;
            }
            else if (binding.isPartOfComposite)
            {
                bindingPreview = bindingButton = CreateBindingPreview(RebindPanelSettings.Default().BindingSetButton, compositeTransform)
                    .GetComponent<BindingButton>();
                bindingButton.Action = action;
                bindingButton.InputBinding = binding;
                bindingButton.Asset = asset;
                bindingButton.BindingIndex = index;
            }
            else
            {
                bindingPreview = bindingButton = CreateBindingPreview(RebindPanelSettings.Default().BindingSetButton, transform)
                    .GetComponent<BindingButton>();
                bindingButton.Action = action;
                bindingButton.InputBinding = binding;
                bindingButton.Asset = asset;
                bindingButton.BindingIndex = index;
            }
           Bindings.Add(bindingPreview);
        }
        if (isFat)
        {
            GetComponent<VerticalLayoutGroup>().padding.left += RebindPanelSettings.Default().CompositBindingPreview
                .GetComponent<VerticalLayoutGroup>().padding.left;
        }

        if (!inputActionText) inputActionText = GetComponentInChildren<Text>();
        inputActionText.text = action.name;
        LayoutRebuilder.MarkLayoutForRebuild(transform.GetComponent<RectTransform>());
    }

    GameObject CreateBindingPreview(GameObject prefab, Transform parent)
    {
        return Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
    }



}