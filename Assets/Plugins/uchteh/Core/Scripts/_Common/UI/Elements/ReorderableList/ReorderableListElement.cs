using Core.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Ui.Extensions
{

    [RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
    public class ReorderableListElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Tooltip("Can this element be dragged?")]
        public bool IsGrabbable = true;
        [Tooltip("Can this element be transfered to another list")]
        public bool IsTransferable = true;
        [Tooltip("Can this element be dropped in space?")]
        public bool isDroppableInSpace = false;


        private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();
        private ReorderableList currentReorderableListRaycasted;

        private int fromIndex;

        private RectTransform draggingObject;
        private LayoutElement draggingObjectLe;
        private Vector2 draggingObjectOriginalSize;

        private RectTransform fakeElement;
        private LayoutElement fakeElementLe;

        private int displacedFromIndex;
        private RectTransform displacedObject;
        private LayoutElement displacedObjectLe;
        private Vector2 displacedObjectOriginalSize;
        private ReorderableList displacedObjectOriginList;

        private bool isDragging;
        private RectTransform rect;
        private ReorderableList reorderableList;
        private CanvasGroup canvasGroup;
        internal bool IsValid;
        
        #region IBeginDragHandler Members

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            IsValid = true;
            if (reorderableList == null)
                return;

            //Can't drag, return...
            if (!reorderableList.IsDraggable || !this.IsGrabbable)
            {
                draggingObject = null;
                return;
            }

            //If not CloneDraggedObject just set draggingObject to this gameobject
            if (reorderableList.CloneDraggedObject == false)
            {
                draggingObject = rect;
                fromIndex = rect.GetSiblingIndex();
                displacedFromIndex = -1;
                //Send OnElementRemoved Event
                reorderableList.OnElementRemoved?.Invoke(new ReorderableList.ReorderableListEventStruct
                {
	                DroppedObject = draggingObject.gameObject,
	                IsAClone = reorderableList.CloneDraggedObject,
	                SourceObject = reorderableList.CloneDraggedObject ? gameObject : draggingObject.gameObject,
	                FromList = reorderableList,
	                FromIndex = fromIndex,
                });
                if (IsValid == false)
                {
                    draggingObject = null;
                    return;
                }
            }
            else
            {
                //Else Duplicate
                GameObject clone = (GameObject)Instantiate(gameObject);
                draggingObject = clone.GetComponent<RectTransform>();
            }

            //Put _dragging object into the dragging area
            draggingObjectOriginalSize = gameObject.GetComponent<RectTransform>().rect.size;
            draggingObjectLe = draggingObject.GetComponent<LayoutElement>();
            draggingObject.SetParent(reorderableList.DraggableArea, true);
            draggingObject.SetAsLastSibling();
            reorderableList.Refresh();

            //Create a fake element for previewing placement
            fakeElement = new GameObject("Fake").AddComponent<RectTransform>();
            fakeElementLe = fakeElement.gameObject.AddComponent<LayoutElement>();

            RefreshSizes();

            //Send OnElementGrabbed Event
            if (reorderableList.OnElementGrabbed != null)
            {
                reorderableList.OnElementGrabbed.Invoke(new ReorderableList.ReorderableListEventStruct
                    {
                        DroppedObject = draggingObject.gameObject,
                        IsAClone = reorderableList.CloneDraggedObject,
                        SourceObject = reorderableList.CloneDraggedObject ? gameObject : draggingObject.gameObject,
                        FromList = reorderableList,
                        FromIndex = fromIndex,
                    });

                if (!IsValid)
                {
                    CancelDrag();
                    return;
                }
            }

            isDragging = true;
        }

        #endregion


        #region IDragHandler Members

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging)
                return;
            if (!IsValid)
            {
                CancelDrag();
                return;
            }
            //Set dragging object on cursor
            var canvas = draggingObject.GetComponentInParent<Canvas>();
            
            Vector3 worldPoint;
            
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position,
                canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null, out worldPoint);
            draggingObject.position = worldPoint;

            ReorderableList oldReorderableListRaycasted = currentReorderableListRaycasted;

            //Check everything under the cursor to find a ReorderableList
            EventSystem.current.RaycastAll(eventData, raycastResults);
            for (int i = 0; i < raycastResults.Count; i++)
            {
                currentReorderableListRaycasted = raycastResults[i].gameObject.GetComponent<ReorderableList>();
                if (currentReorderableListRaycasted != null)
                {
                    break;
                }
            }

            //If nothing found or the list is not dropable, put the fake element outside
            if (currentReorderableListRaycasted == null || currentReorderableListRaycasted.IsDropable == false
                || (oldReorderableListRaycasted != reorderableList && !IsTransferable)
                || ((fakeElement.parent == currentReorderableListRaycasted.Content 
                    ? currentReorderableListRaycasted.Content.childCount - 1 
                    : currentReorderableListRaycasted.Content.childCount) >= currentReorderableListRaycasted.maxItems && !currentReorderableListRaycasted.IsDisplacable)
                || currentReorderableListRaycasted.maxItems <= 0)
            {
                RefreshSizes();
                fakeElement.transform.SetParent(reorderableList.DraggableArea, false);
                // revert the displaced element when not hovering over its list
                if (displacedObject != null)
                {
                    RevertDisplacedElement();
                }
            }
            //Else find the best position on the list and put fake element on the right index 
            else
            {
                if (currentReorderableListRaycasted.Content.childCount < currentReorderableListRaycasted.maxItems && fakeElement.parent != currentReorderableListRaycasted.Content)
                {
                    fakeElement.SetParent(currentReorderableListRaycasted.Content, false);
                }

                float minDistance = float.PositiveInfinity;
                int targetIndex = 0;
                float dist = 0;
                for (int j = 0; j < currentReorderableListRaycasted.Content.childCount; j++)
                {
                    var c = currentReorderableListRaycasted.Content.GetChild(j).GetComponent<RectTransform>();

                    if (currentReorderableListRaycasted.ContentLayout is VerticalLayoutGroup)
                        dist = Mathf.Abs(c.position.y - worldPoint.y);
                    else if (currentReorderableListRaycasted.ContentLayout is HorizontalLayoutGroup)
                        dist = Mathf.Abs(c.position.x - worldPoint.x);
                    else if (currentReorderableListRaycasted.ContentLayout is GridLayoutGroup)
                        dist = (Mathf.Abs(c.position.x - worldPoint.x) + Mathf.Abs(c.position.y - worldPoint.y));

                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        targetIndex = j;
                    }
                }
                if ((currentReorderableListRaycasted != oldReorderableListRaycasted || targetIndex != displacedFromIndex)
                    && currentReorderableListRaycasted.Content.childCount == currentReorderableListRaycasted.maxItems)
                {
                    Transform toDisplace = currentReorderableListRaycasted.Content.GetChild(targetIndex);
                    if (displacedObject != null)
                    {
                        RevertDisplacedElement();
                        if (currentReorderableListRaycasted.Content.childCount > currentReorderableListRaycasted.maxItems)
                        {
                            DisplaceElement(targetIndex, toDisplace);
                        }
                    }
                    else if (fakeElement.parent != currentReorderableListRaycasted.Content)
                    {
                        fakeElement.SetParent(currentReorderableListRaycasted.Content, false);
                        DisplaceElement(targetIndex, toDisplace);
                    }
                }
                RefreshSizes();
                fakeElement.SetSiblingIndex(targetIndex);
                fakeElement.gameObject.SetActive(true);

            }
        }

        #endregion


        #region Displacement

        private void DisplaceElement(int targetIndex, Transform displaced)
        {
            displacedFromIndex = targetIndex;
            displacedObjectOriginList = currentReorderableListRaycasted;
            displacedObject = displaced.GetComponent<RectTransform>();
            displacedObjectLe = displacedObject.GetComponent<LayoutElement>();
            displacedObjectOriginalSize = displacedObject.rect.size;

            var args = new ReorderableList.ReorderableListEventStruct
            {
                DroppedObject = displacedObject.gameObject,
                FromList = currentReorderableListRaycasted,
                FromIndex = targetIndex,
            };


            int c = fakeElement.parent == reorderableList.Content 
                ? reorderableList.Content.childCount - 1 
                : reorderableList.Content.childCount;

            if (reorderableList.IsDropable && c < reorderableList.maxItems && displacedObject.GetComponent<ReorderableListElement>().IsTransferable)
            {
                displacedObjectLe.preferredWidth = draggingObjectOriginalSize.x;
                displacedObjectLe.preferredHeight = draggingObjectOriginalSize.y;
                displacedObject.SetParent(reorderableList.Content, false);
                displacedObject.rotation = reorderableList.transform.rotation;
                displacedObject.SetSiblingIndex(fromIndex);
                // Force refreshing both lists because otherwise we get inappropriate FromList in ReorderableListEventStruct 
                reorderableList.Refresh();
                currentReorderableListRaycasted.Refresh();

                args.ToList = reorderableList;
                args.ToIndex = fromIndex;
                reorderableList.OnElementDisplacedTo.Invoke(args);
                reorderableList.OnElementAdded.Invoke(args);
            }
            else if (displacedObject.GetComponent<ReorderableListElement>().isDroppableInSpace)
            {
                displacedObject.SetParent(currentReorderableListRaycasted.DraggableArea, true);
                currentReorderableListRaycasted.Refresh();
                displacedObject.position += new Vector3(draggingObjectOriginalSize.x / 2, draggingObjectOriginalSize.y / 2, 0);
            }
            else
            {
                displacedObject.SetParent(null, true);
                displacedObjectOriginList.Refresh();
                displacedObject.gameObject.SetActive(false);
            }
            displacedObjectOriginList.OnElementDisplacedFrom.Invoke(args);
            reorderableList.OnElementRemoved.Invoke(args);
        }

        private void RevertDisplacedElement()
        {
            var args = new ReorderableList.ReorderableListEventStruct
            {
                DroppedObject = displacedObject.gameObject,
                FromList = displacedObjectOriginList,
                FromIndex = displacedFromIndex,
            };
            if (displacedObject.parent != null)
            {
                args.ToList = reorderableList;
                args.ToIndex = fromIndex;
            }

            displacedObjectLe.preferredWidth = displacedObjectOriginalSize.x;
            displacedObjectLe.preferredHeight = displacedObjectOriginalSize.y;
            displacedObject.SetParent(displacedObjectOriginList.Content, false);
            displacedObject.rotation = displacedObjectOriginList.transform.rotation;
            displacedObject.SetSiblingIndex(displacedFromIndex);
            displacedObject.gameObject.SetActive(true);

            // Force refreshing both lists because otherwise we get inappropriate FromList in ReorderableListEventStruct 
            reorderableList.Refresh();
            displacedObjectOriginList.Refresh();

            if (args.ToList != null)
            {
                reorderableList.OnElementDisplacedToReturned.Invoke(args);
                reorderableList.OnElementRemoved.Invoke(args);
            }
            displacedObjectOriginList.OnElementDisplacedFromReturned.Invoke(args);
            displacedObjectOriginList.OnElementAdded.Invoke(args);

            displacedFromIndex = -1;
            displacedObjectOriginList = null;
            displacedObject = null;
            displacedObjectLe = null;

        }

        public void FinishDisplacingElement()
        {
            if (displacedObject.parent == null)
            {
                Destroy(displacedObject.gameObject);
            }
            displacedFromIndex = -1;
            displacedObjectOriginList = null;
            displacedObject = null;
            displacedObjectLe = null;
        }

        #endregion


        #region IEndDragHandler Members

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            if (draggingObject != null)
            {
                //If we have a ReorderableList that is dropable
                //Put the dragged object into the content and at the right index
                if (currentReorderableListRaycasted != null && fakeElement.parent == currentReorderableListRaycasted.Content)
                {
                    var args = new ReorderableList.ReorderableListEventStruct
                    {
                        DroppedObject = draggingObject.gameObject,
                        IsAClone = reorderableList.CloneDraggedObject,
                        SourceObject = reorderableList.CloneDraggedObject ? gameObject : draggingObject.gameObject,
                        FromList = reorderableList,
                        FromIndex = fromIndex,
                        ToList = currentReorderableListRaycasted,
                        ToIndex = fakeElement.GetSiblingIndex()
                    };
                   
                    //Send OnelementDropped Event
                    if (reorderableList && reorderableList.OnElementDropped != null)
                    {
                        reorderableList.OnElementDropped.Invoke(args);
                    }
                  
                    if (!IsValid)
                    {
                        CancelDrag();
                        return;
                    }
                   
                    RefreshSizes();
                   
                    draggingObject.SetParent(currentReorderableListRaycasted.Content, false);
                    draggingObject.rotation = currentReorderableListRaycasted.transform.rotation;
                    draggingObject.SetSiblingIndex(fakeElement.GetSiblingIndex());
                   
                    // Force refreshing both lists because otherwise we get inappropriate FromList in ReorderableListEventStruct 
                    reorderableList.Refresh();
                    currentReorderableListRaycasted.Refresh();

                    reorderableList.OnElementAdded.Invoke(args);


                    if (displacedObject != null)
                    {
                        FinishDisplacingElement();
                    }

                    if (!IsValid)
                        throw new Exception("It's too late to cancel the Transfer! Do so in OnElementDropped!");
                }
                
                else
                {
                    //We don't have an ReorderableList
                    if (this.isDroppableInSpace)
                    {
                        reorderableList.OnElementDropped.Invoke(new ReorderableList.ReorderableListEventStruct
                            {
                                DroppedObject = draggingObject.gameObject,
                                IsAClone = reorderableList.CloneDraggedObject,
                                SourceObject =
                                    reorderableList.CloneDraggedObject ? gameObject : draggingObject.gameObject,
                                FromList = reorderableList,
                                FromIndex = fromIndex
                            });
                    }
                    else
                    {
                        CancelDrag();
                    }
                    
                    //If there is no more room for the element in the target list, notify it (OnElementDroppedWithMaxItems event) 
                    if (currentReorderableListRaycasted != null)
                    {
                        if ((currentReorderableListRaycasted.Content.childCount >=
                             currentReorderableListRaycasted.maxItems &&
                             !currentReorderableListRaycasted.IsDisplacable)
                            || currentReorderableListRaycasted.maxItems <= 0)
                        {
                            GameObject o = draggingObject.gameObject;
                            reorderableList.OnElementDroppedWithMaxItems.Invoke(
                                new ReorderableList.ReorderableListEventStruct
                                {
                                    DroppedObject = o,
                                    IsAClone = reorderableList.CloneDraggedObject,
                                    SourceObject = reorderableList.CloneDraggedObject ? gameObject : o,
                                    FromList = reorderableList,
                                    ToList = currentReorderableListRaycasted,
                                    FromIndex = fromIndex
                                });
                        } 
                    }
                    
                }
            }

            //Delete fake element
            if (fakeElement != null)
            {
                Destroy(fakeElement.gameObject);
                fakeElement = null;
            }
            canvasGroup.blocksRaycasts = true;
        }

        #endregion


        void CancelDrag()
        {
            isDragging = false;
            //If it's a clone, delete it
            if (reorderableList.CloneDraggedObject)
            {
                Destroy(draggingObject.gameObject);
            }
            //Else replace the draggedObject to his first place
            else
            {
                RefreshSizes();
                draggingObject.SetParent(reorderableList.Content, false);
                draggingObject.rotation = reorderableList.Content.transform.rotation;
                draggingObject.SetSiblingIndex(fromIndex);


                var args = new ReorderableList.ReorderableListEventStruct
                {
                    DroppedObject = draggingObject.gameObject,
                    IsAClone = reorderableList.CloneDraggedObject,
                    SourceObject = reorderableList.CloneDraggedObject ? gameObject : draggingObject.gameObject,
                    FromList = reorderableList,
                    FromIndex = fromIndex,
                    ToList = reorderableList,
                    ToIndex = fromIndex
                };

                reorderableList.Refresh();

                reorderableList.OnElementAdded.Invoke(args);

                if (!IsValid)
                    throw new Exception("Transfer is already Canceled.");

            }

            //Delete fake element
            if (fakeElement != null)
            {
                Destroy(fakeElement.gameObject);
                fakeElement = null;
            }
            if (displacedObject != null)
            {
                RevertDisplacedElement();
            }
            canvasGroup.blocksRaycasts = true;
        }

        private void RefreshSizes()
        {
            Vector2 size = draggingObjectOriginalSize;

            if (currentReorderableListRaycasted != null
                && currentReorderableListRaycasted.IsDropable
                && currentReorderableListRaycasted.Content.childCount > 0
                && currentReorderableListRaycasted.EqualizeSizesOnDrag)
            {
                var firstChild = currentReorderableListRaycasted.Content.GetChild(0);
                if (firstChild != null)
                {
                    size = firstChild.GetComponent<RectTransform>().rect.size;
                }
            }

            draggingObject.sizeDelta = size;
            fakeElementLe.preferredHeight = draggingObjectLe.preferredHeight = size.y;
            fakeElementLe.preferredWidth = draggingObjectLe.preferredWidth = size.x;
            fakeElement.GetComponent<RectTransform>().sizeDelta = size;
        }

        public void Init(ReorderableList reorderableList)
        {
            this.reorderableList = reorderableList;
            rect = GetComponent<RectTransform>();
            canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        }
    }
}
