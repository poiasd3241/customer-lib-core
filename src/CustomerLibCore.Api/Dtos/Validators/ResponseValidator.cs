using CustomerLibCore.Domain.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="IResponse"/> objects.
	/// </summary>
	public class ResponseValidator : AbstractValidator<IResponse>
	{
		public ResponseValidator()
		{
			// Self
			RuleFor(note => note.Self).Cascade(CascadeMode.Stop)
				.HrefLink();
		}
	}
}
