using System;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample07
{
    class Context : FancyScrollRectContext
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}
