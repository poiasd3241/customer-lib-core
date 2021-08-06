using CustomerLibCore.Api.Dtos.Notes.Response;

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
}
