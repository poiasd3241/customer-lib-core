using CustomerLibCore.Api.Dtos.Notes.Response;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Notes
{
	public class NoteResponseTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var note = new NoteResponse();

			Assert.Null(note.Self);
			Assert.Null(note.Content);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var self = "self1";
			var content = "content1";

			var note = new NoteResponse();

			Assert.NotEqual(self, note.Self);
			Assert.NotEqual(content, note.Content);

			// When
			note.Self = self;
			note.Content = content;

			// Then
			Assert.Equal(self, note.Self);
			Assert.Equal(content, note.Content);
		}
	}
}
