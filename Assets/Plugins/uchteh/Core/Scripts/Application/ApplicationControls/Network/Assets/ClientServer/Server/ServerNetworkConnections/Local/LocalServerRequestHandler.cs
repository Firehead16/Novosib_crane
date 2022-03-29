namespace Core.Global.Network
{
	public abstract class LocalServerRequestHandler : BaseControlMethods, ILocalServerRequestHandler
	{
		protected LocalNetworkBroadcastConnection LocalConnection;

		public abstract Request GetHandlerResponse(Request request);
		
		public override void Load()
		{
			LocalConnection = FindObjectOfType<LocalNetworkBroadcastConnection>();
		}

		public override void Unload()
		{
			LocalConnection = null;
		}
	}
}