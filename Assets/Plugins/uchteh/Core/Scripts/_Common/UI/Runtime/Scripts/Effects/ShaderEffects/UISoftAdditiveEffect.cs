﻿using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{
    [AddComponentMenu("UI/Effects/Extensions/UISoftAdditiveEffect")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class UISoftAdditiveEffect : MonoBehaviour
    {
        MaskableGraphic mGraphic;

        // Use this for initialization
        void Start()
        {
            SetMaterial();
        }

        public void SetMaterial()
        {
            mGraphic = this.GetComponent<MaskableGraphic>();
            if (mGraphic != null)
            {
                if (mGraphic.material == null || mGraphic.material.name == "Default UI Material")
                {
                    //Applying default material with UI Image Crop shader
                    mGraphic.material = new Material(Shader.Find("UI Extensions/UISoftAdditive"));
                }
            }
            else
            {
                Debug.LogError("Please attach component to a Graphical UI component");
            }
        }
        public void OnValidate()
        {
            SetMaterial();
        }
    }
}