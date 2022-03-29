using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Button))]
public class RecordFile : MonoBehaviour
{
	private string filePath;

	public void SetSettings(string path, Action<string> buttonPressed)
	{
		string text = Regex.Match(path, "[¸à-ÿÀ-ß_0-9-]*.mp4").Value;
		text = text.Replace(".mp4", "");
		//string text = path;
		GetComponent<Text>().text = text;
		filePath = path;

		GetComponent<Button>().onClick.AddListener(() =>
		{
			buttonPressed.Invoke(filePath);
		});
	}
}