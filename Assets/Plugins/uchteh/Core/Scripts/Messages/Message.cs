using System;

namespace Core.Settings
{
	[Serializable]
	public class Message
	{

		public object[] Content { get; private set; }

		public dynamic Type { get; private set; }

		public Message(dynamic enumValue, params object[] content)
		{
			if (!(enumValue is Enum))
			{
				throw new Exception("Для сообщений необходимо перечисление");
			}

			Type = enumValue;
			Content = content;
		}
	}
}