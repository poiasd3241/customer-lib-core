using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Validators.Customers;
using CustomerLibCore.Api.Tests.Dtos.Validators.Addresses;
using CustomerLibCore.Api.Tests.Dtos.Validators.Notes;
using CustomerLibCore.Business.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerResponseValidatorTest
	{
		#region Private members

		private static readonly CustomerResponseValidator _validator = new();

		private static Func<ICustomerBasicDetails, IEnumerable<ValidationFailure>>
			GetErrorsSource(string propertyName)
		{
			return (customer) =>
				_validator.ValidateProperty((CustomerResponse)customer, propertyName);
		}

		private static void AssertSinglePropertyInvalidBasicDetails(string propertyName,
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			var customer = new CustomerResponseValidatorFixture().MockValid();

			CustomerBasicDetailsValidationTestHelper.AssertSinglePropertyInvalid(
				customer, GetErrorsSource(propertyName), propertyName,
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - First name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.FirstName))]
		public void ShouldInvalidateByBadFirstName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidBasicDetails(nameof(CustomerResponse.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidBasicDetails(nameof(CustomerResponse.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidBasicDetails(nameof(CustomerResponse.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidBasicDetails(nameof(CustomerResponse.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - TotalPurchasesAmount

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.TotalPurchasesAmount))]
		public void ShouldInvalidateByBadTotalPurchasesAmount(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidBasicDetails(nameof(CustomerResponse.TotalPurchasesAmount),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Addresses

		[Fact]
		public void ShouldInvalidateByBadAddressesFullInvalid()
		{
			// Given
			var propertyName = nameof(CustomerResponse.Addresses);

			var (addressListResponse, details) = new AddressListResponseValidatorFixture()
				.MockInvalidWithDetails();

			var customer = new CustomerResponseValidatorFixture().MockValid();
			customer.Addresses = addressListResponse;

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(propertyName, details);
		}

		#endregion

		#region Invalid property - Notes

		[Fact]
		public void ShouldInvalidateByBadNotesFullInvalid()
		{
			// Given
			var propertyName = nameof(CustomerResponse.Notes);

			var (noteListResponse, details) = new NoteListResponseValidatorFixture()
				.MockInvalidWithDetails();

			var customer = new CustomerResponseValidatorFixture().MockValid();
			customer.Notes = noteListResponse;

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(propertyName, details);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerResponseValidatorFixture().MockValid();

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
			var customer = new CustomerResponseValidatorFixture().MockValidOptional();

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
			var (customer, details) = new CustomerResponseValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerResponseValidatorFixture : IValidatorFixture<CustomerResponse>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerResponseValidator"/>),
		/// optional properties not null.</returns>
		public CustomerResponse MockValid() => new()
		{
			Self = "Self1",
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
			Addresses = new AddressListResponseValidatorFixture().MockValid(),
			Notes = new NoteListResponseValidatorFixture().MockValid()
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerResponse.Self"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="CustomerResponse.FirstName"/> = "",
		/// <br/>
		/// <see cref="CustomerResponse.LastName"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="CustomerResponse.PhoneNumber"/> = "",
		/// <br/>
		/// <see cref="CustomerResponse.Email"/> = "",
		/// <br/>
		/// <see cref="CustomerResponse.TotalPurchasesAmount"/> = ""
		/// <br/>
		/// <see cref="CustomerResponse.Addresses"/> = 
		/// <see cref="AddressListResponseValidatorFixture.MockInvalid"/>,
		/// <br/>
		/// <see cref="CustomerResponse.Notes"/> = 
		/// <see cref="NoteListResponseValidatorFixture.MockInvalid"/>
		/// <br/>
		/// (according to <see cref="CustomerResponseValidator"/>).</returns>
		public CustomerResponse MockInvalid()
		{
			var addressListResponse = new AddressListResponseValidatorFixture().MockInvalid();
			var noteListResponse = new NoteListResponseValidatorFixture().MockInvalid();

			return new()
			{
				Self = null,
				FirstName = "",
				LastName = null,
				PhoneNumber = "",
				Email = "",
				TotalPurchasesAmount = "",
				Addresses = addressListResponse,
				Notes = noteListResponse
			};
		}

		/// <returns>
		/// invalidObject: <see cref="MockInvalid"/>
		/// <br/>
		/// details: values corresponding to all invalid properties of the object
		/// <br/>
		/// (according to <see cref="CustomerResponseValidator"/>).</returns>
		public (CustomerResponse invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			IEnumerable<(string propertyName, string errorMessage)> details = new (string, string)[]
			{
				(nameof(CustomerResponse.Self), ValidationErrorMessages.REQUIRED),
				(nameof(CustomerResponse.FirstName),
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(CustomerResponse.LastName), ValidationErrorMessages.REQUIRED),
				(nameof(CustomerResponse.PhoneNumber),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerResponse.Email),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerResponse.TotalPurchasesAmount),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
			};

			var (_, invalidAddressListResponseDetails) = new AddressListResponseValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidAddressListResponseDetails)
			{
				details = details.AppendDetail(nameof(CustomerResponse.Addresses), detail);
			}

			var (_, invalidNoteListResponseDetails) = new NoteListResponseValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidNoteListResponseDetails)
			{
				details = details.AppendDetail(nameof(CustomerResponse.Notes), detail);
			}

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerResponseValidator"/>),
		/// optional properties null.</returns>
		public CustomerResponse MockValidOptional()
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
