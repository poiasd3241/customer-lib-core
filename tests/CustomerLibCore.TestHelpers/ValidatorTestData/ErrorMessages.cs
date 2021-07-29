using CustomerLibCore.Business.Localization;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class ErrorMessages
	{
		public static (string expectedMessage, string confirmMessage)
			CollectionRequiredNotEmptyMsg() =>
				("required at least 1", ValidationErrorMessages.RequiredAtLeast(1));

		public static (string expectedMessage, string confirmMessage) RequiredMsg() =>
			("required", ValidationErrorMessages.REQUIRED);

		public static (string expectedMessage, string confirmMessage) EmptyOrWhitespaceMsg() =>
			("cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);

		public static (string expectedMessage, string confirmMessage)
			EmptyOrContainWhitespaceMsg() =>
			("cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);

		public static (string expectedMessage, string confirmMessage) NumberDecimalMsg() =>
			("must be a decimal point number", ValidationErrorMessages.NUMBER_DECIMAL);

		public static (string expectedMessage, string confirmMessage) NumberGreaterThan0Msg() =>
			("must be greater than 0", ValidationErrorMessages.NumberGreaterThan(0.ToString()));

		public static (string expectedMessage, string confirmMessage) UnknownTypeMsg() =>
			("unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
	}
}
