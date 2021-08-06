using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Customers
{
	public class CustomerDetailsValidationTestHelper
	{
		public static void AssertSinglePropertyInvalid(ICustomerDetails customer,
			Func<ICustomerDetails, IEnumerable<ValidationFailure>> errorsSource,
			string propertyName, string propertyValue,
			(string expected, string confirm) errorMessages)
		{
			switch (propertyName)
			{
				case nameof(ICustomerDetails.FirstName):
					customer.FirstName = propertyValue;
					break;
				case nameof(ICustomerDetails.LastName):
					customer.LastName = propertyValue;
					break;
				case nameof(ICustomerDetails.PhoneNumber):
					customer.PhoneNumber = propertyValue;
					break;
				case nameof(ICustomerDetails.Email):
					customer.Email = propertyValue;
					break;
				case nameof(ICustomerDetails.TotalPurchasesAmount):
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
