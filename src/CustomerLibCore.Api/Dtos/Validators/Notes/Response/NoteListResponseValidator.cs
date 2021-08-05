using CustomerLibCore.Api.Dtos.Notes.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes.Response
{
	/// <summary>
	/// The fluent validator for <see cref="NoteListResponse"/> objects.
	/// </summary>
	public class NoteListResponseValidator : NoteListResponseBaseValidator
	{
		public NoteListResponseValidator() : base(areItemsRequired: true)
		{
		}
	}
	//public class NoteListResponseValidator : AbstractValidator<NoteListResponse>
	//{
	//	public NoteListResponseValidator(bool areItemsRequired)
	//	{
	//		Include(new ListResponseValidator<NoteResponse>(
	//			new NoteResponseValidator(), areItemsRequired));
	//	}
	//}
}
