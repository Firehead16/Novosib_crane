using System;

namespace Core.Ui.FancyScrollView
{
  
    public class FancyScrollRectContext : IFancyScrollRectContext
    {
        ScrollDirection IFancyScrollRectContext.ScrollDirection { get; set; }
        Func<(float ScrollSize, float ReuseMargin)> IFancyScrollRectContext.CalculateScrollSize { get; set; }
    }
}