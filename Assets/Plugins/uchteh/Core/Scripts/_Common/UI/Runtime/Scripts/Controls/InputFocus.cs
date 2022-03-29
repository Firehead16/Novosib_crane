﻿using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{
    [RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/InputFocus")]
    public class InputFocus : MonoBehaviour
    {
        #region Private Variables

        // The input field we use for chat
        protected InputField _inputField;

        // When set to true, we will ignore the next time the "Enter" key is released
        public bool _ignoreNextActivation = false;

        #endregion

        void Start()
        {
            _inputField = GetComponent<InputField>();
        }

        void Update()
        {
            // Check if the "Enter" key was just released with the chat input not focused
            if (Input.GetKeyUp(KeyCode.Return) && !_inputField.isFocused)
            {
                // If we need to ignore the keypress, do nothing - otherwise activate the input field
                if (_ignoreNextActivation)
                {
                    _ignoreNextActivation = false;
                }
                else
                {
                    _inputField.Select();
                    _inputField.ActivateInputField();
                }
            }
        }

        public void buttonPressed()
        {
            // Do whatever you want with the input field text here

            // Make note of whether the input string was empty, and then clear it out
            bool wasEmpty = _inputField.text == "";
            _inputField.text = "";

            // If the string was not empty, we should reactivate the input field
            if (!wasEmpty)
            {
                _inputField.Select();
                _inputField.ActivateInputField();
            }
        }

        public void OnEndEdit(string textString)
        {
            // If the edit ended because we clicked away, don't do anything extra
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return;
            }

            // Do whatever you want with the input field text here

            // Make note of whether the input string was empty, and then clear it out
            bool wasEmpty = _inputField.text == "";
            _inputField.text = "";

            // if the input string was empty, then allow the field to deactivate
            if (wasEmpty)
            {
                _ignoreNextActivation = true;
            }
        }


    }
}