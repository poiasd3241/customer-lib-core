using CustomerLibCore.Api.Dtos.Notes;
using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes
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
