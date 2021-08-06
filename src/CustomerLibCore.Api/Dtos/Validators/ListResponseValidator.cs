using CustomerLibCore.Domain.FluentValidation;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="IResponse"/> objects.
	/// </summary>
	public class ListResponseValidator<T> : AbstractValidator<IListResponse<T>>
	{
		public ListResponseValidator(AbstractValidator<T> itemValidator,
			bool areItemsRequired)
		{
			// Self
			Include(new ResponseValidator());

			// Items
			RuleFor(r => r.Items).Cascade(CascadeMode.Stop)
				.Required()
					.When(r => areItemsRequired, ApplyConditionTo.CurrentValidator)
				.ForEach(item => item.SetValidator(itemValidator));
		}
	}
}
