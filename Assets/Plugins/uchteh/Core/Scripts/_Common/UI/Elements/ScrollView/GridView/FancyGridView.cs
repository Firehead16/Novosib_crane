using Core.Ui.FancyScrollView.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Ui.FancyScrollView
{
	public abstract class FancyGridView<TItemData, TContext> : FancyScrollRect<TItemData[], TContext>
        where TContext : class, IFancyGridViewContext, new()
    {
    
        protected abstract class DefaultCellGroup : FancyCellGroup<TItemData, TContext> { }

    
        [SerializeField] 
        protected float startAxisSpacing = 0f;

      
        [SerializeField]
        protected int startAxisCellCount = 4;

      
        [SerializeField]
        protected Vector2 cellSize = new Vector2(100f, 100f);

      
        protected sealed override GameObject CellPrefab => cellGroupTemplate;

        protected override float CellSize => Scroller.ScrollDirection == ScrollDirection.Horizontal
            ? cellSize.x
            : cellSize.y;


        public int DataCount { get; private set; }

        GameObject cellGroupTemplate;

        protected override void Initialize()
        {
            base.Initialize();

            Debug.Assert(startAxisCellCount > 0);

            Context.ScrollDirection = Scroller.ScrollDirection;
            Context.GetGroupCount = () => startAxisCellCount;
            Context.GetStartAxisSpacing = () => startAxisSpacing;
            Context.GetCellSize = () => Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? cellSize.y
                : cellSize.x;

            SetupCellTemplate();
        }


        protected abstract void SetupCellTemplate();

        protected virtual void Setup<TGroup>(FancyCell<TItemData, TContext> cellTemplate)
            where TGroup : FancyCell<TItemData[], TContext>
        {
            Context.CellTemplate = cellTemplate.gameObject;

            cellGroupTemplate = new GameObject("Group").AddComponent<TGroup>().gameObject;
            cellGroupTemplate.transform.SetParent(cellContainer, false);
            cellGroupTemplate.SetActive(false);
        }

    
        public virtual void UpdateContents(IList<TItemData> items)
        {
            DataCount = items.Count;

            var itemGroups = items
                .Select((item, index) => (item, index))
                .GroupBy(
                    x => x.index / startAxisCellCount,
                    x => x.item)
                .Select(group => group.ToArray())
                .ToArray();

            UpdateContents(itemGroups);
        }

      
        protected override void JumpTo(int itemIndex, float alignment = 0.5f)
        {
            var groupIndex = itemIndex / startAxisCellCount;
            base.JumpTo(groupIndex, alignment);
        }

    
        protected override void ScrollTo(int itemIndex, float duration, float alignment = 0.5f, Action onComplete = null)
        {
            var groupIndex = itemIndex / startAxisCellCount;
            base.ScrollTo(groupIndex, duration, alignment, onComplete);
        }

   
        protected override void ScrollTo(int itemIndex, float duration, Ease easing, float alignment = 0.5f, Action onComplete = null)
        {
            var groupIndex = itemIndex / startAxisCellCount;
            base.ScrollTo(groupIndex, duration, easing, alignment, onComplete);
        }
    }


    public abstract class FancyGridView<TItemData> : FancyGridView<TItemData, FancyGridViewContext> { }
}