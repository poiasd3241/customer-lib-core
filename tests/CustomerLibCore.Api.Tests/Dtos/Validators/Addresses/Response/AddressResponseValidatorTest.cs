using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses.Response;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Response;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
	public class AddressResponseValidatorTest
	{
		#region Private members

		private static readonly AddressResponseValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		  string propertyValue, (string expected, string confirm) errorMessages)
		{
			var address = new AddressResponseValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(AddressResponse.Self):
					address.Self = propertyValue;
					break;
				case nameof(AddressResponse.Line):
					address.Line = propertyValue;
					break;
				case nameof(AddressResponse.Line2):
					address.Line2 = propertyValue;
					break;
				case nameof(AddressResponse.Type):
					address.Type = propertyValue;
					break;
				case nameof(AddressResponse.City):
					address.City = propertyValue;
					break;
				case nameof(AddressResponse.PostalCode):
					address.PostalCode = propertyValue;
					break;
				case nameof(AddressResponse.State):
					address.State = propertyValue;
					break;
				case nameof(AddressResponse.Country):
					address.Country = propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = _validator.ValidateProperty(address, propertyName);

			errors.AssertSinglePropertyInvalid(propertyName,
				errorMessages);
		}

		#endregion

		#region Invalid property - Self

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadSelf(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.Self),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line))]
		public void ShouldInvalidateByBadLine(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.Line),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line2

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
		public void ShouldInvalidateByBadLine2(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.Line2),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Type

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.TypeText))]
		public void ShouldInvalidateByBadType(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.Type),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - City

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
		public void ShouldInvalidateByBadCity(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.City),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - PostalCode

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
		public void ShouldInvalidateByBadPostalCode(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.PostalCode),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - State

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
		public void ShouldInvalidateByBadState(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.State),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Country

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
		public void ShouldInvalidateByBadCountry(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressResponse.Country),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
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
		public void ShouldValidateFullObjectWithOptionalPropertiesNull()
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
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class AddressResponseValidatorFixture : IValidatorFixture<AddressResponse>
	{
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="AddressResponseValidator"/>).</returns>
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
		/// <see cref="AddressResponse.Self"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.Line"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.Line"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.Line2"/> = "";
		/// <br/>
		/// <see cref="AddressResponse.Type"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.City"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.PostalCode"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.State"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressResponse.Country"/> = <see langword="null"/>;
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
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
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

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="AddressResponse.Line2"/>;
		/// <br/>
		/// (according to <see cref="AddressResponseValidator"/>).</returns>
		public AddressResponse MockValidOptional()
		{
			var address = MockValid();

			address.Line2 = null;

			return address;
		}
	}
}
