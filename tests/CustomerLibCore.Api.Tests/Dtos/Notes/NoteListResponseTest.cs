using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Notes;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Notes
{
	public class NoteListResponseTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var notes = new NoteListResponse();

			Assert.Null(notes.Self);
			Assert.Null(notes.Items);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			var self = "self1";
			var items = new List<NoteResponse>();

			var notes = new NoteListResponse();

			Assert.NotEqual(self, notes.Self);
			Assert.NotEqual(items, notes.Items);

			// When
			notes.Self = self;
			notes.Items = items;

			Assert.Equal(self, notes.Self);
			Assert.Equal(items, notes.Items);
		}
	}
}
