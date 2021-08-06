using CustomerLibCore.Domain.Models.Validators;
using FluentValidation;

namespace CustomerLibCore.Data.Entities.Validators
{
	/// <summary>
	/// The fluent validator for <see cref="NoteEntity"/> objects.
	/// </summary>
	public class NoteEntityValidator : AbstractValidator<NoteEntity>
	{
		public NoteEntityValidator()
		{
			Include(new NoteDetailsValidator());
		}
	}
}
