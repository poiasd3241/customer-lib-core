using CustomerLibCore.Api.Dtos.Notes;
using CustomerLibCore.Business.Validators;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes
{
	/// <summary>
	/// The fluent validator for <see cref="NoteListResponse"/> objects.
	/// </summary>
	public class NoteListResponseValidator : AbstractValidator<NoteListResponse>
	{
		public NoteListResponseValidator()
		{
			Include(new ResponseValidator());

			// Items
			RuleFor(notes => notes.Items).Cascade(CascadeMode.Stop)
				.Required()
				.ForEach(note => note.SetValidator(new NoteResponseValidator()));
		}
	}
}
