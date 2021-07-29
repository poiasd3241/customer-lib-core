using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerPagedResponse"/> objects.
	/// </summary>
	public class CustomerPagedResponseValidator : AbstractValidator<CustomerPagedResponse>
	{
		public CustomerPagedResponseValidator()
		{
			Include(new ResponseValidator());
			Include(new PagedResourceBaseValidator());

			// Items
			RuleFor(customers => customers.Items).Cascade(CascadeMode.Stop)
				.Required()
				.ForEach(customer => customer.SetValidator(new CustomerResponseValidator()));
		}
	}
}
