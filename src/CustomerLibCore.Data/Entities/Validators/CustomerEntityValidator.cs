using CustomerLibCore.Domain.Models.Validators;
using FluentValidation;

namespace CustomerLibCore.Data.Entities.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerEntity"/> objects.
	/// </summary>
	public class CustomerEntityValidator : AbstractValidator<CustomerEntity>
	{
		public CustomerEntityValidator()
		{
			Include(new CustomerDetailsCoreValidator());
		}
	}
}
