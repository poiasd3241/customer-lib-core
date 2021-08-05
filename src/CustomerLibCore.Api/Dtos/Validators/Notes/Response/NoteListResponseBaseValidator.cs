using CustomerLibCore.Api.Dtos.Notes.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes.Response
{
	/// <summary>
	/// The fluent validator for <see cref="NoteListResponse"/> objects.
	/// </summary>
	public class NoteListResponseBaseValidator : AbstractValidator<NoteListResponse>
	{
		public NoteListResponseBaseValidator(bool areItemsRequired)
		{
			Include(new ListResponseValidator<NoteResponse>(
				new NoteResponseValidator(), areItemsRequired));
		}
	}
}
