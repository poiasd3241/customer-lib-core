using CustomerLibCore.Api.Exceptions;
using Xunit;

namespace CustomerLibCore.Api.Tests.Exceptions
{
	public class RouteArgumentExceptionTest
	{
		[Fact]
		public void ShouldCreateRouteArgumentException()
		{
			var message = "myMsg";
			var paramName = "myParam";

			var ex = new RouteArgumentException(message, paramName);

			Assert.Equal(message, ex.Message);
			Assert.Equal(paramName, ex.ParamName);
		}
	}
}
