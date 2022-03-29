using UnityEngine;

namespace Core.Settings
{
    [CreateAssetMenu(fileName = "RebindPanelSettings", menuName = "Settings/RebindPanelSettings")]
    public class RebindPanelSettings : SubSettings<RebindPanelSettings>
    {
        [SerializeField] private GameObject inputActionAssetPreview=null;
        [SerializeField] private GameObject inputActionMapPreview = null;
        [SerializeField] private GameObject inputActionPreview = null;

        [SerializeField] private GameObject bindingSetButton = null;
        [SerializeField] private GameObject compositBindingPreview = null;

       

        public GameObject ActionAssetPreview => inputActionAssetPreview;

        public GameObject ActionMapPreview => inputActionMapPreview;

        public GameObject ActionPreview => inputActionPreview;

        public GameObject BindingSetButton => bindingSetButton;

        public GameObject CompositBindingPreview => compositBindingPreview;

    }
}