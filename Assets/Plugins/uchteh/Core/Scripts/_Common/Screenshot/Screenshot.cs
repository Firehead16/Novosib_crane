using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Screenshot : MonoBehaviour
{
	private Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	[Button]
	void Print()
	{
		var pixelHeight = 1920;
		var pixelWidth = 1080;

		RenderTexture renderTexture = new RenderTexture(pixelWidth, pixelHeight, 24);
		cam.targetTexture = renderTexture;
		cam.Render();
		cam.targetTexture = null;

		RenderTexture.active = renderTexture;

		Texture2D photo = new Texture2D(pixelWidth, pixelHeight, TextureFormat.RGB24, false);
		photo.ReadPixels(new Rect(0, 0, pixelWidth, pixelHeight), 0, 0);
		photo.Apply();
		DestroyImmediate(renderTexture);
		RenderTexture.active = null;

		System.IO.File.WriteAllBytes(ScreenShotName(), photo.EncodeToPNG());
	}


	private static string ScreenShotName()
	{
		return $"C:/Screens/{Application.productName}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
	}
}