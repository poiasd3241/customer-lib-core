using CustomerLibCore.Domain.FluentValidation;
using FluentValidation;

namespace CustomerLibCore.Domain.Models.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="INoteDetails"/> objects.
	/// </summary>
	public class NoteDetailsValidator : AbstractValidator<INoteDetails>
	{
		public NoteDetailsValidator()
		{
			// Content
			RuleFor(note => note.Content).Cascade(CascadeMode.Stop)
				.NoteContent();
		}
	}
}
