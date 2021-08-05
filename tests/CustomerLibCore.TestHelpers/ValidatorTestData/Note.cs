using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;
using Xunit;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class Note
	{
		public class Content : TheoryData<string, (string, string)>
		{
			public Content()
			{
				this.AddRequiredData();
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 1001),
					("max 1000 characters", ValidationErrorMessages.TextMaxLength(1000)));
			}
		}
	}
}
