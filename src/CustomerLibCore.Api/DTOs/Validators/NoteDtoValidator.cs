using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.DTOs.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="NoteDto"/> objects.
	/// </summary>
	public class NoteDtoValidator : AbstractValidator<NoteDto>
	{
		public NoteDtoValidator()
		{
			// Content
			RuleFor(note => note.Content).NoteContent();
		}
	}
}
