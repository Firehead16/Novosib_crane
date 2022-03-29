using UnityEngine.InputSystem;

namespace Core.Gameplay.Inputing
{
	public interface IPlayerInputControl : IGameplayControl
	{
		PlayerInput PlayerInput { get; }
	}
}