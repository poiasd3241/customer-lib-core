using CustomerLibCore.Business.Validators;
using Xunit;

namespace CustomerLibCore.Business.Tests.Validators
{
	public class TextValidationHelperTest
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		public void ShouldConfirmEmptyOrWhitespaceText(string text)
		{
			Assert.True(TextValidationHelper.IsEmptyOrWhitespace(text));
		}

		[Theory]
		[InlineData(null)]
		[InlineData(" a")]
		[InlineData("a ")]
		[InlineData(" a ")]
		[InlineData("a")]
		public void ShouldConfirmNotEmptyNorWhitespaceText(string text)
		{
			Assert.False(TextValidationHelper.IsEmptyOrWhitespace(text));
		}
	}
}
