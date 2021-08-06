using CustomerLibCore.Domain.FluentValidation;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Domain.Models.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="Customer"/> objects.
	/// </summary>
	public class CustomerValidator : AbstractValidator<Customer>
	{
		public CustomerValidator()
		{
			RuleSet("Details", () =>
			{
				Include(new CustomerDetailsCoreValidator());

				// TotalPurchasesAmount - no validation.
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
				options => options.IncludeRuleSets("Details"));

		public ValidationResult ValidateFull(Customer customer) =>
			((IValidator<Customer>)this).Validate(customer,
				options => options.IncludeAllRuleSets());
	}
}
