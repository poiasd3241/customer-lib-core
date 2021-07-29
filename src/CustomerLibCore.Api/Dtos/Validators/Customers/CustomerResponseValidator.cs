using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Validators.Addresses;
using CustomerLibCore.Api.Dtos.Validators.Notes;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerResponse"/> objects.
	/// </summary>
	public class CustomerResponseValidator : AbstractValidator<CustomerResponse>
	{
		public CustomerResponseValidator()
		{
			Include(new ResponseValidator());
			Include(new CustomerBasicDetailsValidator());

			// Addresses
			RuleFor(customer => customer.Addresses)
				.SetValidator(new AddressListResponseValidator());

			// Notes
			RuleFor(customer => customer.Notes)
				.SetValidator(new NoteListResponseValidator());
		}
	}
}
