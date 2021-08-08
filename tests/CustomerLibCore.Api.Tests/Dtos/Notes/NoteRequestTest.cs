using CustomerLibCore.Api.Dtos.Notes.Request;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Notes
{
	public class NoteRequestTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var note = new NoteRequest();

			Assert.Null(note.Content);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			var content = "content1";

			var note = new NoteRequest();

			Assert.NotEqual(content, note.Content);

			// When
			note.Content = content;

			Assert.Equal(content, note.Content);
		}
	}
}
