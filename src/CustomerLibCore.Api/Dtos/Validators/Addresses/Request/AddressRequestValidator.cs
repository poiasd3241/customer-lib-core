﻿using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Domain.Models.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses.Request
{
	/// <summary>
	/// The fluent validator for <see cref="AddressRequest"/> objects.
	/// </summary>
	public class AddressRequestValidator : AbstractValidator<AddressRequest>
	{
		public AddressRequestValidator()
		{
			Include(new AddressDetailsValidator<string>());
		}
	}
}
