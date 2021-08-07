using CustomerLibCore.Data.Entities;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Entities
{
	public class NoteEntityTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			NoteEntity note = new();

			Assert.Equal(0, note.NoteId);
			Assert.Equal(0, note.CustomerId);
			Assert.Null(note.Content);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var noteId = 1;
			var customerId = 2;
			var content = "content1";

			var note = new NoteEntity();

			Assert.NotEqual(noteId, note.NoteId);
			Assert.NotEqual(customerId, note.CustomerId);
			Assert.NotEqual(content, note.Content);

			// When
			note.NoteId = noteId;
			note.CustomerId = customerId;
			note.Content = content;

			// Then
			Assert.Equal(noteId, note.NoteId);
			Assert.Equal(customerId, note.CustomerId);
			Assert.Equal(content, note.Content);
		}

		// TODO: Copy, Equals

	}
}
