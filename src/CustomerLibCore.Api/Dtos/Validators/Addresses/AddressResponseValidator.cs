﻿using CustomerLibCore.Api.Dtos.Addresses;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses
{
	/// <summary>
	/// The fluent validator for <see cref="AddressResponse"/> objects.
	/// </summary>
	public class AddressResponseValidator : AbstractValidator<AddressResponse>
	{
		public AddressResponseValidator()
		{
			Include(new ResponseValidator());
			Include(new AddressDetailsValidator());
		}
	}
}
