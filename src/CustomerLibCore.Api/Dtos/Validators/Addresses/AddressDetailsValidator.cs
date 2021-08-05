using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Domain.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses
{
	/// <summary>
	/// The fluent validator for <see cref="IAddressDetails"/> objects.
	/// </summary>
	public class AddressDetailsValidator : AbstractValidator<IAddressDetails>
	{
		public AddressDetailsValidator()
		{
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
