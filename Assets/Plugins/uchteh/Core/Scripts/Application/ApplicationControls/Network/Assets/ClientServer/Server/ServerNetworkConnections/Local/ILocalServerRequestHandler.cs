namespace Core.Global.Network
{
	/// <summary>
	/// Обработчик запросов клиента через локальную сеть
	/// </summary>
	public interface ILocalServerRequestHandler : ILocalRequestHandler
	{
		Request GetHandlerResponse(Request request);
	}
}