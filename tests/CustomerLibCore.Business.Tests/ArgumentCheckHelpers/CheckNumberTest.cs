using System;
using CustomerLibCore.Business.ArgumentCheckHelpers;
using Xunit;

namespace CustomerLibCore.Business.Tests.ArgumentCheckHelpers
{
	public class CheckNumberTest
	{
		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		public void ShouldNotThrowWhenIntNumberNotLessThan(int minValue, int value)
		{
			var paramName = "whatever";

			CheckNumber.NotLessThan(minValue, value, paramName);
		}

		[Fact]
		public void ShouldThrowWhenIntNumberFailsNotLessThan()
		{
			var paramName = "whatever";
			var minValue = 0;
			var value = -1;

			var exception = Assert.Throws<ArgumentException>(() =>
				CheckNumber.NotLessThan(minValue, value, paramName));

			Assert.Equal(paramName, exception.ParamName);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldNotThrowWhenValidId(int value)
		{
			var paramName = "whatever";

			CheckNumber.ValidId(value, paramName);
		}

		[Fact]
		public void ShouldThrowWhenNotValidId()
		{
			var paramName = "whatever";
			var value = 0;

			var exception = Assert.Throws<ArgumentException>(() =>
				CheckNumber.ValidId(value, paramName));

			Assert.Equal(paramName, exception.ParamName);
		}
	}
}
