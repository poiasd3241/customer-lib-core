using System.Collections.Generic;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Localization;
using CustomerLibCore.Business.Validators;
using FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.FluentValidation.ValidationTestHelper;

namespace CustomerLibCore.Business.Tests.Validators
{
	public class CustomerValidatorTest
	{
		#region Private members

		private static readonly CustomerValidator _customerValidator = new();

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
			var invalidPropertyName = nameof(Customer.FirstName);
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.FirstName = firstName;

			var errors = _customerValidator.Validate(customer, options =>
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
			var invalidPropertyName = nameof(Customer.LastName);
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.LastName = lastName;

			var errors = _customerValidator.Validate(customer, options =>
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
			var invalidPropertyName = nameof(Customer.PhoneNumber);
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.PhoneNumber = phoneNumber;

			var errors = _customerValidator.Validate(customer, options =>
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
			var invalidPropertyName = nameof(Customer.Email);
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.Email = email;

			var errors = _customerValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Addresses

		private class InvalidAddressesData : TheoryData<List<Address>, string, string>
		{
			public InvalidAddressesData()
			{
				Add(null, "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
				Add(new(), "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidAddressesData))]
		public void ShouldInvalidateByBadAddresses(
			List<Address> addresses, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(Customer.Addresses);
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.Addresses = addresses;

			var errors = _customerValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		[Fact]
		public void ShouldInvalidateByBadAddressContent()
		{
			var invalidPropertyName = nameof(Customer.Addresses);
			var address = AddressValidatorFixture.MockAddress();
			address.Line = null;

			var customer = CustomerValidatorFixture.MockCustomer();
			customer.Addresses = new() { address };

			var errors = _customerValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);

			Assert.Equal($"{invalidPropertyName}[0].{nameof(Address.Line)}", error.PropertyName);
			Assert.Equal(ValidationErrorMessages.REQUIRED, error.ErrorMessage);
		}

		#endregion

		#region Invalid property - Notes

		private class InvalidNotesData : TheoryData<List<Note>, string, string>
		{
			public InvalidNotesData()
			{
				Add(null, "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
				Add(new(), "required at least 1", ValidationErrorMessages.RequiredAtLeast(1));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidNotesData))]
		public void ShouldInvalidateByBadNotes(
			List<Note> notes, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(Customer.Notes);
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.Notes = notes;

			var errors = _customerValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		[Fact]
		public void ShouldInvalidateByBadNoteContent()
		{
			var invalidPropertyName = nameof(Customer.Notes);
			var note = NoteValidatorFixture.MockNote();
			note.Content = null;

			var customer = CustomerValidatorFixture.MockCustomer();
			customer.Notes = new() { note };

			var errors = _customerValidator.Validate(customer, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);

			Assert.Equal($"{invalidPropertyName}[0].{nameof(Note.Content)}", error.PropertyName);
			Assert.Equal(ValidationErrorMessages.REQUIRED, error.ErrorMessage);
		}

		#endregion

		#region BasicDetails properties RuleSet

		[Fact]
		public void ShouldValidateCustomerBasicDetailsRuleSet()
		{
			// Given
			var customer = CustomerValidatorFixture.MockCustomer();

			// Make non-BasicDetails properties invalid.
			customer.Addresses = null;
			customer.Notes = null;

			// When
			var resultBasicDetailsRuleSet = _customerValidator.Validate(
				customer, options => options.IncludeRuleSets("BasicDetails"));

			var fullErrors = _customerValidator.ValidateFull(customer).Errors;

			// Then
			Assert.True(resultBasicDetailsRuleSet.IsValid);

			Assert.Equal(2, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(Customer.Addresses),
				nameof(Customer.Notes)
			});
		}

		[Fact]
		public void ShouldInvalidateCustomerBasicDetailsRuleSet()
		{
			// Given
			var customer = CustomerValidatorFixture.MockInvalidCustomer();

			// When
			var basicDetailsRuleSetErrors = _customerValidator.Validate(
				customer, options => options.IncludeRuleSets("BasicDetails")).Errors;

			var fullErrors = _customerValidator.ValidateFull(customer).Errors;

			// Then
			Assert.Equal(4, basicDetailsRuleSetErrors.Count);

			AssertValidationFailuresContainPropertyNames(basicDetailsRuleSetErrors, new[]
			{
				nameof(Customer.FirstName),
				nameof(Customer.LastName),
				nameof(Customer.PhoneNumber),
				nameof(Customer.Email)
			});

			Assert.Equal(6, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(Customer.FirstName),
				nameof(Customer.LastName),
				nameof(Customer.PhoneNumber),
				nameof(Customer.Email),
				nameof(Customer.Addresses),
				nameof(Customer.Notes)
			});
		}

		#endregion

		#region Validation without Addresses and Notes

		[Fact]
		public void ShouldValidateCustomerExcludingAddressesAndNotes()
		{
			// Given
			var customer = CustomerValidatorFixture.MockCustomer();
			customer.Addresses = null;
			customer.Notes = null;

			// When
			var result = _customerValidator.ValidateWithoutAddressesAndNotes(customer);

			var fullErrors = _customerValidator.ValidateFull(customer).Errors;

			// Then
			Assert.True(result.IsValid);

			Assert.Equal(2, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(Customer.Addresses),
				nameof(Customer.Notes)
			});
		}

		[Fact]
		public void ShouldInvalidateCustomerExcludingAddressesAndNotes()
		{
			// Given
			var customer = CustomerValidatorFixture.MockInvalidCustomer();

			// When
			var errors = _customerValidator.ValidateWithoutAddressesAndNotes(customer).Errors;

			var fullErrors = _customerValidator.ValidateFull(customer).Errors;

			// Then
			Assert.Equal(4, errors.Count);

			AssertValidationFailuresContainPropertyNames(errors, new[]
			{
				nameof(Customer.FirstName),
				nameof(Customer.LastName),
				nameof(Customer.PhoneNumber),
				nameof(Customer.Email)
			});

			Assert.Equal(6, fullErrors.Count);

			AssertValidationFailuresContainPropertyNames(fullErrors, new[]
			{
				nameof(Customer.FirstName),
				nameof(Customer.LastName),
				nameof(Customer.PhoneNumber),
				nameof(Customer.Email),
				nameof(Customer.Addresses),
				nameof(Customer.Notes)
			});
		}

		#endregion

		#region Full validation - all RuleSets

		[Fact]
		public void ShouldValidateFullCustomerWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = CustomerValidatorFixture.MockCustomer();

			Assert.NotNull(customer.FirstName);
			Assert.NotNull(customer.Email);
			Assert.NotNull(customer.PhoneNumber);
			Assert.NotNull(customer.TotalPurchasesAmount);


			// When
			var result = _customerValidator.ValidateFull(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullCustomerWithOptionalPropertiesNull()
		{
			// Given
			var customer = CustomerValidatorFixture.MockOptionalCustomer();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.Email);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.TotalPurchasesAmount);

			// When
			var result = _customerValidator.ValidateFull(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullCustomer()
		{
			// Given
			var customer = CustomerValidatorFixture.MockInvalidCustomer();

			// When
			var errors = _customerValidator.ValidateFull(customer).Errors;

			// Then
			Assert.Equal(6, errors.Count);

			AssertValidationFailuresContainPropertyNames(errors, new[]
			{
				nameof(Customer.FirstName),
				nameof(Customer.LastName),
				nameof(Customer.PhoneNumber),
				nameof(Customer.Email),
				nameof(Customer.Addresses),
				nameof(Customer.Notes)
			});
		}

		#endregion
	}

	public class CustomerValidatorFixture
	{
		/// <returns>The mocked customer with valid properties 
		/// (according to <see cref="CustomerValidator"/>), optional properties not null.</returns>
		public static Customer MockCustomer() => new()
		{
			FirstName = "a",
			LastName = "a",
			Addresses = new() { AddressValidatorFixture.MockAddress() },
			PhoneNumber = "+123",
			Email = "a@a.aa",
			Notes = new() { NoteValidatorFixture.MockNote() },
			TotalPurchasesAmount = 123,
		};

		/// <returns>The mocked customer with invalid properties 
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public static Customer MockInvalidCustomer() => new()
		{
			FirstName = "",
			LastName = "",
			PhoneNumber = "",
			Email = "",
			TotalPurchasesAmount = 123,
			Notes = null,
			Addresses = null,
		};

		/// <returns>The mocked customer with valid properties 
		/// (according to <see cref="CustomerValidator"/>), optional properties null.</returns>
		public static Customer MockOptionalCustomer() => new()
		{
			FirstName = null,
			LastName = "a",
			Addresses = new() { AddressValidatorFixture.MockAddress() },
			PhoneNumber = null,
			Email = null,
			Notes = new() { NoteValidatorFixture.MockNote() },
			TotalPurchasesAmount = null
		};
	}
}
