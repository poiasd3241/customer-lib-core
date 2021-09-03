using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Domain.Localization;

namespace CustomerLibCore.Api.Controllers
{
	/// <summary>
	/// Contains checks for route and query arguments.
	/// </summary>
	public class CheckUrlArgument
	{
		/// <summary>
		/// If the value is less than 1, throws the <see cref="RouteArgumentException"/>;
		/// otherwise, does nothing.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="paramName">The parameter name.</param>
		public static void Id(int value, string paramName)
		{
			if (value < 1)
			{
				throw new RouteArgumentException(ErrorMessages.ID, paramName);
			}
		}

		/// <summary> 
		/// If the value is not 0 or 1, throws the <see cref="QueryArgumentException"/>;
		/// <br/>
		/// otherwise, returns the boolean representation of the flag:
		/// <br/>
		/// 0 -> <see langword="false"/>;
		/// <br/>
		/// 1 -> <see langword="true"/>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="paramName">The parameter name.</param>
		public static bool Flag(int value, string paramName)
		{
			if (value is 0)
			{
				return false;
			}

			if (value is 1)
			{
				return true;
			}

			throw new QueryArgumentException(ErrorMessages.INT_FLAG, paramName);
		}
	}
}
