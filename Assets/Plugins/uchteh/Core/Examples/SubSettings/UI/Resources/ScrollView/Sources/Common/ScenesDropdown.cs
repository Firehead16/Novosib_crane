﻿/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Ui.Examples.FancyScrollView
{
    [RequireComponent(typeof(Dropdown))]
    class ScenesDropdown : MonoBehaviour
    {
        readonly string[] scenes =
        {
            "01_Basic",
            "02_FocusOn",
            "03_InfiniteScroll",
            "04_Metaball",
            "05_Voronoi",
            "06_LoopTabBar",
            "07_ScrollRect",
            "08_GridView",
            "09_LoadTexture"
        };

        void Start()
        {
            var dropdown = GetComponent<Dropdown>();
            dropdown.AddOptions(scenes.Select(x => new Dropdown.OptionData(x)).ToList());
            dropdown.value = Mathf.Max(0, Array.IndexOf(scenes, SceneManager.GetActiveScene().name));
            dropdown.onValueChanged.AddListener(value =>
                SceneManager.LoadScene(scenes[value], LoadSceneMode.Single));
        }
    }
}
