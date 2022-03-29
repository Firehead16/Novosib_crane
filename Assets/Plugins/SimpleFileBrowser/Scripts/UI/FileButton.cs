using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Paroxe.PdfRenderer;

namespace GracesGames.SimpleFileBrowser.Scripts.UI
{

	public class FileButton : MonoBehaviour, IPointerClickHandler
	{

		// Проводник
		private FileBrowser _fileBrowser;

		// Путь для файла
		private string _path = "";

		// Интерактивная кнопка
		private bool _interactable;

		// Клик и двойной клик
		private int _clickCount;
		private float _firstClickTime;
		private float _currentTime;
		// Change this constant to tweak the time between single and double clicks
		private const float DoubleClickInterval = 0.25f;

		// Установка значения для кнопки
		public void Setup(FileBrowser fileBrowser, string path, bool interactable)
		{
			_fileBrowser = fileBrowser;
			_path = path;
			_interactable = interactable;
		}

		// При одном клике происходит метод FileClick
		// При двойном нажаитии вызываются FileClick и SelectFile методы
		public void OnPointerClick(PointerEventData eventData)
		{
			if (_interactable)
			{
				_clickCount++;
			}

			if (_clickCount != 1) return;
			_firstClickTime = eventData.clickTime;
			_currentTime = _firstClickTime;
			StartCoroutine(ClickRoutine());
		}

		private IEnumerator ClickRoutine()
		{
			while (_clickCount != 0)
			{
				yield return new WaitForEndOfFrame();

				_currentTime += Time.deltaTime;

				if (!(_currentTime > _firstClickTime + DoubleClickInterval)) continue;
				if (_clickCount == 1)
				{
					//_fileBrowser.FileClick(_path);
					ShowDocument(_path);
				}
				else
				{
					//_fileBrowser.FileClick(_path);
					//_fileBrowser.SelectFile();
				}

				_clickCount = 0;
			}
		}


		void ShowDocument(string path)
		{
			PDFViewer viewer = FindObjectOfType<PDFViewer>();

			if (viewer)
			{
				viewer.FilePath = path;
				viewer.LoadDocument();
			}	
		
		}

	}
}
