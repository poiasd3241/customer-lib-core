using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Domain.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers
{
	/// <summary>
	/// The fluent validator for <see cref="ICustomerBasicDetails"/> objects.
	/// </summary>
	public class CustomerBasicDetailsValidator : AbstractValidator<ICustomerBasicDetails>
	{
		public CustomerBasicDetailsValidator()
		{
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

			// TotalPurchasesAmount - Optional
			RuleFor(customer => customer.TotalPurchasesAmount).Cascade(CascadeMode.Stop)
				.CustomerTotalPurchasesAmount()
					.When(customer => customer.TotalPurchasesAmount is not null);
		}
	}
}
