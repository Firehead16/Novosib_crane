using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("UI/Extensions/PPIViewer")]
    public class PPIViewer : MonoBehaviour
    {
        private Text label;

        void Awake()
        {
            label = GetComponent<Text>();
        }

        void Start()
        {
            if (label != null)
            {
                label.text = "PPI: " + Screen.dpi.ToString();
            }
        }
    }
}