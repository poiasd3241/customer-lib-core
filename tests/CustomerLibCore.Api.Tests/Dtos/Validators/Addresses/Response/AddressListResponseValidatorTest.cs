using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses.Response;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Response;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
	public class AddressListResponseValidatorTest
	{
		#region Private members

		private static readonly AddressListResponseValidator _validator = new();

		#endregion

		#region Invalid property - Self

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadSelf(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(AddressResponse.Self);

			var addresses = new AddressListResponseValidatorFixture().MockValid();
			addresses.Self = propertyValue;

			// When
			var errors = _validator.ValidateProperty(addresses, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - Items [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.Required))]
		public void ShouldInvalidateByItemsNull((string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(AddressListResponse.Items);

			var addresses = new AddressListResponseValidatorFixture().MockValid();
			addresses.Items = null;

			// When
			var errors = _validator.ValidateProperty(addresses, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadItemsElement()
		{
			// Given
			var propertyName = nameof(AddressListResponse.Items);

			var (addressResponse, details) = new AddressResponseValidatorFixture()
				.MockInvalidWithDetails();

			var addresses = new AddressListResponseValidatorFixture().MockValid();
			addresses.Items = new[] { addressResponse };

			// When
			var errors = _validator.Validate(addresses).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObject()
		{
			// Given
			var addresses = new AddressListResponseValidatorFixture().MockValid();

			// When
			var result = _validator.Validate(addresses);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (addresses, details) = new AddressListResponseValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(addresses).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class AddressListResponseValidatorFixture : IValidatorFixture<AddressListResponse>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="AddressListResponseValidator"/>).</returns>
		public AddressListResponse MockValid() => new()
		{
			Self = "Self1",
			Items = new[] { new AddressResponseValidatorFixture().MockValid() }
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="AddressListResponse.Self"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="AddressListResponse.Items"/> =
		/// <see cref="AddressResponseValidatorFixture.MockInvalid"/>;
		/// <br/>
		/// (according to <see cref="AddressListResponseValidator"/>).</returns>
		public AddressListResponse MockInvalid()
		{
			var addressResponse = new AddressResponseValidatorFixture().MockInvalid();

			return new()
			{
				Self = null,
				Items = new[] { addressResponse }
			};
		}

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="AddressListResponseValidator"/>).</returns>
		public (AddressListResponse invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			IEnumerable<(string, string)> details = new (string, string)[]
			{
				(nameof(AddressListResponse.Self), ValidationErrorMessages.REQUIRED)
			};

			var (_, invalidAddressResponseDetails) = new AddressResponseValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidAddressResponseDetails)
			{
				details = details.AppendDetail($"{nameof(AddressListResponse.Items)}[0]", detail);
			}

			return (MockInvalid(), details);
		}
	}
}
