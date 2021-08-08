using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class Address
	{
		public class Line : TheoryData<string, (string, string)>
		{
			public Line()
			{
				this.AddRequiredData();
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 101),
					("max 100 characters", ValidationErrorMessages.TextMaxLength(100)));
			}
		}

		public class Line2 : TheoryData<string, (string, string)>
		{
			public Line2()
			{
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 101),
					("max 100 characters", ValidationErrorMessages.TextMaxLength(100)));
			}
		}

		public class TypeText : TheoryData<string, (string, string)>
		{
			public TypeText()
			{
				this.AddRequiredData();
				this.AddUnknownEnumTypeByNameData<AddressType>(
					new[] { "whatever", 3.ToString() });
			}
		}

		public class TypeEnum : TheoryData<AddressType, (string, string)>
		{
			public TypeEnum()
			{
				Add(0, UnknownTypeMsg());
				Add((AddressType)3, UnknownTypeMsg());
				Add((AddressType)666, UnknownTypeMsg());
			}
		}

		public class City : TheoryData<string, (string, string)>
		{
			public City()
			{
				this.AddRequiredData();
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 51),
					("max 50 characters", ValidationErrorMessages.TextMaxLength(50)));
			}
		}

		public class PostalCode : TheoryData<string, (string, string)>
		{
			public PostalCode()
			{
				this.AddRequiredData();
				this.AddEmptyOrContainWhitespaceData();
				Add(new('a', 7),
					("max 6 characters", ValidationErrorMessages.TextMaxLength(6)));
			}
		}

		public class State : TheoryData<string, (string, string)>
		{
			public State()
			{
				this.AddRequiredData();
				this.AddEmptyOrWhitespaceData();
				Add(new('a', 21),
					("max 20 characters", ValidationErrorMessages.TextMaxLength(20)));
			}
		}

		public class Country : TheoryData<string, (string, string)>
		{
			public Country()
			{
				this.AddRequiredData();
				this.AddEmptyOrWhitespaceData();
				Add("Japan",
					("allowed only 'United States', 'Canada'",
						ValidationErrorMessages.TextAllowedValues(new[]
						{ "United States", "Canada" })));
			}
		}
	}
}
