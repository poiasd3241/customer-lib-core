using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.Data.Entities
{
	[Table("Notes")]
	public class NoteEntity : INoteDetails, IEntity<NoteEntity>
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int NoteId { get; set; }
		public int CustomerId { get; set; }
		public string Content { get; set; }

		public NoteEntity Copy() => new()
		{
			NoteId = NoteId,
			CustomerId = CustomerId,
			Content = Content
		};

		public bool EqualsByValueExcludingId(NoteEntity note2) =>
			note2 is not null &&
			Content == note2.Content;

		public bool EqualsByValue(NoteEntity note2) =>
			EqualsByValueExcludingId(note2) &&
			NoteId == note2.NoteId &&
			CustomerId == note2.CustomerId;
	}
}
