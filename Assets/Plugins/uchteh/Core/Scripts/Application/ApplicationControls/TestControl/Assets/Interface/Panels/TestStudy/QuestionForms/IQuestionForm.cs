namespace Core.Testing
{
	public interface IQuestionForm : IHideAndShowBehavior, ILoadable
	{
		int QuestionId { get; set; }

		bool CheckIfRight();

		PassedQuestion SetInLog();
	}
}