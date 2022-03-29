using System;

namespace Core.Gameplay.Modul
{

	public interface IGameplayModul : IGameplayControl, IHasSaddle
	{
		string GetSaddleId();
		Type GetOrganEnum();
		void Doinitializer(Initializer initializerInitializer);
	} 
}