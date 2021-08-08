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
			Include(new CustomerDetailsCoreValidator());

			// TotalPurchasesAmount - no validation.

			RuleSet("Children", () =>
			{
				// Addresses
				RuleFor(customer => customer.Addresses).Cascade(CascadeMode.Stop)
					.NotNullCollectionWithMinCount(1)
					.ForEach(address => address.SetValidator(new AddressValidator()));

				// Notes
				RuleFor(customer => customer.Notes).Cascade(CascadeMode.Stop)
					.NotNullCollectionWithMinCount(1)
					.ForEach(note => note.SetValidator(new NoteValidator()));
			});
		}

		public ValidationResult ValidateDetails(Customer customer) =>
			Validate(customer);

		public ValidationResult ValidateFull(Customer customer) =>
			((IValidator<Customer>)this).Validate(customer,
				options => options.IncludeAllRuleSets());
	}
}
