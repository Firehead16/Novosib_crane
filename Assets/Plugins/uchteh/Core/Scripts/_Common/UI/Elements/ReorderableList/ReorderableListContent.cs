using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Ui.Extensions
{
    public class ReorderableListContent : MonoBehaviour
    {
        private List<Transform> cachedChildren;
        private List<ReorderableListElement> cachedListElement;
        private ReorderableListElement ele;
        private ReorderableList extList;
        private RectTransform rect;

        private void OnEnable()
        {
            if(rect)StartCoroutine(RefreshChildren());
        }


        public void OnTransformChildrenChanged()
        {
            if(this.isActiveAndEnabled)StartCoroutine(RefreshChildren());
        }

        public void Init(ReorderableList extList)
        {
            this.extList = extList;
            rect = GetComponent<RectTransform>();
            cachedChildren = new List<Transform>();
            cachedListElement = new List<ReorderableListElement>();

            StartCoroutine(RefreshChildren());
        }

        private IEnumerator RefreshChildren()
        {
            //Handle new children
            for (int i = 0; i < rect.childCount; i++)
            {
                if (cachedChildren.Contains(rect.GetChild(i)))
                    continue;

                //Get or Create ReorderableListElement
                ele = rect.GetChild(i).gameObject.GetComponent<ReorderableListElement>() ??
                       rect.GetChild(i).gameObject.AddComponent<ReorderableListElement>();
                ele.Init(extList);

                cachedChildren.Add(rect.GetChild(i));
                cachedListElement.Add(ele);
            }

            //HACK a little hack, if I don't wait one frame I don't have the right deleted children
            yield return 0;

            //Remove deleted child
            for (int i = cachedChildren.Count - 1; i >= 0; i--)
            {
                if (cachedChildren[i] == null)
                {
                    cachedChildren.RemoveAt(i);
                    cachedListElement.RemoveAt(i);
                }
            }
        }
    }
}