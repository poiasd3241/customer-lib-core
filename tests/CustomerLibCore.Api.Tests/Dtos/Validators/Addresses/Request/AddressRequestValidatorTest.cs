using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Request;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
	public class AddressRequestValidatorTest
    {
        #region Private members

        private static readonly AddressRequestValidator _validator = new();

        private static Func<IAddressDetails, IEnumerable<ValidationFailure>>
            GetErrorsSourceFromDetails(string propertyName)
        {
            return (customer) =>
                _validator.ValidateProperty((AddressRequest)customer, propertyName);
        }

        private static void AssertSinglePropertyInvalidForDetails(string propertyName,
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            var address = new AddressRequestValidatorFixture().MockValid();

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
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.Line),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - Line2

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.Line2))]
        public void ShouldInvalidateByBadLine2(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.Line2),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - Type

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.Type))]
        public void ShouldInvalidateByBadType(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.Type),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - City

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.City))]
        public void ShouldInvalidateByBadCity(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.City),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - PostalCode

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.PostalCode))]
        public void ShouldInvalidateByBadPostalCode(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.PostalCode),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - State

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.State))]
        public void ShouldInvalidateByBadState(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.State),
                propertyValue, errorMessages);
        }

        #endregion

        #region Invalid property - Country

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Address.Country))]
        public void ShouldInvalidateByBadCountry(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            AssertSinglePropertyInvalidForDetails(nameof(AddressRequest.Country),
                propertyValue, errorMessages);
        }

        #endregion

        #region Full object

        [Fact]
        public void ShouldValidateFullObjectOptionalPropertiesNotNull()
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
        public void ShouldValidateFullObjectOptionalPropertiesNull()
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
            Assert.Equal(details.Count(), errors.Count);

            errors.AssertContainPropertyNamesAndErrorMessages(details);
        }

        #endregion
    }

    public class AddressRequestValidatorFixture : IValidatorFixture<AddressRequest>
    {
        /// <returns>The mocked object with valid properties 
        /// (according to <see cref="AddressRequestValidator"/>), optional properties not null.
        /// </returns>
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
        /// <see cref="AddressRequest.Line"/> = <see langword="null"/>,
        /// <br/>
        /// <see cref="AddressRequest.Line2"/> = "",
        /// <br/>
        /// <see cref="AddressRequest.Type"/> = <see langword="null"/>,
        /// <br/>
        /// <see cref="AddressRequest.City"/> = <see langword="null"/>,
        /// <br/>
        /// <see cref="AddressRequest.PostalCode"/> = <see langword="null"/>,
        /// <br/>
        /// <see cref="AddressRequest.State"/> = <see langword="null"/>,
        /// <br/>
        /// <see cref="AddressRequest.Country"/> = <see langword="null"/>,
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
        /// invalidObject: <see cref="MockInvalid"/>
        /// <br/>
        /// details: values corresponding to all invalid properties of the object
        /// <br/>
        /// (according to <see cref="AddressRequestValidator"/>).</returns>
        public (AddressRequest invalidObject,
            IEnumerable<(string propertyName, string errorMessage)> details)
            MockInvalidWithDetails()
        {
            var details = new (string, string)[]
            {
                (nameof(AddressRequest.Line), ValidationErrorMessages.REQUIRED),
                (nameof(AddressRequest.Line2), ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
                (nameof(AddressRequest.Type), ValidationErrorMessages.REQUIRED),
                (nameof(AddressRequest.City), ValidationErrorMessages.REQUIRED),
                (nameof(AddressRequest.PostalCode), ValidationErrorMessages.REQUIRED),
                (nameof(AddressRequest.State), ValidationErrorMessages.REQUIRED),
                (nameof(AddressRequest.Country), ValidationErrorMessages.REQUIRED),
            };

            return (MockInvalid(), details);
        }

        /// <returns>The mocked object with valid properties 
        /// (according to <see cref="AddressRequestValidator"/>), optional properties null.
        /// </returns>
        public AddressRequest MockValidOptional()
        {
            var address = MockValid();

            address.Line2 = null;

            return address;
        }
    }
}
