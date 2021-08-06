using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Request;
using CustomerLibCore.Api.Dtos.Validators.Notes.Request;
using CustomerLibCore.Domain.FluentValidation;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers.Request
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerCreateRequest"/> objects.
	/// </summary>
	public class CustomerCreateRequestValidator : AbstractValidator<CustomerCreateRequest>
	{
		public CustomerCreateRequestValidator()
		{
			Include(new DtoCustomerDetailsValidator());

			// Addresses
			RuleFor(customer => customer.Addresses).Cascade(CascadeMode.Stop)
				.NotNullCollectionWithMinCount(1)
				.ForEach(address => address.SetValidator(new AddressRequestValidator()));

			// Notes
			RuleFor(customer => customer.Notes).Cascade(CascadeMode.Stop)
				.NotNullCollectionWithMinCount(1)
				.ForEach(note => note.SetValidator(new NoteRequestValidator()));
		}
	}
}
