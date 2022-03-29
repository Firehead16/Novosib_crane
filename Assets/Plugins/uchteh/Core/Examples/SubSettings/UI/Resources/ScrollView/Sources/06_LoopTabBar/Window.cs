using UnityEngine;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample06
{
    class Window : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;

        public void In(MovementDirection direction) => transition?.In(direction);

        public void Out(MovementDirection direction) => transition?.Out(direction);
    }
}
