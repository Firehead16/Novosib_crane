using System;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample08
{
    class Context : FancyGridViewContext
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}
