using CustomerLibCore.Api.Dtos.Customers;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerUpdateRequest"/> objects.
	/// </summary>
	public class CustomerUpdateRequestValidator : AbstractValidator<CustomerUpdateRequest>
	{
		public CustomerUpdateRequestValidator()
		{
			Include(new CustomerBasicDetailsValidator());
		}
	}
}
