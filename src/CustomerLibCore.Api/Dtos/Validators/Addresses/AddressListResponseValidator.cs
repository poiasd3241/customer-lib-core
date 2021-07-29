using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses
{
	/// <summary>
	/// The fluent validator for <see cref="AddressListResponse"/> objects.
	/// </summary>
	public class AddressListResponseValidator : AbstractValidator<AddressListResponse>
	{
		public AddressListResponseValidator()
		{
			Include(new ResponseValidator());

			// Items
			RuleFor(addresses => addresses.Items).Cascade(CascadeMode.Stop)
				.Required()
				.ForEach(address => address.SetValidator(new AddressResponseValidator()));
		}
	}
}
