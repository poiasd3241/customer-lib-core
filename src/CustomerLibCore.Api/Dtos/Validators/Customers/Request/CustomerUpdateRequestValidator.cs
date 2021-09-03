using CustomerLibCore.Api.Dtos.Customers.Request;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers.Request
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerEditRequest"/> objects.
	/// </summary>
	public class CustomerEditRequestValidator : AbstractValidator<CustomerEditRequest>
	{
		public CustomerEditRequestValidator()
		{
			Include(new DtoCustomerDetailsValidator());
		}
	}
}
