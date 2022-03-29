using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Core.Gameplay.Questing;
using Core.Settings;
using UnityEngine;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
public class RecorderMmpeg : MonoBehaviour
{
	#region  DLL needed to simulate console signal

	[DllImport("kernel32.dll")]
	static extern bool GenerateConsoleCtrlEvent(int dwCtrlEvent, int dwProcessGroupId);
	[DllImport("kernel32.dll")]
	static extern bool SetConsoleCtrlHandler(IntPtr handlerRoutine, bool add);
	[DllImport("kernel32.dll")]
	static extern bool AttachConsole(int dwProcessId);
	[DllImport("kernel32.dll")]
	static extern bool FreeConsole();

	#endregion

	#region  Setting menu

	public enum RecordType
	{
		GDIGRAB,
		DSHOW
	}

	public enum Bitrate
	{
		_1000k,
		_1500k,
		_2000k,
		_2500k,
		_3000k,
		_3500k,
		_4000k,
		_5000k,
		_8000k
	}

	public enum Framerate
	{
		_14,
		_24,
		_30,
		_45,
		_60
	}

	public enum Resolution
	{
		_1280x720,
		_1920x1080,
		_Auto
	}

	public enum OutputPath
	{
		Desktop,
		StreamingAsset,
		DataPath,
		Custom
	}

	public enum VideoCodec
	{
		Auto,
		H264,
		H265,
		NVENC
	}

	#endregion

	#region  member
	[Tooltip("Enable Debug to display the CMD window, otherwise it will not be displayed.")]
	[SerializeField]
	private bool _debug = false;

	[Tooltip("DSHOW-record full screen \nGUIGRAB-record game window (only for release version)")]

	public VideoCodec videoCodec = VideoCodec.Auto;

	public RecordType recordType = RecordType.DSHOW;
	public Resolution resolution = Resolution._1280x720;
	public Framerate framerate = Framerate._24;
	public Bitrate bitrate = Bitrate._1500k;
	public OutputPath outputPath = OutputPath.Desktop;

	public bool IsOldPlayer;


	public string customOutputPath = @"D:/Records";
	public bool IsRecording { get { return _isRecording; } }

	/** ffmpeg parameter description
	 * ffmpeg -i file.mp4 : ���������� � �����
	 * ffmpeg -codecs: ������
	 * ffmpeg -formats: �������
	 * ffmpeg -encoders : ������ ���������
	 * ffmpeg -decoders : ������ ���������
	 * ffmpeg -h encoder=mpeg4 : ���������� �� ��������
	 * ffmpeg -h decoder=aac : ���������� � ��������
           * -f: format
           * gdigrab: ffmpeg's built-in method for grabbing the Windows desktop, supporting grabbing windows with specified names
           * dshow: dependent on third-party software Screen Capture Recorder (hereinafter referred to as SCR)
           * -i: input source
           * title: the name of the window to be recorded, only used in GDIGRAB mode
           * video: video playback hardware name or "screen-capture-recorder", the latter depends on SCR
           * audio: audio playback hardware name or "virtual-audio-capturer", the latter depends on SCR
           * -preset ultrafast: encode at the fastest speed and generate large video files
           * -c:v: video encoding method
           * -c:a: audio encoding method
           * -b:v: video bit rate
           * -r: video frame rate
           * -s: video resolution
           * -y: output files overwrite existing files without prompting
     * 
           * Official FFMPEG document: http://ffmpeg.org/ffmpeg-all.html
           * Screen Capture Recorder homepage: https://github.com/rdp/screen-capture-recorder-to-video-windows-free
     */

	private string _ffpath;
	private string _ffargs;

