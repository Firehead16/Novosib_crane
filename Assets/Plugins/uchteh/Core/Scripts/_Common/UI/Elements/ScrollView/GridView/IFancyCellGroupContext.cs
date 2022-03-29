using System;
using UnityEngine;

namespace Core.Ui.FancyScrollView
{
	public interface IFancyCellGroupContext
    {
        GameObject CellTemplate { get; set; }
        Func<int> GetGroupCount { get; set; }
    }
}