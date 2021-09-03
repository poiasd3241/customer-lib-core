using System;
using CustomerLibCore.Domain.Localization;

namespace CustomerLibCore.Domain.ArgumentCheckHelpers
{
	public class CheckNumber
	{
		/// <summary>
		/// If the value is not greater than the specified value to compare, 
		/// throws the <see cref="ArgumentException"/>; otherwise, does nothing.
		/// </summary>
		/// <param name="valueToCompare">The value that the value to check must be greater than.
		/// </param>
		/// <param name="value">The value to check.</param>
		/// <param name="paramName">The parameter name.</param>
		public static void GreaterThan(int valueToCompare, int value, string paramName)
		{
			if (value <= valueToCompare)
			{
				throw new ArgumentException(
					ErrorMessages.NumberGreaterThan(valueToCompare.ToString()), paramName);
			}
		}

		/// <summary>
		/// If the value is less than 1, throws the <see cref="ArgumentException"/>;
		/// otherwise, does nothing.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="paramName">The parameter name.</param>
		public static void Id(int value, string paramName)
		{
			if (value < 1)
			{
				throw new ArgumentException(ErrorMessages.ID, paramName);
			}
		}
	}
}
