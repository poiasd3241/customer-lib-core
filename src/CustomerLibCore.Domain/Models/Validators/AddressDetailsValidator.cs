using System;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.FluentValidation;
using FluentValidation;

namespace CustomerLibCore.Domain.Models.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="IAddressDetails{TType}"/> objects.
	/// </summary>
	/// <typeparam name="TType">The type of the property <see cref="IAddressDetails{TType}.Type"/>.
	/// Must be an <see cref="AddressType"/> or a <see cref="string"/>.</typeparam>
	public class AddressDetailsValidator<TType> : AbstractValidator<IAddressDetails<TType>>
	{
		public AddressDetailsValidator()
		{
			if (typeof(TType) != typeof(AddressType) || typeof(TType) != typeof(string))
			{
				throw new Exception(
					$"The {nameof(TType)} must be either {nameof(AddressType)} or a string.");
			}

			// Line
			RuleFor(address => address.Line).Cascade(CascadeMode.Stop)
				.AddressLine();

			// Line2 - Optional
			RuleFor(address => address.Line2).Cascade(CascadeMode.Stop)
				.AddressLine2()
					.When(address => address.Line2 is not null);

			// Type
			RuleFor(address => address.Type).Cascade(CascadeMode.Stop)
				.Required()
				.AddressTypeEnum();

			// City
			RuleFor(address => address.City).Cascade(CascadeMode.Stop)
				.AddressCity();

			// PostalCode
			RuleFor(address => address.PostalCode).Cascade(CascadeMode.Stop)
				.AddressPostalCode();

			// State
			RuleFor(address => address.State).Cascade(CascadeMode.Stop)
				.AddressState();

			// Country
			RuleFor(address => address.Country).Cascade(CascadeMode.Stop)
				.AddressCountry();
		}
	}
}
