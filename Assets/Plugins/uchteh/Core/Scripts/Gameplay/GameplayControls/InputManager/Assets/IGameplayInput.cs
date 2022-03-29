namespace Core.Gameplay.Inputing
{
	public interface IGameplayInput : IGameplayControl
	{
		void ChangeControlScheme(ControlScheme type);
	}
}