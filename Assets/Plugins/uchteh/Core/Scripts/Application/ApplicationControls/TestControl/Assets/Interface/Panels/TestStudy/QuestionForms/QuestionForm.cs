using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	public abstract class QuestionForm<TQuestionAnswer> : BaseManager<TQuestionAnswer>, IQuestionForm
		where TQuestionAnswer : class, IQuestionAnswer
	{
		public Question Question { get; private set; }

		public int QuestionId { get; set; }
		

		[SerializeField]
		protected Text Title;

		[SerializeField]
		protected RectTransform AnswerArea;

		protected bool IsAnswerRight;

		public void StartWork()
		{
			Load();
		}

		public override void Load()
		{
			Question = TestControl.GetQuestionById(QuestionId);
			base.Load();
		}


		/// <summary>
		/// Проверить правильность ответа на вопрос
		/// </summary>
		/// <returns></returns>
		public abstract bool CheckIfRight();
		
		/// <summary>
		/// Добавить вопрос в пройденные
		/// </summary>
		/// <returns></returns>
		public abstract PassedQuestion SetInLog();

		public void Show(bool isTimeShow = false)
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}