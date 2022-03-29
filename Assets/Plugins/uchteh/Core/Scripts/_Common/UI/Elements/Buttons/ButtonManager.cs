using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

namespace Core.Ui
{
    public class ButtonManager : MonoBehaviour, IPointerEnterHandler
    {
        // Content
        public string buttonText = "Button";
        public UnityEvent buttonEvent;
        public AudioClip hoverSound;
        public AudioClip clickSound;
        Button buttonVar;

        // Resources
        public TextMeshProUGUI normalText;
        public TextMeshProUGUI highlightedText;
        public AudioSource soundSource;

        // Settings
        public bool useCustomContent = false;
        public bool enableButtonSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;

        void Start()
        {
            if (useCustomContent == false && normalText == null && highlightedText == null)
                UpdateUi();

            if (buttonVar == null)
                buttonVar = gameObject.GetComponent<Button>();

            buttonVar.onClick.AddListener(delegate
            {
                buttonEvent.Invoke();
            });

            if (enableButtonSounds == true && useClickSound == true)
            {
                buttonVar.onClick.AddListener(delegate
                {
                    soundSource.PlayOneShot(clickSound);
                });
            }
        }

        public void UpdateUi()
        {
            normalText.text = buttonText;
            highlightedText.text = buttonText;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useHoverSound == true && buttonVar.interactable == true)
                soundSource.PlayOneShot(hoverSound);
        }
    }
}