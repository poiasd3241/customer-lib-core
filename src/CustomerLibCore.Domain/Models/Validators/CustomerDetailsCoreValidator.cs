using CustomerLibCore.Domain.FluentValidation;
using FluentValidation;

namespace CustomerLibCore.Domain.Models.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="ICustomerDetails"/> objects.
	/// </summary>
	public class CustomerDetailsCoreValidator : AbstractValidator<ICustomerDetailsCore>
	{
		public CustomerDetailsCoreValidator()
		{
			// FirstName - Optional
			RuleFor(customer => customer.FirstName).Cascade(CascadeMode.Stop)
				.CustomerFirstName()
					.When(customer => customer.FirstName is not null);

			// LastName
			RuleFor(customer => customer.LastName).Cascade(CascadeMode.Stop)
				.CustomerLastName();

			// PhoneNumber - Optional
			RuleFor(customer => customer.PhoneNumber).Cascade(CascadeMode.Stop)
				.CustomerPhoneNumber()
					.When(customer => customer.PhoneNumber is not null);

			// Email - Optional
			RuleFor(customer => customer.Email).Cascade(CascadeMode.Stop)
				.CustomerEmail()
					.When(customer => customer.Email is not null);
		}
	}
}
