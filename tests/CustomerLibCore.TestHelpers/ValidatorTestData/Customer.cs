using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;

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
				// No + prefix
				Add("123456", PhoneNumberE164Msg());
				// 0 after +
				Add("+0123456", PhoneNumberE164Msg());
				// > 15 digits
				Add("+1234567890123456", PhoneNumberE164Msg());
				// Bad characters
				Add("+1-23456", PhoneNumberE164Msg());
				Add("+123-456-789", PhoneNumberE164Msg());
				Add("+123.456.789", PhoneNumberE164Msg());
				Add("+123(456)789", PhoneNumberE164Msg());
				Add("+1-234-567-89-01", PhoneNumberE164Msg());
				Add("+1(234)567-89-01", PhoneNumberE164Msg());
			}
		}

		public class Email : TheoryData<string, (string, string)>
		{
			public Email()
			{
				this.AddEmptyOrContainWhitespaceData();
				Add("@b.c", EmailFormatMsg());
				Add("@.c", EmailFormatMsg());
				Add("@c", EmailFormatMsg());
				Add("@", EmailFormatMsg());
				Add("a", EmailFormatMsg());
				Add("a.", EmailFormatMsg());
				Add("a.b", EmailFormatMsg());
				Add(".b", EmailFormatMsg());
				Add("a@b@c", EmailFormatMsg());
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
