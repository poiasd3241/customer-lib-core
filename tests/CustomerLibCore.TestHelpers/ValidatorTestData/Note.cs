using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
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
					("max 1000 characters", Domain.Localization.ErrorMessages.TextMaxLength(1000)));
			}
		}
	}
}
