using System.Collections.Generic;

namespace CustomerLibCore.Api.Dtos.Notes.Response
{
	public class NoteListResponse : IListResponse<NoteResponse>
	{
		public string Self { get; set; }
		public IEnumerable<NoteResponse> Items { get; set; }
	}
}
