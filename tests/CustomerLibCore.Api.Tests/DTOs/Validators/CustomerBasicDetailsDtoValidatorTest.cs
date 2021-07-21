using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Business.Localization;
using FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.FluentValidation.ValidationTestHelper;

namespace CustomerLibCore.Api.Tests.DTOs.Validators
{
	public class CustomerBasicDetailsDtoValidatorTest
	{
		#region Private members

		private static readonly CustomerBasicDetailsDtoValidator
			_customerBasicDetailsDtoValidator = new();

		#endregion

		#region Invalid property - First name

		private class InvalidFirstNameData : TheoryData<string, string, string>
		{
			public InvalidFirstNameData()
			{
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 51), "max 50 characters",
					ValidationErrorMessages.TextMaxLength(50));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidFirstNameData))]
		public void ShouldInvalidateByBadFirstName(
			string firstName, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.FirstName);
			var customer = CustomerBasicDetailsDtoValidatorFixture.MockCustomerBasicDetailsDto();
			customer.FirstName = firstName;

			var errors = _customerBasicDetailsDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Last name

		private class InvalidLastNameData : TheoryData<string, string, string>
		{
			public InvalidLastNameData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 51), "max 50 characters", ValidationErrorMessages.TextMaxLength(50));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidLastNameData))]
		public void ShouldInvalidateByBadLastName(
			string lastName, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.LastName);
			var customer = CustomerBasicDetailsDtoValidatorFixture.MockCustomerBasicDetailsDto();
			customer.LastName = lastName;

			var errors = _customerBasicDetailsDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Phone number

		private class InvalidPhoneNumberData : TheoryData<string, string, string>
		{
			public InvalidPhoneNumberData()
			{
				Add("", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add(" ", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add("123456", "must be in E.164 format",
					ValidationErrorMessages.PHONE_NUMBER_FORMAT_E164);
			}
		}

		[Theory]
		[ClassData(typeof(InvalidPhoneNumberData))]
		public void ShouldInvalidateByBadPhoneNumber(
			string phoneNumber, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.PhoneNumber);
			var customer = CustomerBasicDetailsDtoValidatorFixture.MockCustomerBasicDetailsDto();
			customer.PhoneNumber = phoneNumber;

			var errors = _customerBasicDetailsDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Email

		private class InvalidEmailData : TheoryData<string, string, string>
		{
			public InvalidEmailData()
			{
				Add("", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add(" ", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add("a@a@a", "invalid email format", ValidationErrorMessages.EMAIL_FORMAT);
			}
		}

		[Theory]
		[ClassData(typeof(InvalidEmailData))]
		public void ShouldInvalidateByBadEmail(
			string email, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.Email);
			var customer = CustomerBasicDetailsDtoValidatorFixture.MockCustomerBasicDetailsDto();
			customer.Email = email;

			var errors = _customerBasicDetailsDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - TotalPurchasesAmount

		private class InvalidTotalPurchasesAmountData : TheoryData<string, string, string>
		{
			public InvalidTotalPurchasesAmountData()
			{
				Add("", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add(" ", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add(" 1 ", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add("1.1.1", "must be a decimal point number",
					ValidationErrorMessages.NUMBER_DECIMAL);
				Add("whatever", "must be a decimal point number",
					ValidationErrorMessages.NUMBER_DECIMAL);
			}
		}

		[Theory]
		[ClassData(typeof(InvalidTotalPurchasesAmountData))]
		public void ShouldInvalidateByBadTotalPurchasesAmount(
			string totalPurchasesAmount, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.TotalPurchasesAmount);
			var customer = CustomerBasicDetailsDtoValidatorFixture.MockCustomerBasicDetailsDto();
			customer.TotalPurchasesAmount = totalPurchasesAmount;

			var errors = _customerBasicDetailsDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region All properties

		[Fact]
		public void ShouldValidateFullCustomer()
		{
			// Given
			var customer = CustomerBasicDetailsDtoValidatorFixture.MockCustomerBasicDetailsDto();

			Assert.NotNull(customer.FirstName);
			Assert.NotNull(customer.Email);
			Assert.NotNull(customer.PhoneNumber);
			Assert.NotNull(customer.TotalPurchasesAmount);

			// When
			var result = _customerBasicDetailsDtoValidator.Validate(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullCustomerWithOptionalNullProperties()
		{
			// Given
			var customer = CustomerBasicDetailsDtoValidatorFixture
				.MockOptionalCustomerBasicDetailsDto();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.Email);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.TotalPurchasesAmount);

			// When
			var result = _customerBasicDetailsDtoValidator.Validate(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullCustomer()
		{
			// Given
			var customer = CustomerBasicDetailsDtoValidatorFixture
				.MockInvalidCustomerBasicDetailsDto();

			// When
			var errors = _customerBasicDetailsDtoValidator.Validate(customer).Errors;

			// Then
			Assert.Equal(5, errors.Count);

			AssertValidationFailuresContainPropertyNames(errors, new[]
			{
				nameof(CustomerBasicDetailsDto.FirstName),
				nameof(CustomerBasicDetailsDto.LastName),
				nameof(CustomerBasicDetailsDto.PhoneNumber),
				nameof(CustomerBasicDetailsDto.Email),
				nameof(CustomerBasicDetailsDto.TotalPurchasesAmount)
			});
		}

		#endregion
	}

	public class CustomerBasicDetailsDtoValidatorFixture
	{
		/// <returns>The mocked customer with valid properties 
		/// (according to <see cref="CustomerBasicDetailsDtoValidator"/>),
		/// optional properties not null.</returns>
		public static CustomerBasicDetailsDto MockCustomerBasicDetailsDto() => new()
		{
			FirstName = "a",
			LastName = "a",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
		};

		/// <returns>The mocked customer with invalid properties 
		/// (according to <see cref="CustomerBasicDetailsDtoValidator"/>).</returns>
		public static CustomerBasicDetailsDto MockInvalidCustomerBasicDetailsDto() => new()
		{
			FirstName = "",
			LastName = "",
			PhoneNumber = "",
			Email = "",
			TotalPurchasesAmount = ""
		};

		/// <returns>The mocked customer with valid properties 
		/// (according to <see cref="CustomerBasicDetailsDtoValidator"/>),
		/// optional properties null.</returns>
		public static CustomerBasicDetailsDto MockOptionalCustomerBasicDetailsDto() => new()
		{
			FirstName = null,
			LastName = "a",
			PhoneNumber = null,
			Email = null,
			TotalPurchasesAmount = null
		};
	}
}
