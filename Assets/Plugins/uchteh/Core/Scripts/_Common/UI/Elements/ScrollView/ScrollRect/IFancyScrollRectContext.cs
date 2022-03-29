using System;

namespace Core.Ui.FancyScrollView
{
	public interface IFancyScrollRectContext
    {
        ScrollDirection ScrollDirection { get; set; }

        Func<(float ScrollSize, float ReuseMargin)> CalculateScrollSize { get; set; }
    }
}