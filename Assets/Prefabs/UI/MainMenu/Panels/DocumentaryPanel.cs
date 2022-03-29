using System;
using GracesGames.SimpleFileBrowser.Scripts;
using Paroxe.PdfRenderer;
using UnityEngine;
using UnityEngine.UI;

public class DocumentaryPanel : MonoBehaviour
{
	public event Action OnBackToMainMenuButtonClick;

	[SerializeField]
	private PDFViewer viewer = null;

	[SerializeField]
	private DemoCaller caller = null;

	[SerializeField]
	private string documentaryPath = "";

	[SerializeField]
	private Button backToMainButton = null;

	private string dir = "";

	private void Start()
	{
		backToMainButton.onClick.AddListener(() => OnBackToMainMenuButtonClick?.Invoke());
	}

	private void OnEnable()
	{
		dir = Environment.CurrentDirectory + documentaryPath;

#if UNITY_EDITOR
		dir = Application.dataPath + documentaryPath;
#endif

		Debug.Log(dir);

		try
		{
			caller.Open(ViewMode.Portrait, caller.GetComponent<RectTransform>(), dir);

		}
		catch (Exception exc)
		{
			Debug.LogError(exc.Message);
		}
	}


	private void OnDisable()
	{
		caller.Close();
		viewer.UnloadDocument();
	}
}
