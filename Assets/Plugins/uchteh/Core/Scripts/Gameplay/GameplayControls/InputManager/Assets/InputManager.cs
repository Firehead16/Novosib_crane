using System.Collections.Generic;
using Core.Settings;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum InputControl
		{
			ChangeControlScheme,
			DeactivateInputControls,
			ActivateInputControls,
		}
	}
}

namespace Core.Gameplay.Inputing
{
	public enum ControlScheme
	{
		Keyboard,
		Vr
	}

	public class InputManager : InitableManager<IPlayerInputControl>, IGameplayInput
	{
		private PlayerInputManager playerInputManager;

		[ShowInInspector,ReadOnly]
		private ICollection<IPlayerInputControl> PlayerInputControls => Controls?.Values;

		public bool IsInitialized { get; set; }
		[SerializeField] private ControlScheme defaultScheme = default;

		protected override IReadOnlyCollection<IPlayerInputControl> GetChildControls()
		{
			return FindObjectsOfType<PlayerInputControl>();
		}

	
		protected override void OnItemsInitialised()
		{
			ChangeControlScheme(defaultScheme);
		}

		private void ActivateInputControls()
		{
			foreach (var playerInputControl in PlayerInputControls)
			{
				playerInputControl.Notify(new Message(Messages.InputControl.ActivateInputControls));
			}
		}


		private void DeactivateInputControls()
		{
			foreach (var playerInputControl in PlayerInputControls)
			{
				playerInputControl.Notify(new Message(Messages.InputControl.DeactivateInputControls));
			}
		}

		[Button]
		public void ChangeControlScheme(ControlScheme type)
		{
			foreach (var playerInputControl in PlayerInputControls)
			{
				playerInputControl.Notify(new Message(Messages.InputControl.ChangeControlScheme, type));
			}
		}
	}
}
