using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Ui.Extensions
{
    /// <summary>
    /// Includes a few fixes of my own, mainly to tidy up duplicates, remove unneeded stuff and testing. (nothing major, all the crew above did the hard work!)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Extensions/UI Window Base")]
    public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static bool ResetCoords = false;

        private bool isDragging = false;
        private Vector3 originalCoods = Vector3.zero;
        private Canvas currentCanvas;
        private RectTransform canvasRectTransform;

        [Tooltip("Number of pixels of the window that must stay inside the canvas view.")]
        public int KeepWindowInCanvas = 5;

        [Tooltip("The transform that is moved when dragging, can be left empty in which case its own transform is used.")]
        public RectTransform RootTransform = null;

        void Start()
        {
            if (RootTransform == null)
            {
                RootTransform = GetComponent<RectTransform>();
            }

            originalCoods = RootTransform.position;
            currentCanvas = GetComponentInParent<Canvas>();
            canvasRectTransform = currentCanvas.GetComponent<RectTransform>();
        }

        void Update()
        {
            if (ResetCoords)
                ResetCoordinatePosition();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                var delta = ScreenToCanvas(eventData.position) - ScreenToCanvas(eventData.position - eventData.delta);
                RootTransform.localPosition += delta;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            if (eventData.pointerCurrentRaycast.gameObject == null)
                return;

            if (eventData.pointerCurrentRaycast.gameObject.name == name)
            {
                isDragging = true;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
        }

        private void ResetCoordinatePosition()
        {
            RootTransform.position = originalCoods;
            ResetCoords = false;
        }

        private Vector3 ScreenToCanvas(Vector3 screenPosition)
        {
            Vector3 localPosition;
            Vector2 min;
            Vector2 max;
            var canvasSize = canvasRectTransform.sizeDelta;

            if (currentCanvas.renderMode == RenderMode.ScreenSpaceOverlay || (currentCanvas.renderMode == RenderMode.ScreenSpaceCamera && currentCanvas.worldCamera == null))
            {
                localPosition = screenPosition;

                min = Vector2.zero;
                max = canvasSize;
            }
            else
            {
                var ray = currentCanvas.worldCamera.ScreenPointToRay(screenPosition);
                var plane = new Plane(canvasRectTransform.forward, canvasRectTransform.position);

                float distance;
                if (plane.Raycast(ray, out distance) == false)
                {
                    throw new Exception("Is it practically possible?");
                };
                var worldPosition = ray.origin + ray.direction * distance;
                localPosition = canvasRectTransform.InverseTransformPoint(worldPosition);

                min = -Vector2.Scale(canvasSize, canvasRectTransform.pivot);
                max = Vector2.Scale(canvasSize, Vector2.one - canvasRectTransform.pivot);
            }

            // keep window inside canvas
            localPosition.x = Mathf.Clamp(localPosition.x, min.x + KeepWindowInCanvas, max.x - KeepWindowInCanvas);
            localPosition.y = Mathf.Clamp(localPosition.y, min.y + KeepWindowInCanvas, max.y - KeepWindowInCanvas);

            return localPosition;
        }
    }
}