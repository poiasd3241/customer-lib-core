using System;
using CustomerLibCore.Domain.ArgumentCheckHelpers;
using CustomerLibCore.Domain.Localization;
using Xunit;

namespace CustomerLibCore.Domain.Tests.ArgumentCheckHelpers
{
	public class CheckNumberTest
	{
		[Theory]
		[InlineData(0, 1)]
		[InlineData(0, 2)]
		[InlineData(1, 2)]
		[InlineData(2, 5)]
		public void ShouldNotThrowWhenIntNumberGreaterThan(int valueToCompare, int value)
		{
			var paramName = "whatever";

			CheckNumber.GreaterThan(valueToCompare, value, paramName);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(1, 0)]
		[InlineData(2, 0)]
		[InlineData(1, 1)]
		[InlineData(2, 1)]
		[InlineData(5, 2)]
		public void ShouldThrowWhenIntNumberFailsGreaterThan(int valueToCompare, int value)
		{
			var paramName = "whatever";

			var ex = Assert.Throws<ArgumentException>(() =>
				CheckNumber.GreaterThan(valueToCompare, value, paramName));

			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.NumberGreaterThan(valueToCompare.ToString()) +
				$" (Parameter '{paramName}')", ex.Message);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		public void ShouldNotThrowWhenValidId(int value)
		{
			var paramName = "whatever";

			CheckNumber.Id(value, paramName);
		}

		[Theory]
		[InlineData(-5)]
		[InlineData(-1)]
		[InlineData(0)]
		public void ShouldThrowWhenNotValidId(int value)
		{
			var paramName = "whatever";

			var ex = Assert.Throws<ArgumentException>(() => CheckNumber.Id(value, paramName));

			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.ID + $" (Parameter '{paramName}')", ex.Message);
		}
	}
}
