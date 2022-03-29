using System;
using System.Linq;
using Core.Extensions;
using Core.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Gameplay.Inputing
{
	[RequireComponent(typeof(PlayerInput))]
	public abstract class PlayerInputControl : MonoBehaviour, IPlayerInputControl
	{
		private PlayerInput playerInput;
		public PlayerInput PlayerInput => playerInput;

		public GameObject GameObject => gameObject;

		public bool IsNewInstanse { get; set; }

		public bool IsLoaded { get; set; }

		public bool IsInitialized { get; set; }

		public virtual void Load()
		{
			playerInput = GetComponent<PlayerInput>();
			playerInput.neverAutoSwitchControlSchemes = true;

			AddDevices();

		}

		protected abstract void AddDevices();


		public virtual void Unload()
		{

		}

		public void Initialize()
		{

		}

		private void ChangeControlScheme(ControlScheme type)
		{
			if (PlayerInput.actions.controlSchemes.Any(x => x.name == type.ToString()))
			{
				PlayerInput.SwitchCurrentControlScheme(type.ToString());
			}
			else
			{
				this.DebugLogError("Не хватает " + type + " InputScheme в ассете " + PlayerInput.actions.name);
			}
		}


		public void Send(Type typeControlMessageFor, Message message)
		{

		}

		public void Notify(Message message)
		{
			switch (message.Type)
			{
				case Messages.InputControl.ActivateInputControls:
					PlayerInput.ActivateInput();
					break;
				case Messages.InputControl.DeactivateInputControls:
					PlayerInput.DeactivateInput();
					break;
				case Messages.InputControl.ChangeControlScheme:
					ChangeControlScheme((ControlScheme)message.Content[0]);
					break;
			}
		}
	}
}