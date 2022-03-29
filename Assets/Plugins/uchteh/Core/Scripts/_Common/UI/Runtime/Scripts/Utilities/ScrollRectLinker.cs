using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{
    [RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectLinker")]
    public class ScrollRectLinker : MonoBehaviour
    {

        public bool clamp = true;

        [SerializeField]
        ScrollRect controllingScrollRect = null;
        ScrollRect scrollRect = null;

        void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            if (controllingScrollRect != null)
                controllingScrollRect.onValueChanged.AddListener(MirrorPos);
        }

        void MirrorPos(Vector2 scrollPos)
        {

            if (clamp)
                scrollRect.normalizedPosition = new Vector2(Mathf.Clamp01(scrollPos.x), Mathf.Clamp01(scrollPos.y));
            else
                scrollRect.normalizedPosition = scrollPos;
        }

    }
}