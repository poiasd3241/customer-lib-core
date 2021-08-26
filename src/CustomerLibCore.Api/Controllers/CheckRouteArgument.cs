using CustomerLibCore.Api.Exceptions;

namespace CustomerLibCore.Api.Controllers
{
	public class CheckRouteArgument
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
				throw new RouteArgumentException("ID cannot be less than 1", paramName);
			}
		}

		/// <summary>
		/// If the value is not 0 or 1, throws the <see cref="RouteArgumentException"/>;
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

			throw new RouteArgumentException("Int flag must be either 0 (false) or 1 (true)",
				paramName);
		}
	}
}