	private int _pid;
	public bool _isRecording = false;
	#endregion

/*#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
    private void Start()
    {
        _debug = false;
    }
#endif

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (_debug) Debug.Log("FFRecorder-CMD window is enabled.");
		else Debug.Log("FFRecorder-CMD window is disabled.");

		if (recordType == RecordType.GDIGRAB)
		{
			Debug.Log("FFRecorder-Use [GDIGRAB] mode to record the current window.");
			Debug.LogError("FFRecorder-[GDIGRAB] mode is not available in the editor.");
		}
		else if (recordType == RecordType.DSHOW)
		{
			Debug.Log("FFRecorder-Use [DSHOW] mode to record full screen.");
		}
	}

	
#endif*/
	private void Start()
	{
		string AppPath = Path.GetDirectoryName(Application.dataPath);
		if (File.Exists(AppPath + "\\Config\\VideoRecordSettings.ini"))
		{
			using (StreamReader sr = new StreamReader(AppPath + "\\Config\\VideoRecordSettings.ini"))
			{
				bitrate = (Bitrate)Enum.Parse(typeof(Bitrate), sr.ReadLine());
				resolution = (Resolution)Enum.Parse(typeof(Resolution), sr.ReadLine());
				framerate = (Framerate)Enum.Parse(typeof(Framerate), sr.ReadLine());
			}
		} else Debug.Log("not Yeet");
	}
	
	public void StartRecording()
	{
		if (_isRecording)
		{
			Debug.LogError("FFRecorder::StartRecording-There is currently a recording process.");
			return;
		}

		// Kill the existing ffmpeg process, do not add the .exe suffix
		Process[] goDie = Process.GetProcessesByName("ffmpeg");
		foreach (Process p in goDie) p.Kill();

		// Parse the settings, if the settings are correct, start recording
		bool validSettings = ParseSettings();
		if (validSettings)
		{
			Debug.Log("FFRecorder::StartRecording-execute command:" + _ffpath + " " + _ffargs);
			StartCoroutine(IeRecording());
		}
		else
		{
			Debug.LogError("FFRecorder::StartRecording-Improper setting, recording is cancelled, please check the console output.");
		}
	}

	public void StopRecording(Action onStopRecording = null)
	{
		if (!_isRecording)
		{
			Debug.LogError("FFRecorder::StopRecording-There is currently no recording process, the operation has been cancelled.");
			return;
		}

		StartCoroutine(IeExitCmd(onStopRecording));
	}

	public string name;

