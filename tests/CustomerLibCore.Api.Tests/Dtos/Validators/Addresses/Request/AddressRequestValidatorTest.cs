using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Request;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
    public class AddressRequestValidatorTest
    {
        #region Private members

        private static readonly AddressRequestValidator _validator = new();

        private static void AssertSinglePropertyInvalid(string propertyName,
           string propertyValue, (string expected, string confirm) errorMessages)
        {
            var address = new AddressRequestValidatorFixture().MockValid();

            switch (propertyName)
            {
                case nameof(AddressRequest.Line):
                    address.Line = propertyValue;
                    break;
                case nameof(AddressRequest.Line2):
                    address.Line2 = propertyValue;
                    break;
                case nameof(AddressRequest.Type):
                    address.Type = propertyValue;
                    break;
                case nameof(AddressRequest.City):
                    address.City = propertyValue;
                    break;
                case nameof(AddressRequest.PostalCode):
                    address.PostalCode = propertyValue;
                    break;
                case nameof(AddressRequest.State):
                    address.State = propertyValue;
                    break;
                case nameof(AddressRequest.Country):
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

        #region Invalid property - Line

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line))]
        public void ShouldInvalidateByBadLine(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.Line),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - Line2

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
        public void ShouldInvalidateByBadLine2(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.Line2),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - Type

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.TypeText))]
        public void ShouldInvalidateByBadType(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.Type),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - City

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
        public void ShouldInvalidateByBadCity(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.City),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - PostalCode

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
        public void ShouldInvalidateByBadPostalCode(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.PostalCode),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - State

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
        public void ShouldInvalidateByBadState(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.State),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - Country

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
        public void ShouldInvalidateByBadCountry(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalid(nameof(AddressRequest.Country),
                propertyValue, errorMessages);
        }

        #endregion

        #region Full object

        [Fact]
        public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
        {
            // Given
            var address = new AddressRequestValidatorFixture().MockValid();

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
            var address = new AddressRequestValidatorFixture().MockValidOptional();

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
            var (address, details) = new AddressRequestValidatorFixture()
                .MockInvalidWithDetails();

            // When
            var errors = _validator.Validate(address).Errors;

            // Then
            errors.AssertContainPropertyNamesAndErrorMessages(details);
        }

        #endregion
    }

    public class AddressRequestValidatorFixture : IValidatorFixture<AddressRequest>
    {
        /// <returns>The mocked object with valid properties,
        /// optional properties not <see langword="null"/>
        /// (according to <see cref="AddressRequestValidator"/>).</returns>
        public AddressRequest MockValid() => new()
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
        /// <see cref="AddressRequest.Line"/> = <see langword="null"/>;
        /// <br/>
        /// <see cref="AddressRequest.Line2"/> = "";
        /// <br/>
        /// <see cref="AddressRequest.Type"/> = <see langword="null"/>;
        /// <br/>
        /// <see cref="AddressRequest.City"/> = <see langword="null"/>;
        /// <br/>
        /// <see cref="AddressRequest.PostalCode"/> = <see langword="null"/>;
        /// <br/>
        /// <see cref="AddressRequest.State"/> = <see langword="null"/>;
        /// <br/>
        /// <see cref="AddressRequest.Country"/> = <see langword="null"/>;
        /// <br/>
        /// (according to <see cref="AddressRequestValidator"/>).</returns>
        public AddressRequest MockInvalid() => new()
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
        /// - invalidObject: <see cref="MockInvalid"/>;
        /// <br/>
        /// - details: values corresponding to all invalid properties of the object;
        /// <br/>
        /// (according to <see cref="AddressRequestValidator"/>).</returns>
        public (AddressRequest invalidObject,
            IEnumerable<(string propertyName, string errorMessage)> details)
            MockInvalidWithDetails()
        {
            var details = new (string, string)[]
            {
                (nameof(AddressRequest.Line), ErrorMessages.REQUIRED),
                (nameof(AddressRequest.Line2), ErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
                (nameof(AddressRequest.Type), ErrorMessages.REQUIRED),
                (nameof(AddressRequest.City), ErrorMessages.REQUIRED),
                (nameof(AddressRequest.PostalCode), ErrorMessages.REQUIRED),
                (nameof(AddressRequest.State), ErrorMessages.REQUIRED),
                (nameof(AddressRequest.Country), ErrorMessages.REQUIRED),
            };

            return (MockInvalid(), details);
        }

        /// <returns>The mocked object with valid properties, 
        /// optional properties <see langword="null"/>:
        /// <br/>
        /// <see cref="AddressRequest.Line2"/>;
        /// <br/>
        /// (according to <see cref="AddressRequestValidator"/>).</returns>
        public AddressRequest MockValidOptional()
        {
            var address = MockValid();

            address.Line2 = null;

            return address;
        }
    }
}
