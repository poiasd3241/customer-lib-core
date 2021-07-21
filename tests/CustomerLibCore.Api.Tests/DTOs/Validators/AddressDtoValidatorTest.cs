using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Business.Enums;
using CustomerLibCore.Business.Localization;
using FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.FluentValidation.ValidationTestHelper;

namespace CustomerLibCore.Api.Tests.DTOs.Validators
{
	public class AddressDtoValidatorTest
	{
		#region Private members

		private static readonly AddressDtoValidator _addressDtoValidator = new();

		#endregion

		#region Invalid property - Line

		private class InvalidLineData : TheoryData<string, string, string>
		{
			public InvalidLineData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 101), "max 100 characters",
					ValidationErrorMessages.TextMaxLength(100));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidLineData))]
		public void ShouldInvalidateByBadLine(
			string line, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.Line);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.Line = line;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Line2

		private class InvalidLine2Data : TheoryData<string, string, string>
		{
			public InvalidLine2Data()
			{
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 101), "max 100 characters",
					ValidationErrorMessages.TextMaxLength(100));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidLine2Data))]
		public void ShouldInvalidateByBadLine2(
			string line2, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.Line2);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.Line2 = line2;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Type

		private class InvalidTypeData : TheoryData<string, string, string>
		{
			public InvalidTypeData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add(" ", "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add("0", "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add(((int)AddressType.Shipping).ToString(), "unknown type",
					ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add(((int)AddressType.Billing).ToString(), "unknown type",
					ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add("666", "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add(" Shipping ", "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
				Add("whatever", "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
			}
		}

		[Theory]
		[ClassData(typeof(InvalidTypeData))]
		public void ShouldInvalidateByBadType(
			string type, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.Type);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.Type = type;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - City

		private class InvalidCityData : TheoryData<string, string, string>
		{
			public InvalidCityData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 51), "max 50 characters", ValidationErrorMessages.TextMaxLength(50));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidCityData))]
		public void ShouldInvalidateByBadCity(
			string city, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.City);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.City = city;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - PostalCode

		private class InvalidPostalCodeData : TheoryData<string, string, string>
		{
			public InvalidPostalCodeData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add(" ", "cannot be empty or contain whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
				Add(new('a', 7), "max 6 characters", ValidationErrorMessages.TextMaxLength(6));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidPostalCodeData))]
		public void ShouldInvalidateByBadPostalCode(
			string postalCode, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.PostalCode);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.PostalCode = postalCode;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - State

		private class InvalidStateData : TheoryData<string, string, string>
		{
			public InvalidStateData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 21), "max 20 characters", ValidationErrorMessages.TextMaxLength(20));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidStateData))]
		public void ShouldInvalidateByBadState(
			string state, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.State);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.State = state;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region Invalid property - Country

		private class InvalidCountryData : TheoryData<string, string, string>
		{
			public InvalidCountryData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add("Japan", "allowed only 'United States', 'Canada'",
					ValidationErrorMessages.TextAllowedValues(new[] { "United States", "Canada" }));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidCountryData))]
		public void ShouldInvalidateByBadCountry(
			string country, string expectedErrorMessage, string confirmErrorMessage)
		{
			// Given
			var invalidPropertyName = nameof(AddressDto.Country);
			var address = AddressDtoValidatorFixture.MockAddressDto();
			address.Country = country;

			// When
			var errors = _addressDtoValidator.Validate(address, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region All properties

		[Fact]
		public void ShouldValidateFullAddressWithOptionalPropertiesNotNull()
		{
			// Given
			var address = AddressDtoValidatorFixture.MockAddressDto();

			Assert.NotNull(address.Line2);

			// When
			var result = _addressDtoValidator.Validate(address);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullAddressWithOptionalPropertiesNull()
		{
			// Given
			var address = AddressDtoValidatorFixture.MockOptionalAddressDto();

			Assert.Null(address.Line2);

			// When
			var result = _addressDtoValidator.Validate(address);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullAddress()
		{
			// Given
			var address = AddressDtoValidatorFixture.MockInvalidAddressDto();

			// When
			var errors = _addressDtoValidator.Validate(address).Errors;

			// Then
			Assert.Equal(7, errors.Count);

			AssertValidationFailuresContainPropertyNames(errors, new[]
			{
				nameof(AddressDto.Line),
				nameof(AddressDto.Line2),
				nameof(AddressDto.Type),
				nameof(AddressDto.City),
				nameof(AddressDto.PostalCode),
				nameof(AddressDto.State),
				nameof(AddressDto.Country)
			});
		}

		#endregion
	}

	public class AddressDtoValidatorFixture
	{
		/// <returns>The mocked address DTO with valid properties 
		/// (according to <see cref="AddressDtoValidator"/>), optional properties not null.
		/// </returns>
		public static AddressDto MockAddressDto() => new()
		{
			Line = "line",
			Line2 = "line2",
			Type = "Shipping",
			City = "city",
			PostalCode = "code",
			State = "state",
			Country = "United States"
		};

		/// <returns>The mocked address DTO with invalid properties 
		/// (according to <see cref="AddressDtoValidator"/>).</returns>
		public static AddressDto MockInvalidAddressDto() => new()
		{
			Line = null,
			Line2 = "",
			Type = null,
			City = null,
			PostalCode = null,
			State = null,
			Country = null,
		};

		/// <returns>The mocked address DTO with valid properties 
		/// (according to <see cref="AddressDtoValidator"/>), optional properties null.</returns>
		public static AddressDto MockOptionalAddressDto() => new()
		{
			Line = "line",
			Line2 = null,
			Type = "Shipping",
			City = "city",
			PostalCode = "code",
			State = "state",
			Country = "United States"
		};
	}
}
