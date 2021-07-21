using System;

namespace CustomerLibCore.Business.ArgumentCheckHelpers
{
	public class CheckNumber
	{
		/// <summary>
		/// If the value is less than the specified minimum value, 
		/// throws the <see cref="ArgumentException"/>; otherwise, does nothing.
		/// </summary>
		/// <param name="minValue">The minimum value that the value to check must not exceed.
		/// </param>
		/// <param name="value">The value to check.</param>
		/// <param name="paramName">The parameter name.</param>
		public static void NotLessThan(int minValue, int value, string paramName)
		{
			if (value < minValue)
			{
				throw new ArgumentException($"Cannot be less than {minValue}", paramName);
			}
		}

		/// <summary>
		/// If the value is less than 1, throws the <see cref="ArgumentException"/>;
		/// otherwise, does nothing.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="paramName">The parameter name.</param>
		public static void ValidId(int value, string paramName)
		{
			if (value < 1)
			{
				throw new ArgumentException("ID cannot be less than 1", paramName);
			}
		}
	}
}
