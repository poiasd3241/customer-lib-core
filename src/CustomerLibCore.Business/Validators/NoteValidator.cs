using CustomerLibCore.Domain.Models;
using FluentValidation;

namespace CustomerLibCore.Domain.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="Note"/> objects.
	/// </summary>
	public class NoteValidator : AbstractValidator<Note>
	{
		public NoteValidator()
		{
			// Content
			RuleFor(note => note.Content).NoteContent();
		}
	}
}
