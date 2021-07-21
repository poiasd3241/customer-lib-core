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
		public static void ValidId(int value, string paramName)
		{
			if (value < 1)
			{
				throw new RouteArgumentException("ID cannot be less than 1", paramName);
			}
		}
	}
}
