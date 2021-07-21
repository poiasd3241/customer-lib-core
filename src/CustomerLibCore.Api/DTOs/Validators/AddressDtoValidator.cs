using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.DTOs.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="AddressDto"/> objects.
	/// </summary>
	public class AddressDtoValidator : AbstractValidator<AddressDto>
	{
		public AddressDtoValidator()
		{
			// Line
			RuleFor(address => address.Line).AddressLine();

			// Line2 - Optional
			RuleFor(address => address.Line2).AddressLine2()
				.When(address => address.Line2 is not null);

			// Type
			RuleFor(address => address.Type).Cascade(CascadeMode.Stop)
				.Required()
				.AddressTypeEnum();

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
