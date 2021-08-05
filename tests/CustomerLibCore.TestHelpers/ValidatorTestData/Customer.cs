using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;
using Xunit;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class Customer
	{
		public class FirstName : TheoryData<string, (string, string)>
		{
			public FirstName()
			{
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 51),
					("max 50 characters", ValidationErrorMessages.TextMaxLength(50)));
			}
		}

		public class LastName : TheoryData<string, (string, string)>
		{
			public LastName()
			{
				this.AddRequiredData();
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 51),
					("max 50 characters", ValidationErrorMessages.TextMaxLength(50)));
			}
		}

		public class PhoneNumber : TheoryData<string, (string, string)>
		{
			public PhoneNumber()
			{
				this.AddEmptyOrContainWhitespaceData();
				Add("123456",
					("must be in E.164 format",
					ValidationErrorMessages.PHONE_NUMBER_FORMAT_E164));
			}
		}

		public class Email : TheoryData<string, (string, string)>
		{
			public Email()
			{
				this.AddEmptyOrContainWhitespaceData();
				Add("a@a@a", ("invalid email format", ValidationErrorMessages.EMAIL_FORMAT));
			}
		}

		public class TotalPurchasesAmount : TheoryData<string, (string, string)>
		{
			public TotalPurchasesAmount()
			{
				this.AddEmptyOrContainWhitespaceData();
				Add(" 1 ", EmptyOrContainWhitespaceMsg());
				Add("1.1.1", NumberDecimalMsg());
				Add("whatever", NumberDecimalMsg());
			}
		}
	}
}
