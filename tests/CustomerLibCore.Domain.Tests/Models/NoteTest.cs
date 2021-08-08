﻿using CustomerLibCore.Domain.Models;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Entities
{
	public class NoteTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			Note note = new();

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

			var note = new Note();

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

		//#region Equals by value

		//[Fact]
		//public void ShouldThrowOnEqualsByValueByBadObjectType()
		//{
		//	// Given
		//	var note1 = new Note();
		//	var whatever = "whatever";

		//	// When
		//	var exception = Assert.Throws<ArgumentException>(() => note1.EqualsByValue(whatever));

		//	// Then
		//	Assert.Equal("Must use the same entity type for comparison", exception.Message);
		//}

		//[Fact]
		//public void ShouldConfirmEqualsByValue()
		//{
		//	// Given
		//	var note1 = MockNote();
		//	var note2 = MockNote();

		//	// When
		//	var equalsByValue = note1.EqualsByValue(note2);

		//	// Then
		//	Assert.True(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByNull()
		//{
		//	// Given
		//	var note1 = MockNote();
		//	Note note2 = null;

		//	// When
		//	var equalsByValue = note1.EqualsByValue(note2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByNoteId()
		//{
		//	// Given
		//	var noteId1 = 5;
		//	var noteId2 = 7;

		//	var note1 = MockNote();
		//	var note2 = MockNote();

		//	note1.NoteId = noteId1;
		//	note2.NoteId = noteId2;

		//	// When
		//	var equalsByValue = note1.EqualsByValue(note2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//#endregion

		//#region Lists equal by value

		//private class NullAndNotNullListsData : TheoryData<List<Note>, List<Note>>
		//{
		//	public NullAndNotNullListsData()
		//	{
		//		Add(null, new());
		//		Add(new(), null);
		//	}
		//}

		//[Theory]
		//[ClassData(typeof(NullAndNotNullListsData))]
		//public void ShouldRefuteListsEqualByValueByOneListNull(
		//	List<Note> list1, List<Note> list2)
		//{
		//	// When
		//	var equalByValue = Note.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.False(equalByValue);
		//}

		//[Fact]
		//public void ShouldRefuteListsEqualByValueByCountMismatch()
		//{
		//	// Given
		//	var list1 = new List<Note>();
		//	var list2 = new List<Note>() { new() };

		//	// When
		//	var equalByValue = Note.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.False(equalByValue);
		//}

		//[Fact]
		//public void ShouldConfirmListsEqualByValueByBothNull()
		//{
		//	// Given
		//	List<Note> list1 = null;
		//	List<Note> list2 = null;

		//	// When
		//	var equalByValue = Note.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.True(equalByValue);
		//}

		//private class NotNullEqualListsData : TheoryData<List<Note>, List<Note>>
		//{
		//	public NotNullEqualListsData()
		//	{
		//		Add(new(), new());

		//		Add(new() { null }, new() { null });
		//		Add(new() { MockNote() }, new() { MockNote() });
		//	}
		//}

		//[Theory]
		//[ClassData(typeof(NotNullEqualListsData))]
		//public void ShouldConfirmListsEqualNotNull(List<Note> list1, List<Note> list2)
		//{
		//	// When
		//	var equalByValue = Note.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.True(equalByValue);
		//}

		//private class NotNullNotEqualListsData : TheoryData<List<Note>, List<Note>>
		//{
		//	public NotNullNotEqualListsData()
		//	{
		//		Add(new() { null }, new() { MockNote() });
		//		Add(new() { MockNote() }, new() { null });

		//		var noteId1 = 5;
		//		var noteId2 = 7;

		//		var note1 = MockNote();
		//		var note2 = MockNote();

		//		note1.NoteId = noteId1;
		//		note2.NoteId = noteId2;

		//		Add(new() { note1 }, new() { note2 });
		//	}
		//}

		//[Theory]
		//[ClassData(typeof(NotNullNotEqualListsData))]
		//public void ShouldRefuteListsEqualNotNull(List<Note> list1, List<Note> list2)
		//{
		//	// When
		//	var equalByValue = Note.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.False(equalByValue);
		//}

		//#endregion

		//[Fact]
		//public void ShouldCopyAddress()
		//{
		//	var note = MockNote();

		//	var copy = note.Copy();

		//	Assert.NotEqual(note, copy);

		//	Assert.True(note.EqualsByValue(copy));
		//}

		//private static Note MockNote() => new()
		//{
		//	NoteId = 5,
		//	CustomerId = 8,
		//	Content = "text",
		//};
	}
}