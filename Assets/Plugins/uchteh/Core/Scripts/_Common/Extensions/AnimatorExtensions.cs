using UnityEngine;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение для аниматора
	/// </summary>
	public static class AnimatorExtensions
	{
		/// <summary>
		/// Прогрывается анимация в аниматоре
		/// </summary>
		/// <param name="animator">Аниматор</param>
		/// <param name="animationName">Название анимации</param>
		/// <returns></returns>
		public static bool IsAnimationPlaying(this Animator animator, string animationName)
		{
			// берем информацию о состоянии
			var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			// смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
			if (animatorStateInfo.IsName(animationName))
				return true;

			return false;
		}
	}

}