using System.Linq;
using UnityEngine;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample01
{
    class Example01 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = Enumerable.Range(0, 20)
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);
        }
    }
}
