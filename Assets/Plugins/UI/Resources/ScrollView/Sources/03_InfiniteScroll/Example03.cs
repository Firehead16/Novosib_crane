using System.Linq;
using UnityEngine;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample03
{
    class Example03 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = Enumerable.Range(0, 20)
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
