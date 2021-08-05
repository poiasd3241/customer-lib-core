using CustomerLibCore.Api.Dtos.Notes.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes.Response
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
