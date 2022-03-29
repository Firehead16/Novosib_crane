using UnityEngine;

namespace Core.Ui
{
	public class UIElementInFront : MonoBehaviour
    {
        void Start()
        {
            transform.SetAsLastSibling();
        }
    }
}