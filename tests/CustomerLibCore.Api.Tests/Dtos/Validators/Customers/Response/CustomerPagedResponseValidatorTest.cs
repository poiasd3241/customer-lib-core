using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Customers.Response;
using CustomerLibCore.Api.Dtos.Validators.Customers.Response;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerPagedResponseValidatorTest
	{
		#region Private members

		private static readonly CustomerPagedResponseValidator _validator = new();

		#endregion

		#region Invalid property - Self

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadSelf(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Self);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Self = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - Page

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.PagedResource.PageByItself))]
		public void ShouldInvalidateByBadPageByItself(
			int propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Page);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Page = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.PagedResource.PageAgainstLastPage))]
		public void ShouldInvalidateByBadPageAgainstLastPage(
			(int page, int lastPage) values, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Page);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Page = values.page;
			customers.LastPage = values.lastPage;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - PageSize

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.PagedResource.PageSizeByItself))]
		public void ShouldInvalidateByBadPageSizeByItself(
			int propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.PageSize);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.PageSize = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - LastPage

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.PagedResource.LastPageByItself))]
		public void ShouldInvalidateByBadLastPageByItself(
			int propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.LastPage);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.LastPage = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - Previous

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadPreviousWhenPageRequiresLink(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Previous);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Page = 2;
			customers.Previous = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.CannotHaveValue.String))]
		public void ShouldInvalidateByBadPreviousWhenPageDoesNotAllow(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Previous);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Page = 1;
			customers.Previous = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - Next

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadNextWhenPageRequiresLink(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Next);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Page = 2;
			customers.LastPage = 3;
			customers.Next = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.CannotHaveValue.String))]
		public void ShouldInvalidateByBadNextWhenPageDoesNotAllow(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Next);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Page = 3;
			customers.LastPage = 3;
			customers.Next = propertyValue;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - Items [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.Required))]
		public void ShouldInvalidateByBadItemsNull((string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Items);

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Items = null;

			// When
			var errors = _validator.ValidateProperty(customers, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadItemsElement()
		{
			// Given
			var propertyName = nameof(CustomerPagedResponse.Items);

			var (customerResponse, details) = new CustomerResponseValidatorFixture()
				.MockInvalidWithDetails();

			var customers = new CustomerPagedResponseValidatorFixture().MockValid();
			customers.Items = new[] { customerResponse };

			// When
			var errors = _validator.Validate(customers).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObject()
		{
			// Given
			var customers = new CustomerPagedResponseValidatorFixture().MockValid();

			// When
			var result = _validator.Validate(customers);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (customers, details) = new CustomerPagedResponseValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(customers).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		[Fact]
		public void ShouldInvalidateFullObjectAlternative()
		{
			// Given
			var (customers, details) = new CustomerPagedResponseValidatorFixture()
				.MockInvalidAlternativeWithDetails();

			// When
			var errors = _validator.Validate(customers).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class CustomerPagedResponseValidatorFixture : IValidatorFixture<CustomerPagedResponse>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="CustomerPagedResponseValidator"/>).</returns>
		public CustomerPagedResponse MockValid()
		{
			var customerResponse = new CustomerResponseValidatorFixture().MockValid();

			return new()
			{
				Self = "Self1",
				Page = 2,
				PageSize = 5,
				LastPage = 3,
				Next = "Next1",
				Previous = "Previous1",
				Items = new[] { customerResponse }
			};
		}
		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerPagedResponse.Self"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="CustomerPagedResponse.Page"/> = 0;
		/// <br/>
		/// <see cref="CustomerPagedResponse.PageSize"/> = 0;
		/// <br/>
		/// <see cref="CustomerPagedResponse.LastPage"/> = 0;
		/// <br/>
		/// <see cref="CustomerPagedResponse.Items"/>[0] =
		/// <see cref="CustomerResponseValidatorFixture.MockInvalid"/>;
		/// <br/>
		/// (according to <see cref="CustomerPagedResponseValidator"/>).</returns>
		public CustomerPagedResponse MockInvalid()
		{
			var customerResponse = new CustomerResponseValidatorFixture().MockInvalid();

			return new()
			{
				Self = null,
				Page = 0,
				PageSize = 0,
				LastPage = 0,
				Next = null,
				Previous = null,
				Items = new[] { customerResponse }
			};
		}

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="CustomerPagedResponse.Previous"/> = "";
		/// <br/>
		/// <see cref="CustomerPagedResponse.Next"/> = <see langword="null"/>;
		/// <br/>
		/// This is achieved with the following properties' values:
		/// <br/>
		/// <see cref="CustomerPagedResponse.Page"/> = 1;
		/// <br/>
		/// <see cref="CustomerPagedResponse.PageSize"/> = 1;
		/// <br/>
		/// <see cref="CustomerPagedResponse.LastPage"/> = 2;
		/// <br/>
		/// (according to <see cref="CustomerPagedResponseValidator"/>).</returns>
		public CustomerPagedResponse MockInvalidAlternative()
		{
			var customer = MockValid();

			customer.Page = 1;
			customer.PageSize = 1;
			customer.LastPage = 2;

			customer.Previous = "";
			customer.Next = null;

			return customer;
		}

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="CustomerPagedResponseValidator"/>).</returns>
		public (CustomerPagedResponse invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			IEnumerable<(string propertyName, string errorMessage)> details = new (string, string)[]
			{
				(nameof(CustomerPagedResponse.Self), ValidationErrorMessages.REQUIRED),
				(nameof(CustomerPagedResponse.Page),
					ValidationErrorMessages.NumberGreaterThan(0.ToString())),
				(nameof(CustomerPagedResponse.PageSize),
					ValidationErrorMessages.NumberGreaterThan(0.ToString())),
				(nameof(CustomerPagedResponse.LastPage),
					ValidationErrorMessages.NumberGreaterThan(0.ToString())),
			};

			var (_, invalidCustomerResponseDetails) = new CustomerResponseValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidCustomerResponseDetails)
			{
				details = details.AppendDetail(
					$"{nameof(CustomerPagedResponse.Items)}[0]", detail);
			}

			return (MockInvalid(), details);
		}

		/// <returns>
		/// - invalidObject: <see cref="MockInvalidAlternative"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="CustomerPagedResponseValidator"/>).</returns>
		public (CustomerPagedResponse invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidAlternativeWithDetails()
		{
			IEnumerable<(string propertyName, string errorMessage)> details = new (string, string)[]
			{
				(nameof(CustomerPagedResponse.Previous), ValidationErrorMessages.CANNOT_HAVE_VALUE),
				(nameof(CustomerPagedResponse.Next), ValidationErrorMessages.REQUIRED)
			};

			return (MockInvalidAlternative(), details);
		}
	}
}
