using UnityEngine;

namespace Core.Ui.ColorPicker
{
    public class ColorPickerTester : MonoBehaviour
    {
        [SerializeField]
	    private Renderer pickerRenderer = null;

        [SerializeField]
	    private ColorPickerControl picker = null;

	    private void Awake()
        {
            pickerRenderer = GetComponent<Renderer>();
        }

	    private void Start()
        {
            picker.onValueChanged.AddListener(color =>
            {
                pickerRenderer.material.color = color;
            });
        }
    }
}