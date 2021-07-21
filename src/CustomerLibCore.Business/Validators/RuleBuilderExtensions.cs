using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Business.Enums;
using CustomerLibCore.Business.Localization;
using FluentValidation;

namespace CustomerLibCore.Business.Validators
{
	/// <summary>
	/// Extensions for <see cref="IRuleBuilder{T, TProperty}"/>.
	/// </summary>
	public static class RuleBuilderExtensions
	{
		#region Private Members

		private static readonly int _customer_first_name_max_length = 50;
		private static readonly int _customer_last_name_max_length = 50;

		private static readonly int _address_line_max_length = 100;
		private static readonly int _address_line2_max_length = 100;
		private static readonly int _address_city_max_length = 50;
		private static readonly int _address_postalCode_max_length = 6;
		private static readonly int _address_state_max_length = 20;
		private static readonly string[] _address_country_allowed =
			new[] { "United States", "Canada" };

		private static readonly int _note_content_max_length = 1000;

		#endregion

		#region Single RuleBuilderOptions extensions

		public static IRuleBuilderOptions<T, string> PhoneNumberFormatE164<T>(
		this IRuleBuilder<T, string> ruleBuilder) =>
			ruleBuilder.Matches(@"^\+[1-9]\d{1,14}$");

		public static IRuleBuilderOptions<T, string> EmailFormat<T>(
			this IRuleBuilder<T, string> ruleBuilder) =>
				ruleBuilder.Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");

		#endregion

		public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(
			this IRuleBuilder<T, TProperty> ruleBuilder) =>
			ruleBuilder
				.NotNull().WithMessage(ValidationErrorMessages.REQUIRED);

		public static IRuleBuilderOptions<T, string> TextNotEmptyNorWhitespace<T>(
			this IRuleBuilder<T, string> ruleBuilder) =>
			ruleBuilder
				.Must(property => property.Trim().Length != 0).WithMessage(
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);

		public static IRuleBuilderOptions<T, TProperty> NotNullCollectionWithMinCount<T, TProperty>(
			this IRuleBuilder<T, TProperty> ruleBuilder, int minItemsCount)
				where TProperty : IEnumerable<object> =>
			ruleBuilder
				.Must(property => property?.Count() >= minItemsCount).WithMessage(
					ValidationErrorMessages.RequiredAtLeast(minItemsCount));

		public static IRuleBuilderOptions<T, string> TextMaxLength<T>(
			this IRuleBuilder<T, string> ruleBuilder, int maxLength) =>
			ruleBuilder
				.MaximumLength(maxLength).WithMessage(
					ValidationErrorMessages.TextMaxLength(maxLength));

		public static IRuleBuilderOptions<T, string> NumberDecimal<T>(
			this IRuleBuilder<T, string> ruleBuilder) =>
			ruleBuilder
				.Must(property => decimal.TryParse(property, out _)).WithMessage(
					ValidationErrorMessages.NUMBER_DECIMAL);

		public static IRuleBuilderOptions<T, string> TextNotEmptyNorContainsWhitespace<T>(
			this IRuleBuilder<T, string> ruleBuilder) =>
			ruleBuilder
				.Must(property =>
					property != string.Empty &&
					property.Contains(' ') == false)
				.WithMessage(ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);



		#region Customer

		public static IRuleBuilderOptions<T, string> CustomerFirstName<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_customer_first_name_max_length);
		}

		public static IRuleBuilderOptions<T, string> CustomerLastName<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_customer_last_name_max_length);
		}

		public static IRuleBuilderOptions<T, string> CustomerPhoneNumber<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.TextNotEmptyNorContainsWhitespace()
				.PhoneNumberFormatE164().WithMessage(
					ValidationErrorMessages.PHONE_NUMBER_FORMAT_E164);
		}

		public static IRuleBuilderOptions<T, string> CustomerEmail<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.TextNotEmptyNorContainsWhitespace()
				.EmailFormat().WithMessage(ValidationErrorMessages.EMAIL_FORMAT);
		}

		public static IRuleBuilderOptions<T, string> CustomerTotalPurchasesAmount<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.TextNotEmptyNorContainsWhitespace()
				.NumberDecimal();
		}

		#endregion

		#region Address

		public static IRuleBuilderOptions<T, string> AddressLine<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_address_line_max_length);
		}

		public static IRuleBuilderOptions<T, string> AddressLine2<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_address_line2_max_length);
		}

		public static IRuleBuilderOptions<T, TProperty> AddressTypeEnum<T, TProperty>(
			this IRuleBuilder<T, TProperty> ruleBuilder)
		{
			return ruleBuilder
				.Must(type => Enum.IsDefined(typeof(AddressType), type)).WithMessage(
					ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
		}

		public static IRuleBuilderOptions<T, string> AddressCity<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_address_city_max_length);

		}

		public static IRuleBuilderOptions<T, string> AddressPostalCode<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorContainsWhitespace()
				.TextMaxLength(_address_postalCode_max_length);
		}

		public static IRuleBuilderOptions<T, string> AddressState<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_address_state_max_length);
		}

		public static IRuleBuilderOptions<T, string> AddressCountry<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorWhitespace()
				.Must(country => _address_country_allowed.Contains(country)).WithMessage(
					ValidationErrorMessages.TextAllowedValues(_address_country_allowed));
		}

		#endregion

		#region Note

		public static IRuleBuilderOptions<T, string> NoteContent<T>(
			this IRuleBuilderInitial<T, string> ruleBuilder,
			CascadeMode cascadeMode = CascadeMode.Stop)
		{
			return ruleBuilder.Cascade(cascadeMode)
				.Required()
				.TextNotEmptyNorWhitespace()
				.TextMaxLength(_note_content_max_length);
		}

		#endregion
	}
}
