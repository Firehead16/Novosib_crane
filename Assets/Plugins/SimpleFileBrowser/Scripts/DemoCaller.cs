using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace GracesGames.SimpleFileBrowser.Scripts
{
	// Demo class to illustrate the usage of the FileBrowser script
	// Able to save and load files containing serialized data (e.g. text)

	[RequireComponent(typeof(RectTransform))]
	public class DemoCaller : MonoBehaviour
	{

		// Use the file browser prefab
		public GameObject FileBrowserPrefab;

		// Define a file extension
		public string[] FileExtensions;

		// Input field to get text to save
		private GameObject textToSaveInputField;

		// Label to display loaded text
		private GameObject loadedText;

		// Variable to save intermediate input result
		private string textToSave;

		// Режим проводника
		public bool PortraitMode;


		private GameObject fileBrowserObject;
		private FileBrowser fileBrowserScript;
	

		public void Open(ViewMode viewMode, RectTransform parent, string path)
		{
			fileBrowserObject = Instantiate(FileBrowserPrefab);
			fileBrowserObject.transform.SetParent(parent);
			fileBrowserObject.name = "FileBrowser";

			fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
			fileBrowserScript.MySetupFileBrowser(viewMode, parent, path);
			fileBrowserScript.OpenFilePanel(FileExtensions);
			fileBrowserScript.OnFileSelect += LoadFileUsingPath;
		}

		public void Close()
		{
			foreach (var child in transform.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (child != this)
				{
					Destroy(child.gameObject);
				}
			}
		}


		// Open the file browser using boolean parameter so it can be called in GUI
		public void OpenFileBrowser(bool saving)
		{
			OpenFileBrowser(saving ? FileBrowserMode.Save : FileBrowserMode.Load);
		}

		// Open a file browser to save and load files
		private void OpenFileBrowser(FileBrowserMode fileBrowserMode)
		{

			// Create the file browser and name it
			GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
			fileBrowserObject.name = "FileBrowser";

			// Set the mode to save or load
			FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
			fileBrowserScript.SetupFileBrowser(PortraitMode ? ViewMode.Portrait : ViewMode.Landscape);
			if (fileBrowserMode == FileBrowserMode.Save)
			{
				fileBrowserScript.SaveFilePanel("DemoText", FileExtensions);
				// Subscribe to OnFileSelect event (call SaveFileUsingPath using path) 
				fileBrowserScript.OnFileSelect += SaveFileUsingPath;
			}
			else
			{
				fileBrowserScript.OpenFilePanel(FileExtensions);
				// Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
				fileBrowserScript.OnFileSelect += LoadFileUsingPath;
			}
		}

		// Updates the text to save with the new input (current text in input field)
		public void UpdateTextToSave(string text)
		{
			textToSave = text;
		}

		// Saves a file with the textToSave using a path
		private void SaveFileUsingPath(string path)
		{
			// Make sure path and _textToSave is not null or empty
			if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(textToSave))
			{
				BinaryFormatter bFormatter = new BinaryFormatter();
				// Create a file using the path
				FileStream file = File.Create(path);
				// Serialize the data (textToSave)
				bFormatter.Serialize(file, textToSave);
				// Close the created file
				file.Close();
			}
			else
			{
				Debug.Log("Invalid path or empty file given");
			}
		}

		// Loads a file using a path
		private void LoadFileUsingPath(string path)
		{
			if (path.Length != 0)
			{
				BinaryFormatter bFormatter = new BinaryFormatter();
				// Open the file using the path
				FileStream file = File.OpenRead(path);
				// Convert the file from a byte array into a string
				string fileData = bFormatter.Deserialize(file) as string;
				// We're done working with the file so we can close it
				file.Close();
				// Set the LoadedText with the value of the file
				loadedText.GetComponent<Text>().text = "Loaded data: \n" + fileData;
			}
			else
			{
				Debug.Log("Invalid path given");
			}
		}
	}
}