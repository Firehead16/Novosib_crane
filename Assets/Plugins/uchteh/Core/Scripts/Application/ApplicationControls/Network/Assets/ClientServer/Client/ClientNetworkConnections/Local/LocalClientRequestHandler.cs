namespace Core.Global.Network
{
	public abstract class LocalClientRequestHandler : BaseControlMethods, ILocalClientRequestHandler
	{
		protected LocalTrowServerNetworkConnection LocalConnection;

		public override void Load()
		{
			LocalConnection = FindObjectOfType<LocalTrowServerNetworkConnection>();
		}

		public override void Unload()
		{
			LocalConnection = null;
		}
	}
}