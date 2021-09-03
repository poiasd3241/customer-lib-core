﻿namespace CustomerLibCore.Api.Dtos.Notes.Response
{
	public class NoteResponse : IResponse, IDtoNoteDetails
	{
		public string Self { get; set; }
		public string Content { get; set; }
	}
}