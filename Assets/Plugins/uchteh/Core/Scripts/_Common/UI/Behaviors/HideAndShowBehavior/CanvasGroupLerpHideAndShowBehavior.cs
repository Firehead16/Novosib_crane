using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Ui
{
	[RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupLerpHideAndShowBehavior : MonoBehaviour, IHideAndShowBehavior
    {
        private CanvasGroup canvasGroup;
        private Coroutine curCoroutine;

        private  float duration => SettingsStorage.ThemeSettings != null ? SettingsStorage.ThemeSettings.PanelShowAndHideDuration : 0;

        [SerializeField] private bool overrideDuration = false;
        [SerializeField,ShowIf("overrideDuration")] private float durationIn = 0.5f;
        [SerializeField, ShowIf("overrideDuration")]  private float durationOut=0.5f;


        private void OnEnable()
        {
	        canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(bool isTimeShow = false)
        {
            if (curCoroutine != null) StopCoroutine(curCoroutine);
            if (gameObject.activeInHierarchy)
            {
                curCoroutine = StartCoroutine(AlfaLerp(true));
            }
        }

        public virtual void Hide()
        {
            if (curCoroutine != null) StopCoroutine(curCoroutine);
            if (gameObject.activeInHierarchy)
            {
                curCoroutine = StartCoroutine(AlfaLerp(false));
            }
        }

        private IEnumerator AlfaLerp(bool isFullAlfa)
        {
	        if (canvasGroup)
	        {
		        var newAlfa = isFullAlfa ? 1f : 0f;
		        float  duration;
		        if (overrideDuration) duration = isFullAlfa ? durationIn : durationOut;
		        else duration = this.duration;
		        var oldAlfa = canvasGroup.alpha;
		        var t = 0f;
		        while (t < duration)
		        {
			        if (t > duration) t = duration;
			        else t += Time.deltaTime;
			        canvasGroup.alpha = Mathf.Lerp(oldAlfa, newAlfa, t / duration);
			        canvasGroup.blocksRaycasts = isFullAlfa;
			        yield return new WaitForSecondsRealtime(Time.deltaTime);
		        }
	        }
        }
    }
}