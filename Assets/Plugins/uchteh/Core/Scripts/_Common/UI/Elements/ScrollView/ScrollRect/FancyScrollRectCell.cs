using Core.Ui.FancyScrollView.Core;
using UnityEngine;

namespace Core.Ui.FancyScrollView
{
  
    public abstract class FancyScrollRectCell<TItemData, TContext> : FancyCell<TItemData, TContext>
        where TContext : class, IFancyScrollRectContext, new()
    {

        public override void UpdatePosition(float position)
        {
            var (scrollSize, reuseMargin) = Context.CalculateScrollSize();

            var normalizedPosition = (Mathf.Lerp(0f, scrollSize, position) - reuseMargin) / (scrollSize - reuseMargin * 2f);

            var start = 0.5f * scrollSize;
            var end = -start;

            UpdatePosition(normalizedPosition, Mathf.Lerp(start, end, position));
        }

        protected virtual void UpdatePosition(float normalizedPosition, float localPosition)
        {
            transform.localPosition = Context.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(-localPosition, 0)
                : new Vector2(0, localPosition);
        }
    }

   
    public abstract class FancyScrollRectCell<TItemData> : FancyScrollRectCell<TItemData, FancyScrollRectContext>
    {
        public sealed override void SetContext(FancyScrollRectContext context) => base.SetContext(context);
    }
}
