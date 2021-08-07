using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Models.Validators
{
	public class CustomerValidatorTest
	{
		#region Private members

		private static readonly CustomerValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		   string propertyValue, (string expected, string confirm) errorMessages)
		{
			var customer = new CustomerValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(Customer.FirstName):
					customer.FirstName = propertyValue;
					break;
				case nameof(Customer.LastName):
					customer.LastName = propertyValue;
					break;
				case nameof(Customer.PhoneNumber):
					customer.PhoneNumber = propertyValue;
					break;
				case nameof(Customer.Email):
					customer.Email = propertyValue;
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
			AssertSinglePropertyInvalid(nameof(Customer.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerValidatorFixture().MockValid();

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
			var customer = new CustomerValidatorFixture().MockValidOptional();

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
			var (customer, details) = new CustomerValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerValidatorFixture : IValidatorFixture<Customer>
	{
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public Customer MockValid() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@a.aa",
			TotalPurchasesAmount = 123
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="Customer.FirstName"/> = "";
		/// <br/>
		/// <see cref="Customer.LastName"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="Customer.PhoneNumber"/> = "";
		/// <br/>
		/// <see cref="Customer.Email"/> = "";
		/// <br/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public Customer MockInvalid() => new()
		{
			FirstName = "",
			LastName = null,
			PhoneNumber = "",
			Email = "",
		};

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public (Customer invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(Customer.FirstName),
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(Customer.LastName), ValidationErrorMessages.REQUIRED),
				(nameof(Customer.PhoneNumber),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(Customer.Email),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="Customer.FirstName"/>;
		/// <br/>
		/// <see cref="Customer.PhoneNumber"/>;
		/// <br/>
		/// <see cref="Customer.Email"/>;
		/// <br/>
		/// <see cref="Customer.TotalPurchasesAmount"/>
		/// <br/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public Customer MockValidOptional()
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
