using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Button))]
public class RecordTextFile : MonoBehaviour
{
    private string filePath;

    public void SetSettings(string path, Action<string> buttonPressed)
    {
        string text = Regex.Match(path, "[¸à-ÿÀ-ß_0-9-]*.ini").Value;
        text = text.Replace(".ini", "");
        GetComponent<Text>().text = text;
        filePath = path;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            buttonPressed.Invoke(filePath);
        });
    }
}