using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Validators.Addresses;
using CustomerLibCore.Api.Dtos.Validators.Notes;
using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Customers
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerCreateRequest"/> objects.
	/// </summary>
	public class CustomerCreateRequestValidator : AbstractValidator<CustomerCreateRequest>
	{
		public CustomerCreateRequestValidator()
		{
			Include(new CustomerBasicDetailsValidator());

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
