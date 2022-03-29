using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class Recorder : MonoBehaviour
{

	[SerializeField]
	private string directoryName = "\\Recording";

	private RecorderMmpeg recorderMmpeg;

	private string directory;

	private bool isStartRecording;

	private void Awake()
	{
		recorderMmpeg = GetComponent<RecorderMmpeg>();
		directory = Path.GetDirectoryName(Application.dataPath) + directoryName;
	}


	[Button]
	public void StartRecording()
	{
		isStartRecording = true;
		recorderMmpeg.outputPath = RecorderMmpeg.OutputPath.Custom;
		recorderMmpeg.customOutputPath = directory;
		recorderMmpeg.StartRecording();
	}

	[Button]
	public void StopRecording()
	{
		if (isStartRecording)
		{
			isStartRecording = false;
			recorderMmpeg.StopRecording(() => Debug.Log("End recording."));
		}
	}
}

