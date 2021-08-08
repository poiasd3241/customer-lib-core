using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Models.Validators
{
	public class CustomerValidatorTest
	{
		#region Private members

		private static readonly CustomerValidator _validator = new();

		private static void AssertSinglePropertyInvalid(string propertyName,
		   object propertyValue, (string expected, string confirm) errorMessages)
		{
			var customer = new CustomerValidatorFixture().MockValid();

			switch (propertyName)
			{
				case nameof(Customer.FirstName):
					customer.FirstName = (string)propertyValue;
					break;
				case nameof(Customer.LastName):
					customer.LastName = (string)propertyValue;
					break;
				case nameof(Customer.PhoneNumber):
					customer.PhoneNumber = (string)propertyValue;
					break;
				case nameof(Customer.Email):
					customer.Email = (string)propertyValue;
					break;
				case nameof(Customer.Addresses):
					customer.Addresses = (IEnumerable<Address>)propertyValue;
					break;
				case nameof(Customer.Notes):
					customer.Notes = (IEnumerable<Note>)propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = _validator.ValidateProperty(customer, propertyName);

			errors.AssertSinglePropertyInvalid(propertyName,
				errorMessages);
		}

		#endregion

		#region Invalid property - First name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.FirstName))]
		public void ShouldInvalidateByBadFirstName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.FirstName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Last name

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.LastName))]
		public void ShouldInvalidateByBadLastName(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.LastName),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Phone number

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.PhoneNumber))]
		public void ShouldInvalidateByBadPhoneNumber(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.PhoneNumber),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Email

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Customer.Email))]
		public void ShouldInvalidateByBadEmail(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.Email),
				propertyValue, errorMessages);
		}

		#endregion

		#region Invalid property - Addresses [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.CollectionNotEmpty<Address>))]
		public void ShouldInvalidateByBadAddresses(
			List<Address> propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.Addresses),
				propertyValue, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadAddressesElement()
		{
			// Given
			var propertyName = nameof(Customer.Addresses);

			var (address, details) = new AddressValidatorFixture().MockInvalidWithDetails();

			var customer = new CustomerValidatorFixture().MockValid();
			customer.Addresses = new[] { address };

			// When
			var errors = _validator.ValidateFull(customer).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Invalid property - Notes [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.CollectionNotEmpty<Note>))]
		public void ShouldInvalidateByBadNotes(
			List<Note> propertyValue, (string expected, string confirm) errorMessages)
		{
			AssertSinglePropertyInvalid(nameof(Customer.Notes),
				propertyValue, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadNotesElement()
		{
			// Given
			var propertyName = nameof(Customer.Notes);

			var (note, details) = new NoteValidatorFixture().MockInvalidWithDetails();

			var customer = new CustomerValidatorFixture().MockValid();
			customer.Notes = new[] { note };

			// When
			var errors = _validator.ValidateFull(customer).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Details validation (without Addresses and Notes)

		[Fact]
		public void ShouldValidateCustomerExcludingAddressesAndNotes()
		{
			// Given
			var customer = new CustomerValidatorFixture().MockValid();
			customer.Addresses = null;
			customer.Notes = null;

			// When
			var result = _validator.ValidateDetails(customer);

			var fullErrors = _validator.ValidateFull(customer).Errors;

			// Then
			Assert.True(result.IsValid);

			Assert.Equal(2, fullErrors.Count);

			fullErrors.AssertContainPropertyNames(new[]
			{
				nameof(Customer.Addresses),
				nameof(Customer.Notes)
			});
		}

		[Fact]
		public void ShouldInvalidateCustomerExcludingAddressesAndNotes()
		{
			// Given
			var customer = new CustomerValidatorFixture().MockInvalid();

			// When
			var errors = _validator.ValidateDetails(customer).Errors;

			// Then
			Assert.Equal(4, errors.Count);

			errors.AssertContainPropertyNames(new[]
			{
				nameof(Customer.FirstName),
				nameof(Customer.LastName),
				nameof(Customer.PhoneNumber),
				nameof(Customer.Email)
			});
		}

		#endregion

		#region Full object (all RuleSets)

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNotNull()
		{
			// Given
			var customer = new CustomerValidatorFixture().MockValid();

			Assert.NotNull(customer.FirstName);
			Assert.NotNull(customer.Email);
			Assert.NotNull(customer.PhoneNumber);
			Assert.NotNull(customer.TotalPurchasesAmount);

			// When
			var result = _validator.ValidateFull(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldValidateFullObjectWithOptionalPropertiesNull()
		{
			// Given
			var customer = new CustomerValidatorFixture().MockValidOptional();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.Email);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.TotalPurchasesAmount);

			// When
			var result = _validator.ValidateFull(customer);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (customer, details) = new CustomerValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.ValidateFull(customer).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerValidatorFixture : IValidatorFixture<Customer>
	{
		/// <returns>The mocked object with valid properties,
		/// optional properties not <see langword="null"/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public Customer MockValid()
		{
			var address = new AddressValidatorFixture().MockValid();
			var note = new NoteValidatorFixture().MockValid();

			return new()
			{
				FirstName = "FirstName1",
				LastName = "LastName1",
				PhoneNumber = "+123456789",
				Email = "a@b.c",
				TotalPurchasesAmount = 123,
				Addresses = new[] { address },
				Notes = new[] { note }
			};
		}

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="Customer.FirstName"/> = "";
		/// <br/>
		/// <see cref="Customer.LastName"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="Customer.PhoneNumber"/> = "";
		/// <br/>
		/// <see cref="Customer.Email"/> = "";
		/// <br/>
		/// <see cref="Customer.Addresses"/>[0] = <see cref="AddressValidatorFixture.MockInvalid"/>;
		/// <br/>
		/// <see cref="Customer.Notes"/>[0] = <see cref="NoteValidatorFixture.MockInvalid"/>;
		/// <br/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public Customer MockInvalid()
		{
			var address = new AddressValidatorFixture().MockInvalid();
			var note = new NoteValidatorFixture().MockInvalid();

			return new()
			{
				FirstName = "",
				LastName = null,
				PhoneNumber = "",
				Email = "",
				Addresses = new[] { address },
				Notes = new[] { note }
			};
		}

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public (Customer invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			IEnumerable<(string propertyName, string errorMessage)> details = new (string, string)[]
			{
				(nameof(Customer.FirstName),
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE),
				(nameof(Customer.LastName), ValidationErrorMessages.REQUIRED),
				(nameof(Customer.PhoneNumber),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
				(nameof(Customer.Email),
					ValidationErrorMessages.TEXT_EMPTY_OR_CONTAIN_WHITESPACE),
			};

			var (_, invalidAddressDetails) = new AddressValidatorFixture().MockInvalidWithDetails();

			foreach (var detail in invalidAddressDetails)
			{
				details = details.AppendDetail($"{nameof(Customer.Addresses)}[0]", detail);
			}

			var (_, invalidNoteDetails) = new NoteValidatorFixture().MockInvalidWithDetails();

			foreach (var detail in invalidNoteDetails)
			{
				details = details.AppendDetail(
					$"{nameof(Customer.Notes)}[0]", detail);
			}

			return (MockInvalid(), details);
		}

		/// <returns>The mocked object with valid properties, 
		/// optional properties <see langword="null"/>:
		/// <br/>
		/// <see cref="Customer.FirstName"/>;
		/// <br/>
		/// <see cref="Customer.PhoneNumber"/>;
		/// <br/>
		/// <see cref="Customer.Email"/>;
		/// <br/>
		/// <see cref="Customer.TotalPurchasesAmount"/>
		/// <br/>
		/// (according to <see cref="CustomerValidator"/>).</returns>
		public Customer MockValidOptional()
		{
			var customer = MockValid();

			customer.FirstName = null;
			customer.PhoneNumber = null;
			customer.Email = null;
			customer.TotalPurchasesAmount = null;

			return customer;
		}
	}
}
