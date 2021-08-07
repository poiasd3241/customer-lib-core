using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Entities.Validators
{
	public class CustomerEntityValidatorTest
	{
		#region Private members

		private static readonly CustomerEntityValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		   string propertyValue, (string expected, string confirm) errorMessages)
		{
			var customer = new CustomerEntityValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(CustomerEntity.FirstName):
					customer.FirstName = propertyValue;
					break;
				case nameof(CustomerEntity.LastName):
					customer.LastName = propertyValue;
					break;
				case nameof(CustomerEntity.PhoneNumber):
					customer.PhoneNumber = propertyValue;
					break;
				case nameof(CustomerEntity.Email):
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
			AssertSinglePropertyInvalid(nameof(CustomerEntity.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEntity.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEntity.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(CustomerEntity.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerEntityValidatorFixture().MockValid();

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
			var customer = new CustomerEntityValidatorFixture().MockValidOptional();

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
			var (customer, details) = new CustomerEntityValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customer).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerEntityValidatorFixture : IValidatorFixture<CustomerEntity>
	{
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="CustomerEntityValidator"/>).</returns>
		public CustomerEntity MockValid() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@a.aa",
			TotalPurchasesAmount = 123
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerEntity.FirstName"/> = "";
		/// <br/>
		/// <see cref="CustomerEntity.LastName"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="CustomerEntity.PhoneNumber"/> = "";
		/// <br/>
		/// <see cref="CustomerEntity.Email"/> = "";
		/// <br/>
		/// (according to <see cref="CustomerEntityValidator"/>).</returns>
		public CustomerEntity MockInvalid() => new()
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
		/// (according to <see cref="CustomerEntityValidator"/>).</returns>
		public (CustomerEntity invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(CustomerEntity.FirstName),
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(CustomerEntity.LastName), ValidationErrorMessages.REQUIRED),
				(nameof(CustomerEntity.PhoneNumber),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(CustomerEntity.Email),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="CustomerEntity.FirstName"/>;
		/// <br/>
		/// <see cref="CustomerEntity.PhoneNumber"/>;
		/// <br/>
		/// <see cref="CustomerEntity.Email"/>;
		/// <br/>
		/// <see cref="CustomerEntity.TotalPurchasesAmount"/>;
		/// <br/>
		/// (according to <see cref="CustomerEntityValidator"/>).</returns>
		public CustomerEntity MockValidOptional()
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
