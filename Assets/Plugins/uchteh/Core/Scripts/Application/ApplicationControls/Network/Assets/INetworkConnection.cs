namespace Core.Global.Network
{
	public interface INetworkConnection : IInitialize, ICanInstanse
	{
		void CheckConnection();

	}
}