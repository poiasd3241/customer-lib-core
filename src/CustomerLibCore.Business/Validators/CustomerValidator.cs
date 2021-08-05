using CustomerLibCore.Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Domain.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="Customer"/> objects.
	/// </summary>
	public class CustomerValidator : AbstractValidator<Customer>
	{
		public CustomerValidator()
		{
			RuleSet("BasicDetails", () =>
			{
				// FirstName - Optional
				RuleFor(customer => customer.FirstName).CustomerFirstName()
					.When(customer => customer.FirstName is not null);

				// LastName
				RuleFor(customer => customer.LastName).CustomerLastName();

				// PhoneNumber - Optional
				RuleFor(customer => customer.PhoneNumber).CustomerPhoneNumber()
					.When(customer => customer.PhoneNumber is not null);

				// Email - Optional
				RuleFor(customer => customer.Email).CustomerEmail()
					.When(customer => customer.Email is not null);
			});

			// Addresses
			RuleFor(customer => customer.Addresses).Cascade(CascadeMode.Stop)
				.NotNullCollectionWithMinCount(1)
				.ForEach(address => address.SetValidator(new AddressValidator()));

			// Notes
			RuleFor(customer => customer.Notes).Cascade(CascadeMode.Stop)
				.NotNullCollectionWithMinCount(1)
				.ForEach(note => note.SetValidator(new NoteValidator()));
		}

		public ValidationResult ValidateWithoutAddressesAndNotes(Customer customer) =>
			((IValidator<Customer>)this).Validate(customer,
				options => options.IncludeRuleSets("BasicDetails"));

		public ValidationResult ValidateFull(Customer customer) =>
			((IValidator<Customer>)this).Validate(customer,
				options => options.IncludeAllRuleSets());
	}
}
