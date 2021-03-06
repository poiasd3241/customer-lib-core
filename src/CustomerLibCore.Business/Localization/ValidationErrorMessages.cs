namespace CustomerLibCore.Business.Localization
{
	public static class ValidationErrorMessages
	{
		public const string REQUIRED = "required";

		public const string NUMBER_DECIMAL = "must be a decimal point number";

		public const string ENUM_TYPE_UNKNOWN = "unknown type";

		public const string TEXT_EMPTY_OR_WHITESPACE = "cannot be empty or whitespace";
		public const string TEXT_EMPTY_OR_CONTAIN_WHITESPACE =
			"cannot be empty or contain whitespace";

		public const string EMAIL_FORMAT = "invalid email format";

		public const string PHONE_NUMBER_FORMAT_E164 = "must be in E.164 format";

		public static string RequiredAtLeast(int minItemsCount) =>
			$"required at least {minItemsCount}";

		public static string TextMaxLength(int maxLength) =>
			$"max {maxLength} characters";

		public static string TextAllowedValues(string[] allowed) =>
			$"allowed only '{string.Join("', '", allowed)}'";
	}
}
