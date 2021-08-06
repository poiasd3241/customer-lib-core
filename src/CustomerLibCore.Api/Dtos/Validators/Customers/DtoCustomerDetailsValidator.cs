using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers
{
	/// <summary>
	/// The fluent validator for <see cref="IDtoCustomerDetails"/> objects.
	/// </summary>
	public class DtoCustomerDetailsValidator : AbstractValidator<IDtoCustomerDetails>
	{
		public DtoCustomerDetailsValidator()
		{
			Include(new CustomerDetailsCoreValidator());

			// TotalPurchasesAmount - Optional
			RuleFor(customer => customer.TotalPurchasesAmount).Cascade(CascadeMode.Stop)
				.CustomerTotalPurchasesAmount()
					.When(customer => customer.TotalPurchasesAmount is not null);
		}
	}
}
