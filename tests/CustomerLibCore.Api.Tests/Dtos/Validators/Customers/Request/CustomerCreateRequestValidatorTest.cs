using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Notes.Request;
using CustomerLibCore.Api.Dtos.Validators.Customers.Request;
using CustomerLibCore.Api.Tests.Dtos.Validators.Addresses;
using CustomerLibCore.Api.Tests.Dtos.Validators.Notes;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerCreateRequestValidatorTest
	{
		#region Private members

		private static readonly CustomerCreateRequestValidator _validator = new();

		private static Func<ICustomerDetails, IEnumerable<ValidationFailure>>
			GetErrorsSource(string propertyName)
		{
			return (customer) =>
				_validator.ValidateProperty((CustomerCreateRequest)customer, propertyName);
		}

		private static void AssertSinglePropertyInvalidDetails(string propertyName,
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			var customer = new CustomerCreateRequestValidatorFixture().MockValid();

			CustomerDetailsValidationTestHelper.AssertSinglePropertyInvalid(
				customer, GetErrorsSource(propertyName), propertyName, propertyValue,
				errorMessages);
		}

		#endregion

		#region Invalid property - First name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.FirstName))]
		public void ShouldInvalidateByBadFirstName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidDetails(nameof(CustomerCreateRequest.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidDetails(nameof(CustomerCreateRequest.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidDetails(nameof(CustomerCreateRequest.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidDetails(nameof(CustomerCreateRequest.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - TotalPurchasesAmount

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.TotalPurchasesAmount))]
		public void ShouldInvalidateByBadTotalPurchasesAmount(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidDetails(
				nameof(CustomerCreateRequest.TotalPurchasesAmount),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Addresses [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common
			.CollectionRequiredNotEmpty<AddressRequest>))]
		public void ShouldInvalidateByBadAddressesNullOrEmpty(
			List<AddressRequest> addresses, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerCreateRequest.Addresses);

			var customer = new CustomerCreateRequestValidatorFixture().MockValid();
			customer.Addresses = addresses;

			// When
			var errors = _validator.ValidateProperty(customer, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadAddressesChild()
		{
			// Given
			var propertyName = nameof(CustomerCreateRequest.Addresses);

			var (addressRequest, details) = new AddressRequestValidatorFixture()
				.MockInvalidWithDetails();

			var customer = new CustomerCreateRequestValidatorFixture().MockValid();
			customer.Addresses = new[] { addressRequest };

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Invalid property - Notes [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common
			.CollectionRequiredNotEmpty<NoteRequest>))]
		public void ShouldInvalidateByBadNotesNullOrEmpty(
			List<NoteRequest> notes, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerCreateRequest.Notes);

			var customer = new CustomerCreateRequestValidatorFixture().MockValid();
			customer.Notes = notes;

			// When
			var errors = _validator.ValidateProperty(customer, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadNotesChild()
		{
			// Given
			var propertyName = nameof(CustomerCreateRequest.Notes);

			var (noteRequest, details) = new NoteRequestValidatorFixture()
				.MockInvalidWithDetails();

			var customer = new CustomerCreateRequestValidatorFixture().MockValid();
			customer.Notes = new[] { noteRequest };

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerCreateRequestValidatorFixture().MockValid();

			Assert.NotNull(customer.FirstName);
			Assert.NotNull(customer.Email);
			Assert.NotNull(customer.PhoneNumber);
			Assert.NotNull(customer.TotalPurchasesAmount);

			// When
			var result = _validator.Validate(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNull()
		{
			// Given
			var customer = new CustomerCreateRequestValidatorFixture().MockValidOptional();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.Email);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.TotalPurchasesAmount);

			// When
			var result = _validator.Validate(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (customer, details) = new CustomerCreateRequestValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerCreateRequestValidatorFixture : IValidatorFixture<CustomerCreateRequest>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerCreateRequestValidator"/>),
		/// optional properties not null.</returns>
		public CustomerCreateRequest MockValid() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
			Addresses = new[] { new AddressRequestValidatorFixture().MockValid() },
			Notes = new[] { new NoteRequestValidatorFixture().MockValid() }

		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerCreateRequest.FirstName"/> = "",
		/// <br/>
		/// <see cref="CustomerCreateRequest.LastName"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="CustomerCreateRequest.PhoneNumber"/> = "",
		/// <br/>
		/// <see cref="CustomerCreateRequest.Email"/> = "",
		/// <br/>
		/// <see cref="CustomerCreateRequest.TotalPurchasesAmount"/> = ""
		/// <br/>
		/// <see cref="CustomerCreateRequest.Addresses"/>[0] =
		/// <see cref="AddressRequestValidatorFixture.MockInvalid"/>
		/// <br/>
		/// <see cref="CustomerCreateRequest.Notes"/>[0] =
		/// <see cref="NoteRequestValidatorFixture.MockInvalid"/>
		/// <br/>
		/// (according to <see cref="CustomerCreateRequestValidator"/>).</returns>
		public CustomerCreateRequest MockInvalid()
		{
			var addressRequest = new AddressRequestValidatorFixture().MockInvalid();
			var noteRequest = new NoteRequestValidatorFixture().MockInvalid();

			return new()
			{
				FirstName = "",
				LastName = null,
				PhoneNumber = "",
				Email = "",
				TotalPurchasesAmount = "",
				Addresses = new[] { addressRequest },
				Notes = new[] { noteRequest }
			};
		}

		/// <returns>
		/// invalidObject: <see cref="MockInvalid"/>
		/// <br/>
		/// details: values corresponding to all invalid properties of the object
		/// <br/>
		/// (according to <see cref="CustomerCreateRequestValidator"/>).</returns>
		public (CustomerCreateRequest invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			IEnumerable<(string propertyName, string errorMessage)> details = new (string, string)[]
			{
				(nameof(CustomerCreateRequest.FirstName),
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(CustomerCreateRequest.LastName), ValidationErrorMessages.REQUIRED),
				(nameof(CustomerCreateRequest.PhoneNumber),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerCreateRequest.Email),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerCreateRequest.TotalPurchasesAmount),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE)
			};

			var (_, invalidAddressRequestDetails) = new AddressRequestValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidAddressRequestDetails)
			{
				details = details.AppendDetail(
					$"{nameof(CustomerCreateRequest.Addresses)}[0]", detail);
			}

			var (_, invalidNoteRequestDetails) = new NoteRequestValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidNoteRequestDetails)
			{
				details = details.AppendDetail(
					$"{nameof(CustomerCreateRequest.Notes)}[0]", detail);
			}

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerCreateRequestValidator"/>),
		/// optional properties null.</returns>
		public CustomerCreateRequest MockValidOptional()
		{
			var customer = MockValid();

			customer.FirstName = null;
			customer.PhoneNumber = null;
			customer.Email = null;
			customer.TotalPurchasesAmount = null;

			return customer;
		}
	}
}
