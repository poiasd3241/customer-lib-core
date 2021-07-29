using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
	public class AddressDetailsValidationTestHelper
	{
		public static void AssertSinglePropertyInvalid(IAddressDetails address,
			Func<IAddressDetails, IEnumerable<ValidationFailure>> errorsSource,
			string propertyName, string propertyValue,
			(string expected, string confirm) errorMessages)
		{
			switch (propertyName)
			{
				case nameof(IAddressDetails.Line):
					address.Line = propertyValue;
					break;
				case nameof(IAddressDetails.Line2):
					address.Line2 = propertyValue;
					break;
				case nameof(IAddressDetails.Type):
					address.Type = propertyValue;
					break;
				case nameof(IAddressDetails.City):
					address.City = propertyValue;
					break;
				case nameof(IAddressDetails.PostalCode):
					address.PostalCode = propertyValue;
					break;
				case nameof(IAddressDetails.State):
					address.State = propertyValue;
					break;
				case nameof(IAddressDetails.Country):
					address.Country = propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = errorsSource(address);

			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}
	}
}
