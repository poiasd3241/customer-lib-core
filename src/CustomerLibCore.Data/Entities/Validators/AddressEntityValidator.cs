using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Models.Validators;
using FluentValidation;

namespace CustomerLibCore.Data.Entities.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="AddressEntity"/> objects.
	/// </summary>
	public class AddressEntityValidator : AbstractValidator<AddressEntity>
	{
		public AddressEntityValidator()
		{
			Include(new AddressDetailsValidator<AddressType>());
		}
	}
}
