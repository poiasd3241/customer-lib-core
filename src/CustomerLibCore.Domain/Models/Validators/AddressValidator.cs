using CustomerLibCore.Domain.Enums;
using FluentValidation;

namespace CustomerLibCore.Domain.Models.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="Address"/> objects.
	/// </summary>
	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			Include(new AddressDetailsValidator<AddressType>());
		}
	}
}
