using UnityEngine.InputSystem;

public interface IInputBindingControl : IApplicationControl, IInitialize
{
	InputActionAsset ReadInputAsset(InputActionAsset asset);
	void SaveInputAsset(InputActionAsset asset);

}