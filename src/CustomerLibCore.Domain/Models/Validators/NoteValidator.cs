using FluentValidation;

namespace CustomerLibCore.Domain.Models.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="Note"/> objects.
	/// </summary>
	public class NoteValidator : AbstractValidator<Note>
	{
		public NoteValidator()
		{
			Include(new NoteDetailsValidator());
		}
	}
}
