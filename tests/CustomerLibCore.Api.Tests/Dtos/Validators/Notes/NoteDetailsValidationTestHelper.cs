using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Notes;
using CustomerLibCore.TestHelpers.FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Notes
{
	public class NoteDetailsValidationTestHelper
	{
		public static void AssertSinglePropertyInvalid(INoteDetails address,
			Func<INoteDetails, IEnumerable<ValidationFailure>> errorsSource,
			string propertyName, string propertyValue,
			(string expected, string confirm) errorMessages)
		{
			if (propertyName == nameof(INoteDetails.Content))
			{
				address.Content = propertyValue;
			}
			else
			{
				throw new ArgumentException("Unknown property name", propertyName);
			}

			var errors = errorsSource(address);

			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}
	}
}
