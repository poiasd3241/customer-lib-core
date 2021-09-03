using CustomerLibCore.Api.Controllers;
using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Domain.Localization;
using Xunit;

namespace CustomerLibCore.Api.Tests.Controllers
{
	public class CheckUrlArgumentTest
	{
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		public void ShouldNotThrowWhenValidId(int value)
		{
			var paramName = "whatever";

			CheckUrlArgument.Id(value, paramName);
		}

		[Theory]
		[InlineData(-5)]
		[InlineData(-1)]
		[InlineData(0)]
		public void ShouldThrowWhenNotValidId(int value)
		{
			var paramName = "whatever";

			var ex = Assert.Throws<RouteArgumentException>(() =>
				CheckUrlArgument.Id(value, paramName));

			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.ID, ex.Message);
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		public void ShouldConvertIntFlagToBoolean(int value, bool result)
		{
			var paramName = "whatever";

			var actual = CheckUrlArgument.Flag(value, paramName);

			Assert.Equal(result, actual);
		}

		[Theory]
		[InlineData(-5)]
		[InlineData(-1)]
		[InlineData(2)]
		[InlineData(5)]
		public void ShouldThrowWhenNotValidIntFlag(int value)
		{
			var paramName = "whatever";

			var ex = Assert.Throws<QueryArgumentException>(() =>
				CheckUrlArgument.Flag(value, paramName));

			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.INT_FLAG, ex.Message);
		}
	}
}
