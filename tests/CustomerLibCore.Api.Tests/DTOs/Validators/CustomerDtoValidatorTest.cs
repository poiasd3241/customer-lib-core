using System.Collections.Generic;
using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Business.Localization;
using FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.FluentValidation.ValidationTestHelper;

namespace CustomerLibCore.Api.Tests.DTOs.Validators
{
	public class CustomerDtoValidatorTest
	{
		#region Private members

		private static readonly CustomerDtoValidator _customerDtoValidator = new();

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
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.FirstName = firstName;

			var errors = _customerDtoValidator.Validate(customer, options =>
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
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.LastName = lastName;

			var errors = _customerDtoValidator.Validate(customer, options =>
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
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.PhoneNumber = phoneNumber;

			var errors = _customerDtoValidator.Validate(customer, options =>
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
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.Email = email;

			var errors = _customerDtoValidator.Validate(customer, options =>
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
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.TotalPurchasesAmount = totalPurchasesAmount;

			var errors = _customerDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Addresses

		private class InvalidAddressesDtoData : TheoryData<List<AddressDto>, string, string>
		{
			public InvalidAddressesDtoData()
			{
				Add(null, "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
				Add(new(), "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidAddressesDtoData))]
		public void ShouldInvalidateByBadAddresses(
			List<AddressDto> addresses, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.Addresses);
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.Addresses = addresses;

			var errors = _customerDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		[Fact]
		public void ShouldInvalidateByBadAddressContent()
		{
			var invalidPropertyName = nameof(CustomerDto.Addresses);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.Line = null;

			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.Addresses = new() { address };

			var errors = _customerDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);

			Assert.Equal($"{invalidPropertyName}[0].{nameof(AddressDto.Line)}", error.PropertyName);
			Assert.Equal(ValidationErrorMessages.REQUIRED, error.ErrorMessage);

		}

		#endregion

		#region Invalid property - Notes

		private class InvalidNotesDtoData : TheoryData<List<NoteDto>, string, string>
		{
			public InvalidNotesDtoData()
			{
				Add(null, "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
				Add(new(), "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidNotesDtoData))]
		public void ShouldInvalidateByBadNotes(
			List<NoteDto> notes, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(CustomerDto.Notes);
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.Notes = notes;

			var errors = _customerDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		[Fact]
		public void ShouldInvalidateByBadNoteContent()
		{
			var invalidPropertyName = nameof(CustomerDto.Notes);
			var note = NoteDtoValidatorFixture.MockNoteDto();
			note.Content = null;

			var customer = CustomerDtoValidatorFixture.MockCustomerDto();
			customer.Notes = new() { note };

			var errors = _customerDtoValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);

			Assert.Equal($"{invalidPropertyName}[0].{nameof(NoteDto.Content)}", error.PropertyName);
			Assert.Equal(ValidationErrorMessages.REQUIRED, error.ErrorMessage);
		}

		#endregion

		#region BasicDetails properties RuleSet

		[Fact]
		public void ShouldValidateCustomerBasicDetailsRuleSet()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();

			// Make non-BasicDetails properties invalid.
			customer.Addresses = null;
			customer.Notes = null;

			// When
			var resultBasicDetailsRuleSet = _customerDtoValidator.Validate(
				customer, options => options.IncludeRuleSets("BasicDetails"));

			var fullErrors = _customerDtoValidator.ValidateFull(customer).Errors;

			// Then
			Assert.True(resultBasicDetailsRuleSet.IsValid);

			Assert.Equal(2, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(CustomerDto.Addresses),
				nameof(CustomerDto.Notes)
			});
		}

		[Fact]
		public void ShouldInvalidateCustomerBasicDetailsRuleSet()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockInvalidCustomerDto();

			// When
			var basicDetailsRuleSetErrors = _customerDtoValidator.Validate(
				customer, options => options.IncludeRuleSets("BasicDetails")).Errors;

			var fullErrors = _customerDtoValidator.ValidateFull(customer).Errors;

			// Then
			Assert.Equal(5, basicDetailsRuleSetErrors.Count);

			AssertValidationFailuresContainPropertyNames(basicDetailsRuleSetErrors, new[]
			{
				nameof(CustomerDto.FirstName),
				nameof(CustomerDto.LastName),
				nameof(CustomerDto.PhoneNumber),
				nameof(CustomerDto.Email),
				nameof(CustomerDto.TotalPurchasesAmount)
			});

			Assert.Equal(7, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(CustomerDto.FirstName),
				nameof(CustomerDto.LastName),
				nameof(CustomerDto.PhoneNumber),
				nameof(CustomerDto.Email),
				nameof(CustomerDto.TotalPurchasesAmount),
				nameof(CustomerDto.Addresses),
				nameof(CustomerDto.Notes)
			});
		}

		#endregion

		#region Validation without Addresses and Notes

		[Fact]
		public void ShouldValidateCustomerExcludingAddressesAndNotes()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();

			// Make non-BasicDetails properties invalid.
			customer.Addresses = null;
			customer.Notes = null;

			// When
			var result = _customerDtoValidator
				.ValidateWithoutAddressesAndNotes(customer);

			var fullErrors = _customerDtoValidator.ValidateFull(customer).Errors;

			// Then
			Assert.True(result.IsValid);

			Assert.Equal(2, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(CustomerDto.Addresses),
				nameof(CustomerDto.Notes)
			});
		}

		[Fact]
		public void ShouldInvalidateCustomerExcludingAddressesAndNotes()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockInvalidCustomerDto();

			// When
			var errors = _customerDtoValidator.ValidateWithoutAddressesAndNotes(customer).Errors;

			var fullErrors = _customerDtoValidator.ValidateFull(customer).Errors;

			// Then
			Assert.Equal(5, errors.Count);

			AssertValidationFailuresContainPropertyNames(errors, new[]
			{
				nameof(CustomerDto.FirstName),
				nameof(CustomerDto.LastName),
				nameof(CustomerDto.PhoneNumber),
				nameof(CustomerDto.Email),
				nameof(CustomerDto.TotalPurchasesAmount)
			});

			Assert.Equal(7, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(CustomerDto.FirstName),
				nameof(CustomerDto.LastName),
				nameof(CustomerDto.PhoneNumber),
				nameof(CustomerDto.Email),
				nameof(CustomerDto.TotalPurchasesAmount),
				nameof(CustomerDto.Addresses),
				nameof(CustomerDto.Notes)
			});
		}

		#endregion

		#region Full validation - all RuleSets

		[Fact]
		public void ShouldValidateFullCustomer()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockCustomerDto();

			Assert.NotNull(customer.FirstName);
			Assert.NotNull(customer.Email);
			Assert.NotNull(customer.PhoneNumber);
			Assert.NotNull(customer.TotalPurchasesAmount);

			// When
			var result = _customerDtoValidator.ValidateFull(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullCustomerWithOptionalNullProperties()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockOptionalCustomerDto();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.Email);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.TotalPurchasesAmount);

			// When
			var result = _customerDtoValidator.ValidateFull(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullCustomer()
		{
			// Given
			var customer = CustomerDtoValidatorFixture.MockInvalidCustomerDto();

			// When
			var errors = _customerDtoValidator.ValidateFull(customer).Errors;

			// Then
			Assert.Equal(7, errors.Count);

			AssertValidationFailuresContainPropertyNames(errors, new[]
			{
				nameof(CustomerDto.FirstName),
				nameof(CustomerDto.LastName),
				nameof(CustomerDto.PhoneNumber),
				nameof(CustomerDto.Email),
				nameof(CustomerDto.TotalPurchasesAmount),
				nameof(CustomerDto.Addresses),
				nameof(CustomerDto.Notes)
			});
		}

		#endregion
	}

	public class CustomerDtoValidatorFixture
	{
		/// <returns>The mocked customer with valid properties 
		/// (according to <see cref="CustomerDtoValidator"/>), optional properties not null.</returns>
		public static CustomerDto MockCustomerDto() => new()
		{
			FirstName = "a",
			LastName = "a",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
			Addresses = new() { AddressDtoValidatorFixture.MockAddressDto() },
			Notes = new() { NoteDtoValidatorFixture.MockNoteDto() }
		};

		/// <returns>The mocked customer with invalid properties 
		/// (according to <see cref="CustomerDtoValidator"/>)</returns>
		public static CustomerDto MockInvalidCustomerDto() => new()
		{
			FirstName = "",
			LastName = "",
			PhoneNumber = "",
			Email = "",
			TotalPurchasesAmount = "",
			Addresses = null,
			Notes = null
		};

		/// <returns>The mocked customer with valid properties 
		/// (according to <see cref="CustomerDtoValidator"/>), optional properties null.</returns>
		public static CustomerDto MockOptionalCustomerDto() => new()
		{
			FirstName = null,
			LastName = "a",
			PhoneNumber = null,
			Email = null,
			TotalPurchasesAmount = null,
			Addresses = new() { AddressDtoValidatorFixture.MockAddressDto() },
			Notes = new() { NoteDtoValidatorFixture.MockNoteDto() }
		};
	}
}
