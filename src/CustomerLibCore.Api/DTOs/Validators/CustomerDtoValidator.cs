using CustomerLibCore.Business.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.DTOs.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="CustomerDto"/> objects.
	/// </summary>
	public class CustomerDtoValidator : AbstractValidator<CustomerDto>
	{
		public CustomerDtoValidator()
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

				// TotalPurchasesAmount - Optional
				RuleFor(customer => customer.TotalPurchasesAmount)
					.CustomerTotalPurchasesAmount()
					.When(customer => customer.TotalPurchasesAmount is not null);
			});

			// Addresses
			RuleFor(customer => customer.Addresses)
				.NotNullCollectionWithMinCount(1)
				.ForEach(address => address.SetValidator(new AddressDtoValidator()));

			// Notes
			RuleFor(customer => customer.Notes)
				.NotNullCollectionWithMinCount(1)
				.ForEach(note => note.SetValidator(new NoteDtoValidator()));
		}

		public ValidationResult ValidateWithoutAddressesAndNotes(CustomerDto customer) =>
			((IValidator<CustomerDto>)this).Validate(customer,
				options => options.IncludeRuleSets("BasicDetails"));

		public ValidationResult ValidateFull(CustomerDto customer) =>
			((IValidator<CustomerDto>)this).Validate(customer,
				options => options.IncludeAllRuleSets());
	}
}
