using System;

namespace CustomerLibCore.Domain.Converters
{
	public class FlagConverter
	{
		public static bool FromInt(int value)
		{
			if (value is 1)
			{
				return true;
			}

			if (value is 0)
			{
				return false;
			}

			throw new ArgumentException("Int flag must be either 0 (false) or 1 (true)");
		}
	}
}
