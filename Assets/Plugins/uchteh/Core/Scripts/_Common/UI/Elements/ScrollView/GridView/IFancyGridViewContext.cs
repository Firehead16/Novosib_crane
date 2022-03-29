using System;

namespace Core.Ui.FancyScrollView
{

    public interface IFancyGridViewContext : IFancyScrollRectContext, IFancyCellGroupContext
    {
        Func<float> GetStartAxisSpacing { get; set; }
        Func<float> GetCellSize { get; set ; }
    }
}