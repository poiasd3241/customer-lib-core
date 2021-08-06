using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Models.Validators
{
	public class AddressValidatorTest
	{
		#region Private members

		private static readonly AddressValidator _validator = new();

		private static Func<IAddressDetails, IEnumerable<ValidationFailure>>
			GetErrorsSourceFromDetails(string propertyName)
		{
			return (customer) =>
				_validator.ValidateProperty((Address)customer, propertyName);
		}

		private static void AssertSinglePropertyInvalidForDetails(string propertyName,
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			var address = new AddressValidatorFixture().MockValid();

			AddressDetailsValidationTestHelper.AssertSinglePropertyInvalid(
				address, GetErrorsSourceFromDetails(propertyName), propertyName,
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line))]
		public void ShouldInvalidateByBadLine(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.Line),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line2

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
		public void ShouldInvalidateByBadLine2(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.Line2),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Type

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Type))]
		public void ShouldInvalidateByBadType(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.Type),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - City

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
		public void ShouldInvalidateByBadCity(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.City),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - PostalCode

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
		public void ShouldInvalidateByBadPostalCode(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.PostalCode),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - State

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
		public void ShouldInvalidateByBadState(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.State),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Country

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
		public void ShouldInvalidateByBadCountry(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(Address.Country),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectOptionalPropertiesNotNull()
		{
			// Given
			var address = new AddressValidatorFixture().MockValid();

			Assert.NotNull(address.Line2);

			// When
			var result = _validator.Validate(address);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullObjectOptionalPropertiesNull()
		{
			// Given
			var address = new AddressValidatorFixture().MockValidOptional();

			Assert.Null(address.Line2);

			// When
			var result = _validator.Validate(address);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (address, details) = new AddressValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(address).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class AddressValidatorFixture : IValidatorFixture<Address>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="AddressValidator"/>), optional properties not null.
		/// </returns>
		public Address MockValid() => new()
		{
			Line = "Line1",
			Line2 = "Line21",
			Type = "Shipping",
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="Address.Line"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="Address.Line2"/> = "",
		/// <br/>
		/// <see cref="Address.Type"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="Address.City"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="Address.PostalCode"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="Address.State"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="Address.Country"/> = <see langword="null"/>,
		/// <br/>
		/// (according to <see cref="AddressValidator"/>).</returns>
		public Address MockInvalid() => new()
		{
			Line = null,
			Line2 = "",
			Type = null,
			City = null,
			PostalCode = null,
			State = null,
			Country = null
		};

		/// <returns>
		/// invalidObject: <see cref="MockInvalid"/>
		/// <br/>
		/// details: values corresponding to all invalid properties of the object
		/// <br/>
		/// (according to <see cref="AddressValidator"/>).</returns>
		public (Address invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(Address.Line), ValidationErrorMessages.REQUIRED),
				(nameof(Address.Line2), ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(Address.Type), ValidationErrorMessages.REQUIRED),
				(nameof(Address.City), ValidationErrorMessages.REQUIRED),
				(nameof(Address.PostalCode), ValidationErrorMessages.REQUIRED),
				(nameof(Address.State), ValidationErrorMessages.REQUIRED),
				(nameof(Address.Country), ValidationErrorMessages.REQUIRED),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="AddressValidator"/>), optional properties null.
		/// </returns>
		public Address MockValidOptional()
		{
			var address = MockValid();

			address.Line2 = null;

			return address;
		}
	}
}
