using System.Collections.Generic;
using Core.Settings;


namespace Core.Gameplay.Interface
{
	public sealed class InterfaceManager : InitableManagerWithSettins<IInterfaceControl, InterfaceControlsSettings>, IGameplayInterface
	{
		protected override void OnItemsInitialised()
		{
		}

		private IEnumerable<IInterfaceControl> InterfaceControls => Controls.Values;


		/// <summary>
		/// Уведомить контроллеры
		/// </summary>
		/// <param name="message"></param>
		public override void Notify(Message message)
		{
			foreach (var interfaceControl in InterfaceControls)
			{
				interfaceControl.Notify(message);
			}
		}
	} 
}

