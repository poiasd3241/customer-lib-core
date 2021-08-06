using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.Api.Dtos.Notes.Request
{
	public class NoteRequest : IDtoNoteDetails
	{
		public string Content { get; set; }
	}
}