	private bool ParseSettings()
	{
		_ffpath = Application.streamingAssetsPath + @"/ffmpeg/ffmpeg.exe";
		name = QuestStorage.Instance.UserName;
		name = name.Replace(' ', '-');
		name = name.Remove(name.Length - 1);
		Debug.Log(name);
		//string name = Application.productName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
		string fileName = name + '_' + DateTime.Now.ToString("dd-MM-yyyy-HH-mm");

		// resolution
		string s;
		if (resolution == Resolution._1280x720)
		{
			int w = 1280;
			int h = 720;
			//if (Screen.width < w)
			//{
			//    w = Screen.width;
			//    Debug.LogWarning(string.Format("The horizontal recording resolution is greater than the horizontal resolution of the screen and has been automatically reduced to {0}.", w));
			//}
			//if (Screen.height < h)
			//{
			//    h = Screen.height;
			//    Debug.LogWarning(string.Format("The vertical resolution of the recording is greater than the vertical resolution of the screen and has been automatically reduced to {0}.", h));
			//}
			s = w + "x" + h;
		}
		else if (resolution == Resolution._1920x1080)
		{
			int w = 1920;
			int h = 1080;
			//if (Screen.width < w)
			//{
			//    w = Screen.width;
			//    Debug.LogWarning(string.Format("The horizontal recording resolution is greater than the horizontal resolution of the screen and has been automatically reduced to {0}.", w));
			//}
			//if (Screen.height < h)
			//{
			//    h = Screen.height;
			//    Debug.LogWarning(string.Format("The vertical resolution of the recording is greater than the vertical resolution of the screen and has been automatically reduced to {0}.", h));
			//}
			s = w + "x" + h;
		}
		else  /*(resolution == Resolution._Auto)*/
		{
			s = Screen.width + "x" + Screen.height;
		}
		// Frame rate
		string r = framerate.ToString().Remove(0, 1);
		// bit rate
		string b = bitrate.ToString().Remove(0, 1);

		// output location
		string output;
		if (outputPath == OutputPath.Desktop) output = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + Application.productName + "_Records";
		else if (outputPath == OutputPath.DataPath) output = Application.dataPath + "/" + Application.productName + "_Records";
		else if (outputPath == OutputPath.StreamingAsset) output = Application.streamingAssetsPath + "/" + Application.productName + "_Records";
		else /*(outputPath == OutputPath.Custom)*/ output = customOutputPath;

		// ��������� ��������� ������
		if (recordType == RecordType.GDIGRAB)
		{
			_ffargs = $"-f gdigrab -i desktop -f dshow -i audio=\"virtual-audio-capturer\" -y -preset fast -rtbufsize 512M -b:v {b} -r {r} -s {s}";
		}
		else
		{
			// Parameters: -b bit rate -r frame rate -s resolution file path file name
			_ffargs = $"-f dshow -i video=\"screen-capture-recorder\" -f dshow -i audio=\"virtual-audio-capturer\" -y -rtbufsize 512M -b:v {b} -r {r} -s {s}";
		}

		// ����������
		switch (videoCodec)
		{
			case VideoCodec.Auto:
				break;
			case VideoCodec.H264:
				_ffargs += " -vcodec libx264";
				break;
			case VideoCodec.H265:
				_ffargs += " -vcodec libx265";
				break;
			case VideoCodec.NVENC:
				_ffargs += " -vcodec h264_nvenc";
				break;
		}

		// ��� ������ �������
		if (IsOldPlayer)
		{
			_ffargs += " -pix_fmt yuv420p";
		}

		_ffargs += $" {output}/{fileName}.mp4";
		
		

		// ������� �������� �����
		if (!System.IO.Directory.Exists(output))
		{
			try
			{
				System.IO.Directory.CreateDirectory(output);
			}
			catch (Exception e)
			{
				Debug.LogError("FFRecorder::ParseSettings - " + e.Message);
				return false;
			}
		}

		return true;
	}

	private IEnumerator IeRecording()
	{
		yield return null;

		Process ffp = new Process();
		ffp.StartInfo.FileName = _ffpath;                   // Location of process executable file
		ffp.StartInfo.Arguments = _ffargs;                  // Command line parameters passed to the executable file
		ffp.StartInfo.CreateNoWindow = !_debug;             // Whether to display the console window
		ffp.StartInfo.UseShellExecute = _debug;             // Whether to use the operating system shell program to start the process
		ffp.Start();                                        // Start the process

		_pid = ffp.Id;
		_isRecording = true;
	}

	private IEnumerator IeExitCmd(Action onStopRecording)
	{
		// Attach the current process to the console of the pid process
		AttachConsole(_pid);
		// Set the handle of console events to Zero, that is, the current process does not respond to console events
		// Avoid sending [Ctrl C] command to the console to end with the current process
		SetConsoleCtrlHandler(IntPtr.Zero, true);
		// Send [Ctrl C] end command to the console
		// ffmpeg will receive the instruction to stop recording
		GenerateConsoleCtrlEvent(0, 0);

		// ffmpeg cannot be stopped immediately, wait for a while, otherwise the video cannot be played
		yield return new WaitForSecondsRealtime(10.0f);//(3.0f);

		// Unload the handle of the console event, otherwise the subsequent ffmpeg call cannot stop normally
		SetConsoleCtrlHandler(IntPtr.Zero, false);
		// Strip off the attached console
		FreeConsole();

		_isRecording = false;

		if (onStopRecording != null)
		{
			onStopRecording();
		}
	}

	private void OnDestroy()
	{
		if (_isRecording)
		{
			try
			{
				Debug.LogError("FFRecorder::OnDestroy-The recording process ended abnormally and the output file may not be playable.");
				Process.GetProcessById(_pid).Kill();
			}
			catch (Exception e)
			{
				Debug.LogError("FFRecorder::OnDestroy - " + e.Message);
			}
		}
	}
}
