using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	public class DropMenu : MonoBehaviour
	{
	    [SerializeField] 
	    private Transform content = null;

	    public Transform Content => content;

	    [SerializeField]
        private bool isNeedDestroyOnExit;

	    public bool IsNeedDestroyOnExit
	    {
	        get => isNeedDestroyOnExit;
	        set => isNeedDestroyOnExit = value;
	    }

	    /// <summary>
        /// Кнопки функций органов
        /// </summary>
        private List<Button> funcButtons = new List<Button>();

	    public void AddButton(Button btn)
		{
			btn.transform.localPosition = Vector3.ProjectOnPlane(btn.transform.localPosition, Vector3.forward);
			btn.transform.localRotation = Quaternion.identity;

			funcButtons.Add(btn);
		}

		public void DeleteButton(Button btn)
		{
			funcButtons.Remove(btn);
		}

		public void ClearButtons()
		{
			foreach (Button btn in funcButtons)
			{
				Destroy(btn.gameObject);
			}
			funcButtons.Clear();
		}

		public void Destroy()
		{
			if (IsNeedDestroyOnExit)
			{
				Destroy(gameObject);
			}
		}
	} 
}
