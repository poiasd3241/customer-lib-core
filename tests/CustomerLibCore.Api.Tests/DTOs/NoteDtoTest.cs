using CustomerLibCore.Api.DTOs;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs
{
	public class NoteDtoTest
	{
		[Fact]
		public void ShouldCreateNoteDto()
		{
			NoteDto noteDto = new();

			Assert.Null(noteDto.Content);
		}

		[Fact]
		public void ShouldSetNoteDtoProperties()
		{
			var text = "text";

			NoteDto noteDto = new();

			noteDto.Content = text;

			Assert.Equal(text, noteDto.Content);
		}
	}
}
