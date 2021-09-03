using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Validators.Customers.Request;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerEditRequestValidatorTest
	{
		#region Private members

		private static readonly CustomerEditRequestValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		   string propertyValue, (string expected, string confirm) errorMessages)
		{
			var customer = new CustomerEditRequestValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(CustomerEditRequest.FirstName):
					customer.FirstName = propertyValue;
					break;
				case nameof(CustomerEditRequest.LastName):
					customer.LastName = propertyValue;
					break;
				case nameof(CustomerEditRequest.PhoneNumber):
					customer.PhoneNumber = propertyValue;
					break;
				case nameof(CustomerEditRequest.Email):
					customer.Email = propertyValue;
					break;
				case nameof(CustomerEditRequest.TotalPurchasesAmount):
					customer.TotalPurchasesAmount = propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = _validator.ValidateProperty(customer, propertyName);

			errors.AssertSinglePropertyInvalid(propertyName,
				errorMessages);
		}

		#endregion

		#region Invalid property - First name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.FirstName))]
		public void ShouldInvalidateByBadFirstName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEditRequest.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEditRequest.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEditRequest.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEditRequest.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - TotalPurchasesAmount

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.TotalPurchasesAmount))]
		public void ShouldInvalidateByBadTotalPurchasesAmount(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEditRequest.TotalPurchasesAmount),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerEditRequestValidatorFixture().MockValid();

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
			var customer = new CustomerEditRequestValidatorFixture().MockValidOptional();

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
			var (customer, details) = new CustomerEditRequestValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerEditRequestValidatorFixture : IValidatorFixture<CustomerEditRequest>
	{
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="CustomerEditRequestValidator"/>).</returns>
		public CustomerEditRequest MockValid() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = "666",
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerEditRequest.FirstName"/> = "";
		/// <br/>
		/// <see cref="CustomerEditRequest.LastName"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="CustomerEditRequest.PhoneNumber"/> = "";
		/// <br/>
		/// <see cref="CustomerEditRequest.Email"/> = "";
		/// <br/>
		/// <see cref="CustomerEditRequest.TotalPurchasesAmount"/> = "";
		/// <br/>
		/// (according to <see cref="CustomerEditRequestValidator"/>).</returns>
		public CustomerEditRequest MockInvalid() => new()
		{
			FirstName = "",
			LastName = null,
			PhoneNumber = "",
			Email = "",
			TotalPurchasesAmount = ""
		};

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="CustomerEditRequestValidator"/>).</returns>
		public (CustomerEditRequest invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(CustomerEditRequest.FirstName),
					ErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(CustomerEditRequest.LastName), ErrorMessages.REQUIRED),
				(nameof(CustomerEditRequest.PhoneNumber),
					ErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerEditRequest.Email),
					ErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerEditRequest.TotalPurchasesAmount),
					ErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="CustomerEditRequest.FirstName"/>;
		/// <br/>
		/// <see cref="CustomerEditRequest.PhoneNumber"/>;
		/// <br/>
		/// <see cref="CustomerEditRequest.Email"/>;
		/// <br/>
		/// <see cref="CustomerEditRequest.TotalPurchasesAmount"/>
		/// <br/>
		/// (according to <see cref="CustomerEditRequestValidator"/>).</returns>
		public CustomerEditRequest MockValidOptional()
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
