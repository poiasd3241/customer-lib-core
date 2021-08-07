using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Models.Validators
{
	public class AddressValidatorTest
	{
		#region Private members

		private static readonly AddressValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		   object propertyValue, (string expected, string confirm) errorMessages)
		{
			var address = new AddressValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(Address.Line):
					address.Line = (string)propertyValue;
					break;
				case nameof(Address.Line2):
					address.Line2 = (string)propertyValue;
					break;
				case nameof(Address.Type):
					address.Type = (AddressType)propertyValue;
					break;
				case nameof(Address.City):
					address.City = (string)propertyValue;
					break;
				case nameof(Address.PostalCode):
					address.PostalCode = (string)propertyValue;
					break;
				case nameof(Address.State):
					address.State = (string)propertyValue;
					break;
				case nameof(Address.Country):
					address.Country = (string)propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = _validator.ValidateProperty(address, propertyName);

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
			AssertSinglePropertyInvalid(nameof(Address.Line),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line2

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
		public void ShouldInvalidateByBadLine2(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Address.Line2),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Type

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Type))]
		public void ShouldInvalidateByBadType(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Address.Type),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - City

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
		public void ShouldInvalidateByBadCity(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Address.City),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - PostalCode

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
		public void ShouldInvalidateByBadPostalCode(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Address.PostalCode),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - State

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
		public void ShouldInvalidateByBadState(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Address.State),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Country

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
		public void ShouldInvalidateByBadCountry(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Address.Country),
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
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="AddressValidator"/>).</returns>
		public Address MockValid() => new()
		{
			Line = "Line1",
			Line2 = "Line21",
			Type = AddressType.Shipping,
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="Address.Line"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="Address.Line2"/> = "";
		/// <br/>
		/// <see cref="Address.Type"/> = 0;
		/// <br/>
		/// <see cref="Address.City"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="Address.PostalCode"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="Address.State"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="Address.Country"/> = <see langword="null"/>;
		/// <br/>
		/// (according to <see cref="AddressValidator"/>).</returns>
		public Address MockInvalid() => new()
		{
			Line = null,
			Line2 = "",
			Type = 0,
			City = null,
			PostalCode = null,
			State = null,
			Country = null
		};

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
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
				(nameof(Address.Type), ValidationErrorMessages.ENUM_TYPE_UNKNOWN),
				(nameof(Address.City), ValidationErrorMessages.REQUIRED),
				(nameof(Address.PostalCode), ValidationErrorMessages.REQUIRED),
				(nameof(Address.State), ValidationErrorMessages.REQUIRED),
				(nameof(Address.Country), ValidationErrorMessages.REQUIRED),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="Address.Line2"/>;
		/// <br/>
		/// (according to <see cref="AddressValidator"/>).</returns>
		public Address MockValidOptional()
		{
			var address = MockValid();

			address.Line2 = null;

			return address;
		}
	}
}
