using System;

namespace Core.Extensions
{
	/// <summary>
	/// Расширение для перечислений
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Вспомогательный метод, который сообщит вам, что перечисляемое значение определено в данном перечислении 
		/// </summary>
		/// <param name="value">Значение</param>
		/// <returns></returns>
		public static bool IsDefined(this Enum value)
		{
			return Enum.IsDefined(value.GetType(), value);
		}
	} 
}

