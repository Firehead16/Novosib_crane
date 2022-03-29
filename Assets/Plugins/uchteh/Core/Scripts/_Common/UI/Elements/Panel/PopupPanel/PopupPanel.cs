using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Ui
{
	public class PopupPanel : Panel, ISubscribe 
    {
        protected float Duration;
        protected float AlfaMin;
        protected float AlfaMax;

        protected Coroutine CurBehaviorCoroutine;

        /// <summary>
        /// Наведение на панель
        /// </summary>
        protected bool MouseOnPanel;

        private EventTrigger eventTrigger;


        public override void Initialize()
        {
            eventTrigger = GetComponent<EventTrigger>();

            if (HidenByDefault)
            {
                GetComponent<CanvasGroup>().alpha = 0;
            }


            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entry.callback.AddListener(data => MouseLeavePanel());
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener(data => MouseEnterOnPanel());
            eventTrigger.triggers.Add(entry);
        }

        public void Subscribe()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entry.callback.AddListener(data => MouseLeavePanel());
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener(data => MouseEnterOnPanel());
            eventTrigger.triggers.Add(entry);
        }

        public void UnSubscribe()
        {
          
        }



        public override void Show(bool isTimeShow = false)
        {
            if (CurBehaviorCoroutine != null) StopCoroutine(CurBehaviorCoroutine);
            if (gameObject.activeInHierarchy)
            {
                CurBehaviorCoroutine = StartCoroutine(AlfaLerp(1f));
            }
        }

        public override void Hide()
        {
            if (CurBehaviorCoroutine != null) StopCoroutine(CurBehaviorCoroutine);
            if (gameObject.activeInHierarchy)
            {
                CurBehaviorCoroutine = StartCoroutine(AlfaLerp(0f));
            }
        }

        protected virtual void MouseEnterOnPanel()
        {
            MouseOnPanel = true;
            if (CurBehaviorCoroutine != null) StopCoroutine(CurBehaviorCoroutine);
            CurBehaviorCoroutine = StartCoroutine(AlfaLerp(AlfaMax));
        }

        protected virtual void MouseLeavePanel()
        {
            MouseOnPanel = false;
            if (CurBehaviorCoroutine != null) StopCoroutine(CurBehaviorCoroutine);
            CurBehaviorCoroutine = StartCoroutine(AlfaLerp(AlfaMin));
        }

        private IEnumerator AlfaLerp(float newAlfa)
        {
            var oldAlfa = GetComponent<CanvasGroup>().alpha;
            var t = 0f;
            while (t < Duration)
            {
                if (t > Duration) t = Duration;
                else t += Time.deltaTime;
                GetComponent<CanvasGroup>().alpha = Mathf.Lerp(oldAlfa, newAlfa, t / Duration);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
