using CustomerLibCore.Api.Dtos.Customers.Request;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers.Request
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerUpdateRequest"/> objects.
	/// </summary>
	public class CustomerUpdateRequestValidator : AbstractValidator<CustomerUpdateRequest>
	{
		public CustomerUpdateRequestValidator()
		{
			Include(new DtoCustomerDetailsValidator());
		}
	}
}
