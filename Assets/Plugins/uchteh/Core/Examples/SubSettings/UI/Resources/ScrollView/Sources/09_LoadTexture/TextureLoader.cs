﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample09
{
    static class TextureLoader
    {
        public static void Load(string url, Action<(string Url, Texture Texture)> onSuccess) =>
            Loader.Instance.Load(url, onSuccess);

        class Loader : MonoBehaviour
        {
            readonly Dictionary<string, Texture> cache = new Dictionary<string, Texture>();

            static Loader instance;

            public static Loader Instance => instance ??
                (instance = FindObjectOfType<Loader>() ??
                    new GameObject(typeof(TextureLoader).Name).AddComponent<Loader>());

            public void Load(string url, Action<(string Url, Texture Texture)> onSuccess)
            {
                if (cache.TryGetValue(url, out var cachedTexture))
                {
                    onSuccess((url, cachedTexture));
                    return;
                }

                StartCoroutine(DownloadTexture(url, result =>
                {
                    cache[result.Url] = result.Texture;
                    onSuccess(result);
                }));
            }

            IEnumerator DownloadTexture(string url, Action<(string Url, Texture Texture)> onSuccess)
            {
                using (var request = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return request.SendWebRequest();

                    if (request.isNetworkError)
                    {
                        Debug.LogErrorFormat("Error: {0}", request.error);
                        yield break;
                    }

                    onSuccess((
                        url,
                        ((DownloadHandlerTexture) request.downloadHandler).texture
                    ));
                }
            }

            void OnDestroy()
            {
                foreach (var kv in cache)
                {
                    Destroy(kv.Value);
                }

                instance = null;
            }
        }
    }
}
