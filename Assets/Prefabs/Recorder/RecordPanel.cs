using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VideoHelper;
using UnityEngine;
using UnityEngine.UI;

public class RecordPanel : MonoBehaviour
{
	public event Action OnBackToMainMenuButtonClick;

	[SerializeField] 
	private Button backButton = null;

	[SerializeField]
	private string directoryName = "";
	
	[SerializeField]
	private string directoryTextsName = "";

	[SerializeField]
	private GameObject fileContainer = null;
	[SerializeField]
	private GameObject textfileContainer = null;

	[SerializeField]
	private RecordFile recordFilePrefab = null;
	
	[SerializeField]
	private RecordTextFile recordTextFilePrefab = null;

	[ShowInInspector,ReadOnly] 
	private List<RecordFile> recordFiles;
	
	[ShowInInspector,ReadOnly] 
	private List<RecordTextFile> recordTextFiles;

	[SerializeField]
	private VideoController videoController = null;

	[SerializeField] 
	private TMP_Text CurrentResult = null;

	private string directory;
	private string directoryTexts;


	private void Awake()
	{
		backButton.onClick.AddListener(() => OnBackToMainMenuButtonClick?.Invoke());

		directory = Path.GetDirectoryName(Application.dataPath) + directoryName;
		directoryTexts = Path.GetDirectoryName(Application.dataPath) + directoryTextsName;
		recordFiles = new List<RecordFile>();
		recordTextFiles = new List<RecordTextFile>();
	}

	private void OnEnable()
	{
		GetRecordFiles();
		GetRecordTextFiles();
	}

	private void OnDisable()
	{
		videoController.gameObject.SetActive(false);
		ClearRecordFiles();
	}

	private void GetRecordFiles()
	{
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		else
		{
			string[] filePaths = Directory.GetFiles(directory);

			if (recordFiles.Any())
			{
				ClearRecordFiles();
			}

			foreach (string filePath in filePaths)
			{
				RecordFile recordFile = Instantiate(recordFilePrefab, fileContainer.transform);
				recordFile.SetSettings(filePath, ButtonPressed);
				recordFiles.Add(recordFile);
			}
		}

	}
	
	

	private void ClearRecordFiles()
	{
		foreach (var recordFile in recordFiles)
		{
			Destroy(recordFile.gameObject);
		}
		recordFiles.Clear();
	}
	
	

	private void ButtonPressed(string filePath)
	{
		videoController.gameObject.SetActive(true);
		videoController.PrepareForUrl(filePath);
	}
	
	
	
	private void GetRecordTextFiles()
	{
		if (!Directory.Exists(directoryTexts))
		{
			Directory.CreateDirectory(directoryTexts);
		}
		else
		{
			string[] filePaths = Directory.GetFiles(directoryTexts);

			if (recordTextFiles.Any())
			{
				ClearTextRecordFiles();
			}

			foreach (string filePath in filePaths)
			{
				RecordTextFile recordTextFile = Instantiate(recordTextFilePrefab, textfileContainer.transform);
				recordTextFile.SetSettings(filePath, ButtonTextPressed);
				recordTextFiles.Add(recordTextFile);
			}
		}

	}
	
	private void ClearTextRecordFiles()
	{
		foreach (var recordTextFile in recordTextFiles)
		{
			Destroy(recordTextFile.gameObject);
		}
		recordTextFiles.Clear();
	}
	
	private void ButtonTextPressed(string filePath)
	{
		CurrentResult.text = File.ReadAllText(filePath);
		CurrentResult.gameObject.SetActive(true);
		// videoController.gameObject.SetActive(true);
		// videoController.PrepareForUrl(filePath);
	}

}
