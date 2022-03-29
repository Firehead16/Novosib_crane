using Core.Ui.FancyScrollView.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.FancyScrollView.Examples.FancyScrollViewExample01
{
    class Cell : FancyCell<ItemData>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        float currentPosition;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
