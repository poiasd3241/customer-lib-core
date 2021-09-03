using CustomerLibCore.Domain.Localization;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class ErrorMessages
	{
		public static (string expectedMessage, string confirmMessage)
		CollectionNotEmptyMsg() =>
			("required at least 1", Domain.Localization.ErrorMessages.RequiredAtLeast(1));

		public static (string expectedMessage, string confirmMessage) RequiredMsg() =>
			("required", Domain.Localization.ErrorMessages.REQUIRED);

		public static (string expectedMessage, string confirmMessage) EmptyOrWhitespaceMsg() =>
			("cannot be empty or whitespace",
					Domain.Localization.ErrorMessages.TEXT_EMPTY_OR_WHITESPACE);

		public static (string expectedMessage, string confirmMessage)
		EmptyOrContainWhitespaceMsg() =>
			("cannot be empty or contain whitespace",
			Domain.Localization.ErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);

		public static (string expectedMessage, string confirmMessage) NumberDecimalMsg() =>
			("must be a decimal point number", Domain.Localization.ErrorMessages.NUMBER_DECIMAL);

		public static (string expectedMessage, string confirmMessage) NumberGreaterThan0Msg() =>
			("must be greater than 0", Domain.Localization.ErrorMessages.NumberGreaterThan(0.ToString()));

		public static (string expectedMessage, string confirmMessage) UnknownTypeMsg() =>
			("unknown type", Domain.Localization.ErrorMessages.ENUM_TYPE_UNKNOWN);

		public static (string expectedMessage, string confirmMessage) PhoneNumberE164Msg() =>
			("must be in E.164 format", Domain.Localization.ErrorMessages.PHONE_NUMBER_FORMAT_E164);

		public static (string expectedMessage, string confirmMessage) EmailFormatMsg() =>
			("invalid email format", Domain.Localization.ErrorMessages.EMAIL_FORMAT);
	}
}
