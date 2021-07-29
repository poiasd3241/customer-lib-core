using CustomerLibCore.Api.Dtos.Notes;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes
{
	/// <summary>
	/// The fluent validator for <see cref="NoteResponse"/> objects.
	/// </summary>
	public class NoteResponseValidator : AbstractValidator<NoteResponse>
	{
		public NoteResponseValidator()
		{
			Include(new ResponseValidator());
			Include(new NoteDetailsValidator());
		}
	}
}
