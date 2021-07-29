using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Validators.Customers;
using CustomerLibCore.Business.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerUpdateRequestValidatorTest
	{
		#region Private members

		private static readonly CustomerUpdateRequestValidator _validator = new();

		private static Func<ICustomerBasicDetails, IEnumerable<ValidationFailure>> GetErrorsSource(
			string propertyName)
		{
			return (customer) =>
				_validator.ValidateProperty((CustomerUpdateRequest)customer, propertyName);
		}

		private static void AssertSinglePropertyInvalid(string propertyName, string propertyValue,
			(string expected, string confirm) errorMessages)
		{
			var customer = new CustomerUpdateRequestValidatorFixture().MockValid();

			CustomerBasicDetailsValidationTestHelper.AssertSinglePropertyInvalid(
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
			AssertSinglePropertyInvalid(nameof(CustomerUpdateRequest.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerUpdateRequest.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerUpdateRequest.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerUpdateRequest.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - TotalPurchasesAmount

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.TotalPurchasesAmount))]
		public void ShouldInvalidateByBadTotalPurchasesAmount(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerUpdateRequest.TotalPurchasesAmount),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerUpdateRequestValidatorFixture().MockValid();

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
			var customer = new CustomerUpdateRequestValidatorFixture().MockValidOptional();

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
			var (customer, details) = new CustomerUpdateRequestValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerUpdateRequestValidatorFixture : IValidatorFixture<CustomerUpdateRequest>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerUpdateRequestValidator"/>),
		/// optional properties not null.</returns>
		public CustomerUpdateRequest MockValid() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerUpdateRequest.FirstName"/> = "",
		/// <br/>
		/// <see cref="CustomerUpdateRequest.LastName"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="CustomerUpdateRequest.PhoneNumber"/> = "",
		/// <br/>
		/// <see cref="CustomerUpdateRequest.Email"/> = "",
		/// <br/>
		/// <see cref="CustomerUpdateRequest.TotalPurchasesAmount"/> = ""
		/// <br/>
		/// (according to <see cref="CustomerUpdateRequestValidator"/>).</returns>
		public CustomerUpdateRequest MockInvalid() => new()
		{
			FirstName = "",
			LastName = null,
			PhoneNumber = "",
			Email = "",
			TotalPurchasesAmount = ""
		};

		/// <returns>
		/// invalidObject: <see cref="MockInvalid"/>
		/// <br/>
		/// details: values corresponding to all invalid properties of the object
		/// <br/>
		/// (according to <see cref="CustomerUpdateRequestValidator"/>).</returns>
		public (CustomerUpdateRequest invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(CustomerUpdateRequest.FirstName),
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(CustomerUpdateRequest.LastName), ValidationErrorMessages.REQUIRED),
				(nameof(CustomerUpdateRequest.PhoneNumber),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerUpdateRequest.Email),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerUpdateRequest.TotalPurchasesAmount),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
			};

			return (MockInvalid(), details);
		}


		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerUpdateRequestValidator"/>),
		/// optional properties null.</returns>
		public CustomerUpdateRequest MockValidOptional()
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
