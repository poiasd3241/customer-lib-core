using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerLibCore.Domain.Models
{
	[Serializable]
	public class Note : Entity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int NoteId { get; set; }
		public int CustomerId { get; set; }
		public string Content { get; set; }

		public override bool EqualsByValue(object noteToCompareTo)
		{
			if (noteToCompareTo is null)
			{
				return false;
			}

			EnsureSameEntityType(noteToCompareTo);
			var note = (Note)noteToCompareTo;

			return
				NoteId == note.NoteId &&
				CustomerId == note.CustomerId &&
				Content == note.Content;
		}

		public static bool ListsEqualByValues(IEnumerable<Note> list1, IEnumerable<Note> list2) =>
			EntitiesHelper.ListsEqualByValues(list1, list2);

		public Note Copy() => new()
		{
			NoteId = NoteId,
			CustomerId = CustomerId,
			Content = Content
		};
	}
}
