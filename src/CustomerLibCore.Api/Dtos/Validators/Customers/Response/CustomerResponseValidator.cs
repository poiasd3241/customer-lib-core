using CustomerLibCore.Api.Dtos.Customers.Response;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Response;
using CustomerLibCore.Api.Dtos.Validators.Notes.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers.Response
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
				.SetValidator(new AddressListResponseBaseValidator(areItemsRequired: false));
			//.SetValidator(new ListResponseValidator<AddressResponse>(
			//new AddressResponseValidator(), true));
			//.SetValidator(new AddressListResponseValidator(false));

			// Notes
			RuleFor(customer => customer.Notes)
				.SetValidator(new NoteListResponseBaseValidator(areItemsRequired: false));
			//.SetValidator(new NoteListResponseValidator(false));
		}
	}
}
