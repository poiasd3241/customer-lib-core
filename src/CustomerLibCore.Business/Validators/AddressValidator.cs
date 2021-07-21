using CustomerLibCore.Business.Entities;
using FluentValidation;

namespace CustomerLibCore.Business.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="Address"/> objects.
	/// </summary>
	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			// Line
			RuleFor(address => address.Line).AddressLine();

			// Line2 - Optional
			RuleFor(address => address.Line2).AddressLine2()
				.When(address => address.Line2 is not null);

			// Type
			RuleFor(address => address.Type).AddressTypeEnum();

			// City
			RuleFor(address => address.City).AddressCity();

			// PostalCode
			RuleFor(address => address.PostalCode).AddressPostalCode();

			// State
			RuleFor(address => address.State).AddressState();

			// Country
			RuleFor(address => address.Country).AddressCountry();
		}
	}
}
