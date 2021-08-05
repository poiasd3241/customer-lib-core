using CustomerLibCore.Api.Dtos.Customers.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers.Response
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerPagedResponse"/> objects.
	/// </summary>
	public class CustomerPagedResponseValidator : AbstractValidator<CustomerPagedResponse>
	{
		public CustomerPagedResponseValidator()
		{
			Include(new PagedResourceBaseValidator());

			Include(new ListResponseValidator<CustomerResponse>(
				new CustomerResponseValidator(), true));
		}
	}
}
