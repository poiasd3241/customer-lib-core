using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Localization;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="IResponse"/> objects.
	/// </summary>
	public class PagedResourceBaseValidator : AbstractValidator<IPagedResourceBase>
	{
		public PagedResourceBaseValidator()
		{
			// Page
			RuleFor(resource => resource.Page).Cascade(CascadeMode.Stop)
				.NumberGreaterThan(0)
				.LessThanOrEqualTo(resource => resource.LastPage).WithMessage(
					ValidationErrorMessages.NumberLessThanOrEqualToPropertyValue(
						nameof(IPagedResourceBase.LastPage)))
					.When(resource => resource.LastPage > 0, ApplyConditionTo.CurrentValidator);

			// PageSize
			RuleFor(resource => resource.PageSize).Cascade(CascadeMode.Stop)
				.NumberGreaterThan(0);

			// LastPage
			RuleFor(resource => resource.LastPage).Cascade(CascadeMode.Stop)
				.NumberGreaterThan(0);

			When(resource =>
				resource.Page > 0 &&
				resource.PageSize > 0 &&
				resource.LastPage > 0 &&
				resource.Page <= resource.LastPage, () =>
				{
					// Previous link
					RuleFor(resource => resource.Previous).Cascade(CascadeMode.Stop)
						.HrefLink()
							.When(resource => resource.Page > 1, ApplyConditionTo.CurrentValidator)
						.CannotHaveValue()
							.When(resource => resource.Page == 1,
								ApplyConditionTo.CurrentValidator);

					// Next link
					RuleFor(resource => resource.Next).Cascade(CascadeMode.Stop)
					.HrefLink()
						.When(resource => resource.Page < resource.LastPage,
							ApplyConditionTo.CurrentValidator)
					.CannotHaveValue()
						.When(resource => resource.Page == resource.LastPage,
							ApplyConditionTo.CurrentValidator);
				});
		}
	}
}
