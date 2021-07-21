using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.DTOs.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerBasicDetailsDto"/> objects.
	/// </summary>
	public class CustomerBasicDetailsDtoValidator : AbstractValidator<CustomerBasicDetailsDto>
	{
		public CustomerBasicDetailsDtoValidator()
		{
			// FirstName - Optional
			RuleFor(customer => customer.FirstName).CustomerFirstName()
				.When(customer => customer.FirstName is not null);

			// LastName
			RuleFor(customer => customer.LastName).CustomerLastName();

			// PhoneNumber - Optional
			RuleFor(customer => customer.PhoneNumber).CustomerPhoneNumber()
				.When(customer => customer.PhoneNumber is not null);

			// Email - Optional
			RuleFor(customer => customer.Email).CustomerEmail()
				.When(customer => customer.Email is not null);

			// TotalPurchasesAmount - Optional
			RuleFor(customer => customer.TotalPurchasesAmount)
				.CustomerTotalPurchasesAmount()
				.When(customer => customer.TotalPurchasesAmount is not null);
		}
	}
}
