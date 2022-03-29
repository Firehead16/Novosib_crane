using System;
using System.Collections;
using UnityEngine;

namespace Core.Ui
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupHideAndShowBehavior : MonoBehaviour, IHideAndShowBehavior
    {
        private CanvasGroup canvasGroup;
        private Coroutine curCoroutine;

        public event Action OnShow;
        public event Action OnHide;

        private static float Duration => SettingsStorage.ThemeSettings.PanelShowAndHideDuration;

        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(bool isTimeShow)
        {
	        SetVisibility(true);
            OnShow?.Invoke();

	        if (isTimeShow)
	        {
		        StartCoroutine(Wait());
	        }
        }

        public virtual void Hide()
        {
            SetVisibility(false);
            OnHide?.Invoke();
        }


        private IEnumerator Wait()
        {
	        var t = 0f;
	        while (t < Duration)
	        {
		        if (t > Duration) t = Duration;
		        else t += Time.deltaTime;
		        yield return new WaitForSecondsRealtime(Time.deltaTime);
	        }

            Hide();
        }

        private void SetVisibility(bool isFullAlpha)
        {
            var newAlpha = isFullAlpha ? 1f : 0f;
            canvasGroup.alpha = newAlpha;
            gameObject.SetActive(isFullAlpha);
        }
    }
}