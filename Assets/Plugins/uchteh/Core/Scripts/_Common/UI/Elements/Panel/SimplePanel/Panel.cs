using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	/// <summary>
	/// Для каждой панели:
	/// * Элементы панели (кнопки, поля ввода) должны быть приватными и сериализуемыми
	/// * Каждому элементу должен соответствовать публичный объект (для кнопки - Action, для поля ввода или текста - свойство) 
	/// </summary>
	[RequireComponent(typeof(Image))]
    public class Panel : BaseControlMethods, IPanel
    {
        private IHideAndShowBehavior hideAndShowBehaviorScript;

        [SerializeField, BoxGroup("Параметры инициализации")] private bool isInitialized;
        public bool IsInitialized { get => isInitialized; set => isInitialized = value; }

        public event Action OnMenuShow;
        public event Action OnMenuHide;

        /// <summary>
        /// Родительский компонент
        /// </summary>
        public Transform PanelSaddle;

        /// <summary>
        /// Родительская панель
        /// </summary>
        public Panel ParentPanel;

        /// <summary>
        /// Необходимо спрятать при инициализации
        /// </summary>
        public bool HidenByDefault = false;

        /// <summary>
        /// При включении устанавливать последней в списке панелей
        /// </summary>
        public bool MovableSibling = false;


        /// <summary>
        /// Load панели
        /// </summary>
        public override void Load()
        {
            base.Load();
            hideAndShowBehaviorScript = GetComponent<IHideAndShowBehavior>();
            if (HidenByDefault)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Unload()
        {
         base.Unload();  
            Destroy(gameObject);
        }

        /// <summary>
        /// Установить родительский компонент
        /// </summary>
        protected virtual void SetSaddle()
        {
            if (PanelSaddle)
            {
                transform.SetParent(PanelSaddle);
                transform.CopyPositionAndRotation(PanelSaddle);
            }
        }

        /// <summary>
        /// Показать меню
        /// </summary>
        [Button]
        public virtual void Show(bool isTimeShow = false)
        {
	        hideAndShowBehaviorScript?.Show(isTimeShow);
            if (MovableSibling) transform.SetAsLastSibling();
            OnMenuShow?.Invoke();
        }

        /// <summary>
        /// Скрыть меню
        /// </summary>
        [Button]
        public virtual void Hide()
        {
	        hideAndShowBehaviorScript?.Hide();
            if (MovableSibling) transform.SetAsFirstSibling();
            OnMenuHide?.Invoke();

        }

        /// <summary>
        /// Убрать меню
        /// </summary>
        public virtual void RemovePanel()
        {
            GetComponent<RectTransform>().position = 2 * Screen.safeArea.max;
        }
    }
}