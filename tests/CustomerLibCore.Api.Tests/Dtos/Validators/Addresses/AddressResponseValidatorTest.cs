using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Api.Dtos.Validators.Addresses;
using CustomerLibCore.Business.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
	public class AddressResponseValidatorTest
	{
		#region Private members

		private static readonly AddressResponseValidator _validator = new();

		private static Func<IAddressDetails, IEnumerable<ValidationFailure>>
			GetErrorsSourceFromDetails(string propertyName)
		{
			return (customer) =>
				_validator.ValidateProperty((AddressResponse)customer, propertyName);
		}

		private static void AssertSinglePropertyInvalidForDetails(string propertyName,
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			var address = new AddressResponseValidatorFixture().MockValid();

			AddressDetailsValidationTestHelper.AssertSinglePropertyInvalid(
				address, GetErrorsSourceFromDetails(propertyName), propertyName,
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Self

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadSelf(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(AddressResponse.Self);

			var address = new AddressResponseValidatorFixture().MockValid();
			address.Self = propertyValue;

			// When
			var errors = _validator.ValidateProperty(address, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName,
				errorMessages);
		}

		#endregion

		#region Invalid property - Line

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line))]
		public void ShouldInvalidateByBadLine(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.Line),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line2

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
		public void ShouldInvalidateByBadLine2(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.Line2),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Type

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Type))]
		public void ShouldInvalidateByBadType(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.Type),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - City

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
		public void ShouldInvalidateByBadCity(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.City),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - PostalCode

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
		public void ShouldInvalidateByBadPostalCode(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.PostalCode),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - State

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
		public void ShouldInvalidateByBadState(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.State),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Country

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
		public void ShouldInvalidateByBadCountry(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalidForDetails(nameof(AddressResponse.Country),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectOptionalPropertiesNotNull()
		{
			// Given
			var address = new AddressResponseValidatorFixture().MockValid();

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
			var address = new AddressResponseValidatorFixture().MockValidOptional();

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
			var (address, details) = new AddressResponseValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(address).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class AddressResponseValidatorFixture : IValidatorFixture<AddressResponse>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="AddressResponseValidator"/>), optional properties not null.
		/// </returns>
		public AddressResponse MockValid() => new()
		{
			Self = "Self1",
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
		/// <see cref="AddressResponse.Self"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.Line"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.Line"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.Line2"/> = "",
		/// <br/>
		/// <see cref="AddressResponse.Type"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.City"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.PostalCode"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.State"/> = <see langword="null"/>,
		/// <br/>
		/// <see cref="AddressResponse.Country"/> = <see langword="null"/>,
		/// <br/>
		/// (according to <see cref="AddressResponseValidator"/>).</returns>
		public AddressResponse MockInvalid() => new()
		{
			Self = null,
			Line = null,
			Line2 = "",
			Type = null,
			City = null,
			PostalCode = null,
			State = null,
			Country = null,
		};

		/// <returns>
		/// invalidObject: <see cref="MockInvalid"/>
		/// <br/>
		/// details: values corresponding to all invalid properties of the object
		/// <br/>
		/// (according to <see cref="AddressResponseValidator"/>).</returns>
		public (AddressResponse invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(AddressResponse.Self), ValidationErrorMessages.REQUIRED),
				(nameof(AddressResponse.Line), ValidationErrorMessages.REQUIRED),
				(nameof(AddressResponse.Line2), ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(AddressResponse.Type), ValidationErrorMessages.REQUIRED),
				(nameof(AddressResponse.City), ValidationErrorMessages.REQUIRED),
				(nameof(AddressResponse.PostalCode), ValidationErrorMessages.REQUIRED),
				(nameof(AddressResponse.State), ValidationErrorMessages.REQUIRED),
				(nameof(AddressResponse.Country), ValidationErrorMessages.REQUIRED),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="AddressResponseValidator"/>), optional properties null.
		/// </returns>
		public AddressResponse MockValidOptional()
		{
			var address = MockValid();

			address.Line2 = null;

			return address;
		}
	}
}
