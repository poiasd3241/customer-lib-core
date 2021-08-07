using System;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.Data.Entities
{
	public class NoteEntity : INoteDetails, IEntity<NoteEntity>
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int NoteId { get; set; }
		public int CustomerId { get; set; }
		public string Content { get; set; }

		public NoteEntity Copy() => new()
		{
			NoteId = NoteId,
			CustomerId = CustomerId,
			Content = Content
		};

		public bool EqualsByValue(NoteEntity note2) =>
			note2 is not null &&
			NoteId == note2.NoteId &&
			CustomerId == note2.CustomerId &&
			Content == note2.Content;
	}
}
