using System;
using System.Collections.Generic;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Data.Tests.Entities.Validators
{
	public class AddressEntityValidatorTest
	{
		#region Private members

		private static readonly AddressEntityValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		   object propertyValue, (string expected, string confirm) errorMessages)
		{
			var address = new AddressEntityValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(AddressEntity.Line):
					address.Line = (string)propertyValue;
					break;
				case nameof(AddressEntity.Line2):
					address.Line2 = (string)propertyValue;
					break;
				case nameof(AddressEntity.Type):
					address.Type = (AddressType)propertyValue;
					break;
				case nameof(AddressEntity.City):
					address.City = (string)propertyValue;
					break;
				case nameof(AddressEntity.PostalCode):
					address.PostalCode = (string)propertyValue;
					break;
				case nameof(AddressEntity.State):
					address.State = (string)propertyValue;
					break;
				case nameof(AddressEntity.Country):
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
			AssertSinglePropertyInvalid(nameof(AddressEntity.Line),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Line2

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
		public void ShouldInvalidateByBadLine2(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressEntity.Line2),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Type

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.TypeEnum))]
		public void ShouldInvalidateByBadType(
			AddressType propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressEntity.Type),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - City

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
		public void ShouldInvalidateByBadCity(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressEntity.City),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - PostalCode

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
		public void ShouldInvalidateByBadPostalCode(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressEntity.PostalCode),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - State

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
		public void ShouldInvalidateByBadState(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressEntity.State),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Country

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
		public void ShouldInvalidateByBadCountry(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(AddressEntity.Country),
				propertyValue, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var address = new AddressEntityValidatorFixture().MockValid();

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
			var address = new AddressEntityValidatorFixture().MockValidOptional();

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
			var (address, details) = new AddressEntityValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(address).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class AddressEntityValidatorFixture : IValidatorFixture<AddressEntity>
	{
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="AddressEntityValidator"/>).</returns>
		public AddressEntity MockValid() => new()
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
		/// <see cref="AddressEntity.Line"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressEntity.Line2"/> = "";
		/// <br/>
		/// <see cref="AddressEntity.Type"/> = 0;
		/// <br/>
		/// <see cref="AddressEntity.City"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressEntity.PostalCode"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressEntity.State"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressEntity.Country"/> = <see langword="null"/>;
		/// <br/>
		/// (according to <see cref="AddressEntityValidator"/>).</returns>
		public AddressEntity MockInvalid() => new()
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
		/// (according to <see cref="AddressEntityValidator"/>).</returns>
		public (AddressEntity invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(AddressEntity.Line), ValidationErrorMessages.REQUIRED),
				(nameof(AddressEntity.Line2), ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(AddressEntity.Type), ValidationErrorMessages.ENUM_TYPE_UNKNOWN),
				(nameof(AddressEntity.City), ValidationErrorMessages.REQUIRED),
				(nameof(AddressEntity.PostalCode), ValidationErrorMessages.REQUIRED),
				(nameof(AddressEntity.State), ValidationErrorMessages.REQUIRED),
				(nameof(AddressEntity.Country), ValidationErrorMessages.REQUIRED),
			};

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="AddressEntity.Line2"/>;
		/// <br/>
		/// (according to <see cref="AddressEntityValidator"/>).</returns>
		public AddressEntity MockValidOptional()
		{
			var address = MockValid();

			address.Line2 = null;

			return address;
		}
	}
}
