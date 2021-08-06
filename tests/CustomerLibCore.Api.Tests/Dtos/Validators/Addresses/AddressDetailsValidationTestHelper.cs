using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Addresses
{
	public class AddressDetailsValidationTestHelper
	{
		public static void AssignProperty(IDtoAddressDetails address,
		string propertyName, string propertyValue)
		{
			switch (propertyName)
			{
				case nameof(IDtoAddressDetails.Line):
					address.Line = propertyValue;
					break;
				case nameof(IDtoAddressDetails.Line2):
					address.Line2 = propertyValue;
					break;
				case nameof(IDtoAddressDetails.Type):
					address.Type = propertyValue;
					break;
				case nameof(IDtoAddressDetails.City):
					address.City = propertyValue;
					break;
				case nameof(IDtoAddressDetails.PostalCode):
					address.PostalCode = propertyValue;
					break;
				case nameof(IDtoAddressDetails.State):
					address.State = propertyValue;
					break;
				case nameof(IDtoAddressDetails.Country):
					address.Country = propertyValue;
					break;
				default:
					throw new ArgumentException("Unknown property name", propertyName);
			}
		}

		public static void AssertSinglePropertyInvalid1(IDtoAddressDetails address,
					Func<IDtoAddressDetails, IEnumerable<ValidationFailure>> errorsSource,
					string propertyName, string propertyValue,
					(string expected, string confirm) errorMessages)
		{
			AssignProperty(address, propertyName, propertyValue);

			var errors = errorsSource(address);

			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}
	}
}
