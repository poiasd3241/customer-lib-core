using CustomerLibCore.Api.Dtos.Addresses;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses
{
	/// <summary>
	/// The fluent validator for <see cref="AddressRequest"/> objects.
	/// </summary>
	public class AddressRequestValidator : AbstractValidator<AddressRequest>
	{
		public AddressRequestValidator()
		{
			Include(new AddressDetailsValidator());
		}
	}
}
