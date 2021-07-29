//using CustomerLibCore.Business.Entities;
//using CustomerLibCore.Business.Enums;
//using CustomerLibCore.Business.Localization;
//using CustomerLibCore.Business.Validators;
//using FluentValidation;
//using Xunit;
//using static CustomerLibCore.TestHelpers.FluentValidation.ValidationFailureExtensions;

//namespace CustomerLibCore.Business.Tests.Validators
//{
//	public class AddressValidatorTest
//	{
//		#region Private members

//		private static readonly AddressValidator _addressValidator = new();

//		#endregion

//		#region Invalid property - Line

//		private class InvalidLineData : TheoryData<string, string, string>
//		{
//			public InvalidLineData()
//			{
//				Add(null, "required", ValidationErrorMessages.REQUIRED);
//				Add("", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(" ", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(new('a', 101), "max 100 characters",
//					ValidationErrorMessages.TextMaxLength(100));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidLineData))]
//		public void ShouldInvalidateByBadLine(
//			string line, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.Line);
//			var address = AddressValidatorFixture.MockAddress();
//			address.Line = line;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region Invalid property - Line2

//		private class InvalidLine2Data : TheoryData<string, string, string>
//		{
//			public InvalidLine2Data()
//			{
//				Add("", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(" ", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(new('a', 101), "max 100 characters",
//					ValidationErrorMessages.TextMaxLength(100));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidLine2Data))]
//		public void ShouldInvalidateByBadLine2(
//			string line2, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.Line2);
//			var address = AddressValidatorFixture.MockAddress();
//			address.Line2 = line2;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region Invalid property - Type

//		private class InvalidTypeData : TheoryData<AddressType, string, string>
//		{
//			public InvalidTypeData()
//			{
//				Add(0, "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
//				Add((AddressType)666, "unknown type", ValidationErrorMessages.ENUM_TYPE_UNKNOWN);
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidTypeData))]
//		public void ShouldInvalidateByBadType(
//			AddressType type, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.Type);
//			var address = AddressValidatorFixture.MockAddress();
//			address.Type = type;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region Invalid property - City

//		private class InvalidCityData : TheoryData<string, string, string>
//		{
//			public InvalidCityData()
//			{
//				Add(null, "required", ValidationErrorMessages.REQUIRED);
//				Add("", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(" ", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(new('a', 51), "max 50 characters", ValidationErrorMessages.TextMaxLength(50));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidCityData))]
//		public void ShouldInvalidateByBadCity(
//			string city, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.City);
//			var address = AddressValidatorFixture.MockAddress();
//			address.City = city;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region Invalid property - PostalCode

//		private class InvalidPostalCodeData : TheoryData<string, string, string>
//		{
//			public InvalidPostalCodeData()
//			{
//				Add(null, "required", ValidationErrorMessages.REQUIRED);
//				Add("", "cannot be empty or contain whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
//				Add(" ", "cannot be empty or contain whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE);
//				Add(new('a', 7), "max 6 characters", ValidationErrorMessages.TextMaxLength(6));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidPostalCodeData))]
//		public void ShouldInvalidateByBadPostalCode(
//			string postalCode, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.PostalCode);
//			var address = AddressValidatorFixture.MockAddress();
//			address.PostalCode = postalCode;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region Invalid property - State

//		private class InvalidStateData : TheoryData<string, string, string>
//		{
//			public InvalidStateData()
//			{
//				Add(null, "required", ValidationErrorMessages.REQUIRED);
//				Add("", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(" ", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(new('a', 21), "max 20 characters", ValidationErrorMessages.TextMaxLength(20));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidStateData))]
//		public void ShouldInvalidateByBadState(
//			string state, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.State);
//			var address = AddressValidatorFixture.MockAddress();
//			address.State = state;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region Invalid property - Country

//		private class InvalidCountryData : TheoryData<string, string, string>
//		{
//			public InvalidCountryData()
//			{
//				Add(null, "required", ValidationErrorMessages.REQUIRED);
//				Add("", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(" ", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add("Japan", "allowed only 'United States', 'Canada'",
//					ValidationErrorMessages.TextAllowedValues(new[] { "United States", "Canada" }));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidCountryData))]
//		public void ShouldInvalidateByBadCountry(
//			string country, (string expected, string confirm) errorMessage)
//		{
//			// Given
//			var propertyName = nameof(Address.Country);
//			var address = AddressValidatorFixture.MockAddress();
//			address.Country = country;

//			// When
//			var errors = _addressValidator.Validate(address, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region All properties

//		[Fact]
//		public void ShouldValidateFullAddressWithOptionalPropertiesNotNull()
//		{
//			// Given
//			var address = AddressValidatorFixture.MockAddress();

//			Assert.NotNull(address.Line2);

//			// When
//			var result = _addressValidator.Validate(address);

//			// Then
//			Assert.True(result.IsValid);
//		}

//		[Fact]
//		public void ShouldValidateFullAddressWithOptionalPropertiesNull()
//		{
//			// Given
//			var address = AddressValidatorFixture.MockOptionalAddress();

//			Assert.Null(address.Line2);

//			// When
//			var result = _addressValidator.Validate(address);

//			// Then
//			Assert.True(result.IsValid);
//		}

//		[Fact]
//		public void ShouldInvalidateFullAddress()
//		{
//			// Given
//			var address = AddressValidatorFixture.MockInvalidAddress();

//			// When
//			var errors = _addressValidator.Validate(address).Errors;

//			// Then
//			Assert.Equal(7, errors.Count);

//			AssertContainPropertyNames(errors, new[]
//			{
//				nameof(Address.Line),
//				nameof(Address.Line2),
//				nameof(Address.Type),
//				nameof(Address.City),
//				nameof(Address.PostalCode),
//				nameof(Address.State),
//				nameof(Address.Country)
//			});
//		}

//		#endregion
//	}

//	public class AddressValidatorFixture
//	{
//		/// <returns>The mocked address with valid properties 
//		/// (according to <see cref="AddressValidator"/>), optional properties not null.</returns>
//		public static Address MockAddress() => new()
//		{
//			Line = "line",
//			Line2 = "line2",
//			Type = AddressType.Shipping,
//			City = "city",
//			PostalCode = "code",
//			State = "state",
//			Country = "United States"
//		};

//		/// <returns>The mocked address with invalid properties 
//		/// (according to <see cref="AddressValidator"/>).</returns>
//		public static Address MockInvalidAddress() => new()
//		{
//			Line = null,
//			Line2 = "",
//			Type = 0,
//			City = null,
//			PostalCode = null,
//			State = null,
//			Country = null,
//		};

//		/// <returns>The mocked address with valid properties 
//		/// (according to <see cref="AddressValidator"/>), optional properties null.</returns>
//		public static Address MockOptionalAddress() => new()
//		{
//			Line = "line",
//			Line2 = null,
//			Type = AddressType.Shipping,
//			City = "city",
//			PostalCode = "code",
//			State = "state",
//			Country = "United States"
//		};
//	}
//}
