using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerBasicDetailsValidationTestHelper
	{
		public static void AssertSinglePropertyInvalid(ICustomerBasicDetails customer,
			Func<ICustomerBasicDetails, IEnumerable<ValidationFailure>> errorsSource,
			string propertyName, string propertyValue,
			(string expected, string confirm) errorMessages)
		{
			switch (propertyName)
			{
				case nameof(ICustomerBasicDetails.FirstName):
					customer.FirstName = propertyValue;
					break;
				case nameof(ICustomerBasicDetails.LastName):
					customer.LastName = propertyValue;
					break;
				case nameof(ICustomerBasicDetails.PhoneNumber):
					customer.PhoneNumber = propertyValue;
					break;
				case nameof(ICustomerBasicDetails.Email):
					customer.Email = propertyValue;
					break;
				case nameof(ICustomerBasicDetails.TotalPurchasesAmount):
					customer.TotalPurchasesAmount = propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = errorsSource(customer);

			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}
	}
}
