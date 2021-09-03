using CustomerLibCore.Domain.Models;
using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.TestHelpers;
using Xunit;
using static CustomerLibCore.Data.IntegrationTests.Repositories.EF.CustomerRepositoryTest;
using CustomerLibCore.Data.Tests.Entities.Validators;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using System;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.TestHelpers.FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.TestHelpers.ValidatorTestData;
using Castle.Core.Resource;
using CustomerLibCore.Domain.FluentValidation;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class NoteRepositoryTest
	{
		#region Constructors

		[Fact]
		public void ShouldCreateNoteRepository()
		{
			var context = DbContextHelper.Context;

			var repo = new NoteRepository(context);

			Assert.NotNull(repo);
		}

		#endregion

		#region Exists

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldCheckIfNoteExistsByIdFalseWhenEmptyRepo(int noteId)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepository();

			// When
			var exists = repo.Exists(noteId);

			// Then
			Assert.False(exists);
		}

		[Theory]
		[InlineData(1, true)]
		[InlineData(2, true)]
		[InlineData(3, false)]
		public void ShouldCheckIfNoteExistsById(int noteId, bool expectedExists)
		{
			// Given
			var customerId = 1;
			var amount = 2;

			var repo = NoteRepositoryFixture.CreateEmptyRepository();

			NoteRepositoryFixture.CreateMockNote(customerId, amount);

			// When
			var exists = repo.Exists(noteId);

			// Then
			Assert.Equal(expectedExists, exists);
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(1, 2)]
		[InlineData(2, 1)]
		[InlineData(2, 2)]
		public void ShouldCheckIfNoteExistsForCustomerWhenEmptyRepo(int noteId, int customerId)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepository();

			// When
			var exists = repo.ExistsForCustomer(noteId, customerId);

			// Then
			Assert.False(exists);
		}

		[Theory]
		[InlineData(1, false, true, false)]
		[InlineData(2, false, true, false)]
		[InlineData(3, true, false, false)]
		[InlineData(4, true, false, false)]
		[InlineData(5, false, false, false)]
		public void ShouldCheckIfNoteExistsForCustomer(
			int noteId, bool expectedExistsForCustomer1,
			bool expectedExistsForCustomer2, bool expectedExistsForCustomer3)
		{
			// Given
			var notesAmount = 2;
			var customersAmount = 2;

			var customerId1 = 1;
			var customerId2 = 2;
			// This customer doesn't exist
			var customerId3 = 3;

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			// First create notes for the customerId = 2!
			NoteRepositoryFixture.CreateMockNote(customerId2, notesAmount);
			NoteRepositoryFixture.CreateMockNote(customerId1, notesAmount);

			// When
			var existsForCustomer1 = repo.ExistsForCustomer(noteId, customerId1);
			var existsForCustomer2 = repo.ExistsForCustomer(noteId, customerId2);
			var existsForCustomer3 = repo.ExistsForCustomer(noteId, customerId3);

			// Then
			Assert.Equal(expectedExistsForCustomer1, existsForCustomer1);
			Assert.Equal(expectedExistsForCustomer2, existsForCustomer2);
			Assert.Equal(expectedExistsForCustomer3, existsForCustomer3);
		}

		#endregion

		#region Create

		[Fact]
		public void ShouldThrowOnCreateWhenProvidedNullObject()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateRepository();

			// When
			var ex = Assert.Throws<ArgumentNullException>(() => repo.Create(null));

			// Then
			Assert.Equal("note", ex.ParamName);
		}

		[Fact]
		public void ShouldThrowOnCreateWhenProvidedInvalidObject()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateRepository();

			var (note, details) = new NoteEntityValidatorFixture().MockInvalidWithDetails();

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				repo.Create(note)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		[Fact]
		public void ShouldCreateNote()
		{
			// Given
			var customersAmount = 2;
			var customerId1 = 1;
			var customerId2 = 2;

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			var note = NoteRepositoryFixture.MockNote();
			note.CustomerId = customerId1;

			// When
			var createdId1 = repo.Create(note);

			note.NoteId = 0;
			note.CustomerId = customerId2;
			var createdId2 = repo.Create(note);

			note.NoteId = 0;
			note.CustomerId = customerId1;
			var createdId3 = repo.Create(note);

			// Then
			Assert.Equal(1, createdId1);
			Assert.Equal(2, createdId2);
			Assert.Equal(3, createdId3);

			Assert.True(repo.ExistsForCustomer(createdId1, customerId1));
			Assert.True(repo.ExistsForCustomer(createdId2, customerId2));
			Assert.True(repo.ExistsForCustomer(createdId3, customerId1));
		}

		#endregion

		#region Create many for customer

		[Fact]
		public void ShouldThrowOnCreateManyForCustomerWhenProvidedNullObject()
		{
			// Given
			var customerId = 1;

			var repo = NoteRepositoryFixture.CreateRepository();

			// When
			var ex = Assert.Throws<ArgumentNullException>(() =>
				repo.CreateManyForCustomer(null, customerId));

			// Then
			Assert.Equal("notes", ex.ParamName);
		}

		[Fact]
		public void ShouldThrowOnCreateManyForCustomerWhenProvidedNullObjectElement()
		{
			// Given
			var customerId = 1;

			var repo = NoteRepositoryFixture.CreateRepository();

			var note1 = NoteRepositoryFixture.MockNote();
			var notes = new NoteEntity[] { note1, null };

			// When
			var ex = Assert.Throws<ArgumentNullException>(() =>
				repo.CreateManyForCustomer(notes, customerId));

			// Then
			Assert.Equal("note", ex.ParamName);

			Assert.NotNull(note1);
		}

		[Fact]
		public void ShouldThrowOnCreateManyForCustomerWhenProvidedInvalidObjectElement()
		{
			// Given
			var customerId = 1;

			var repo = NoteRepositoryFixture.CreateRepository();

			var noteValidatorFixture = new NoteEntityValidatorFixture();

			var note1 = noteValidatorFixture.MockValid();
			var (note2, details) = noteValidatorFixture.MockInvalidWithDetails();

			var notes = new NoteEntity[] { note1, note2 };

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				repo.CreateManyForCustomer(notes, customerId)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);

			Assert.True(new NoteEntityValidator().Validate(note1).IsValid);
		}

		[Fact]
		public void ShouldThrowOnCreateManyForCustomerOnCustomerIdMismatch()
		{
			// Given
			var customerId = 5;
			var mismatchCustomerId = 8;

			var repo = NoteRepositoryFixture.CreateRepository();

			var note1 = NoteRepositoryFixture.MockNote(customerId);
			var note2 = NoteRepositoryFixture.MockNote(mismatchCustomerId);

			var notes = new NoteEntity[] { note1, note2 };

			// When
			var ex = Assert.Throws<ArgumentException>(() =>
				repo.CreateManyForCustomer(notes, customerId));

			// Then
			Assert.Equal("all items must have the same CustomerId value, customerId = 5",
				ex.Message);
			Assert.Equal("notes", ex.ParamName);

			Assert.Equal(customerId, note1.CustomerId);
			Assert.NotEqual(customerId, note2.CustomerId);
		}

		[Fact]
		public void ShouldCreateManyForCustomer()
		{
			// Given
			var customersAmount = 2;
			var customerId1 = 1;
			var customerId2 = 2;

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			var note1 = NoteRepositoryFixture.MockNote(customerId1);
			var note2 = NoteRepositoryFixture.MockNote(customerId2);

			var notes1 = new NoteEntity[] { note1 };
			var notes2 = new NoteEntity[] { note2, note2 };

			// When
			repo.CreateManyForCustomer(notes2, customerId2);
			repo.CreateManyForCustomer(notes1, customerId1);

			// Then
			Assert.Equal(2, repo.ReadManyForCustomer(customerId2).Count);
			Assert.True(repo.ExistsForCustomer(1, customerId2));
			Assert.True(repo.ExistsForCustomer(2, customerId2));

			Assert.Single(repo.ReadManyForCustomer(customerId1));
			Assert.True(repo.ExistsForCustomer(3, customerId1));
		}

		#endregion

		#region Get count for customer

		// TODO

		#endregion


		//#region Read by Id

		//[Theory]
		//[InlineData(1)]
		//[InlineData(2)]
		//public void ShouldReadNoteNullWhenEmptyRepo(int noteId)
		//{
		//	// Given
		//	var repo = NoteRepositoryFixture.CreateEmptyRepository();

		//	// When
		//	var readNote = repo.Read(noteId);

		//	// Then
		//	Assert.Null(readNote);
		//}

		//[Theory]
		//[InlineData(1, false)]
		//[InlineData(2, false)]
		//[InlineData(3, true)]
		//[InlineData(4, true)]
		//public void ShouldReadNoteNullWhenNotFound(int noteId, bool isNull)
		//{
		//	// Given
		//	var customerId1 = 1;
		//	var customerId2 = 2;
		//	var customersAmount = 2;

		//	var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

		//	NoteRepositoryFixture.CreateMockNote(customerId2);

		//	// When
		//	var readNote = repo.Read(noteId);

		//	// Then
		//	Assert.Null(readNote);
		//}

		//[Fact]
		//public void ShouldReadNote()
		//{
		//	// Given
		//	var repo = NoteRepositoryFixture.CreateEmptyRepository();
		//	var note = NoteRepositoryFixture.CreateMockNote();

		//	// When
		//	var readNote = repo.Read(1);

		//	// Then
		//	Assert.Equal(1, readNote.NoteId);
		//	Assert.Equal(note.CustomerId, readNote.CustomerId);
		//	Assert.Equal(note.Content, readNote.Content);
		//}

		//#endregion

		#region Read single for customer

		[Theory]
		[InlineData(1, 1)]
		[InlineData(1, 2)]
		[InlineData(2, 1)]
		[InlineData(2, 2)]
		public void ShouldReadSingleForCustomerNullWhenEmptyRepo(int noteId, int customerId)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepository();

			// When
			var readNote = repo.ReadForCustomer(noteId, customerId);

			// Then
			Assert.Null(readNote);
		}

		[Theory]
		// customerId = 1
		[InlineData(1, 1)]
		[InlineData(2, 1)]
		[InlineData(5, 1)]
		// customerId = 2
		[InlineData(3, 2)]
		[InlineData(4, 2)]
		[InlineData(5, 2)]
		// customerId = 3 (doesn't exist)
		[InlineData(1, 3)]
		[InlineData(2, 3)]
		[InlineData(3, 3)]
		[InlineData(4, 3)]
		[InlineData(5, 3)]
		public void ShouldReadSingleForCustomerNullWhenNotFound(int noteId, int customerId)
		{
			// Given
			var notesAmount = 2;
			var customersAmount = 2;

			var customerId1 = 1;
			var customerId2 = 2;

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			// First create notes for the customerId = 2!
			NoteRepositoryFixture.CreateMockNote(customerId2, notesAmount);
			NoteRepositoryFixture.CreateMockNote(customerId1, notesAmount);

			// When
			var readNote = repo.ReadForCustomer(noteId, customerId);

			// Then
			Assert.Null(readNote);
		}

		[Fact]
		public void ShouldReadSingleForCustomer()
		{
			// Given
			var notesAmount = 2;
			var customersAmount = 2;

			var customerId1 = 1;
			var customerId2 = 2;

			var noteId1 = 1;
			var noteId2 = 2;
			var noteId3 = 3;
			var noteId4 = 4;

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			// First create notes for the customerId = 2!
			NoteRepositoryFixture.CreateMockNote(customerId2, notesAmount);
			NoteRepositoryFixture.CreateMockNote(customerId1, notesAmount);

			// When
			var readNote1 = repo.ReadForCustomer(noteId1, customerId2);
			var readNote2 = repo.ReadForCustomer(noteId2, customerId2);
			var readNote3 = repo.ReadForCustomer(noteId3, customerId1);
			var readNote4 = repo.ReadForCustomer(noteId4, customerId1);

			// Then
			Assert.Equal(noteId1, readNote1.NoteId);
			Assert.Equal(noteId2, readNote2.NoteId);
			Assert.Equal(noteId3, readNote3.NoteId);
			Assert.Equal(noteId4, readNote4.NoteId);

			Assert.Equal(customerId2, readNote1.CustomerId);
			Assert.Equal(customerId2, readNote2.CustomerId);
			Assert.Equal(customerId1, readNote3.CustomerId);
			Assert.Equal(customerId1, readNote4.CustomerId);

			var note = NoteRepositoryFixture.MockNote();

			var readNotes = new[] { readNote1, readNote2, readNote3, readNote4 };

			foreach (var readNote in readNotes)
			{
				Assert.Equal(note.Content, readNote.Content);
			}
		}

		#endregion

		#region Read many for customer

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldReadManyForCustomerEmptyWhenEmptyRepo(int customerId)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepository();

			// When
			var readNotes = repo.ReadManyForCustomer(customerId);

			// Then
			Assert.Empty(readNotes);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		public void ShouldReadManyForCustomerEmptyWhenNotFound(int customerId)
		{
			// Given
			var existingCustomerId = 1;

			var repo = NoteRepositoryFixture.CreateEmptyRepository();
			NoteRepositoryFixture.CreateMockNote(existingCustomerId);

			// When
			var readNotes = repo.ReadManyForCustomer(customerId);

			// Then
			Assert.Empty(readNotes);

			Assert.True(repo.ExistsForCustomer(1, existingCustomerId));
		}

		[Fact]
		public void ShouldReadManyForCustomer()
		{
			// Given
			var notesAmount = 2;
			var customersAmount = 2;

			var customerId1 = 1;
			var customerId2 = 2;

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			// First create notes for the customerId = 2!
			NoteRepositoryFixture.CreateMockNote(customerId2, notesAmount);
			NoteRepositoryFixture.CreateMockNote(customerId1, notesAmount);

			// When
			var readNotes1 = repo.ReadManyForCustomer(customerId1);
			var readNotes2 = repo.ReadManyForCustomer(customerId2);

			// Then
			Assert.Equal(2, readNotes1.Count);
			Assert.Equal(2, readNotes2.Count);

			var note = NoteRepositoryFixture.MockNote();

			foreach (var readNote in readNotes1)
			{
				Assert.Equal(customerId1, readNote.CustomerId);
				Assert.Equal(note.Content, readNote.Content);
			}

			foreach (var readNote in readNotes2)
			{
				Assert.Equal(customerId2, readNote.CustomerId);
				Assert.Equal(note.Content, readNote.Content);
			}
		}

		#endregion

		#region Update

		[Fact]
		public void ShouldThrowOnUpdateWhenProvidedNullObject()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateRepository();

			// When
			var ex = Assert.Throws<ArgumentNullException>(() => repo.Update(null));

			// Then
			Assert.Equal("note", ex.ParamName);
		}

		[Fact]
		public void ShouldThrowOnUpdateWhenProvidedInvalidObject()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateRepository();

			var (note, details) = new NoteEntityValidatorFixture().MockInvalidWithDetails();

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				repo.Update(note)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		// This test guards against the update of a note that can be found using only one of the
		// two required search keys: noteId and customerId.

		[Theory]
		// customerId = 1
		[InlineData(2, 1, true)]
		[InlineData(3, 1, true)]
		// customerId = 2
		[InlineData(1, 2, true)]
		[InlineData(3, 2, true)]
		// customerId = 3 (doesn't exist)
		[InlineData(1, 3, true)]
		[InlineData(2, 3, true)]
		[InlineData(3, 3, false)]
		[InlineData(4, 3, false)]
		[InlineData(123, 3, false)]
		// customerId = 123 (doesn't exist)
		[InlineData(1, 123, true)]
		[InlineData(2, 123, true)]
		[InlineData(3, 123, false)]
		[InlineData(4, 123, false)]
		[InlineData(123, 123, false)]
		public void ShouldNotUpdateExistingNotesWhenNotFoundIncludingPartialAndFullIdMatch(
			int noteId, int customerId, bool isPartialOrFullIdMatch)
		{
			// Given
			var customersAmount = 2;
			var customerId1 = 1;
			var customerId2 = 2;

			var noteId1 = 1;
			var noteId2 = 2;

			var input = new[] { noteId, customerId };
			var existing = new[] { customerId1, customerId2, noteId1, noteId2 };
			var distinctExisting = new[] { 1, 2 };

			if (isPartialOrFullIdMatch)
			{
				AssertX.ContainsAny(input, existing, distinctExisting);
			}
			else
			{
				AssertX.DoesNotContain(input, existing, distinctExisting);
			}

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			var content1 = "Content1";
			var content2 = "Content2";
			var newContent = "NewContent";

			var note1 = NoteRepositoryFixture.MockNote(customerId1, content1);
			var note2 = NoteRepositoryFixture.MockNote(customerId2, content2);

			repo.Create(note1);
			repo.Create(note2);

			var createdNote1 = repo.ReadForCustomer(noteId1, customerId1);
			var createdNote2 = repo.ReadForCustomer(noteId2, customerId2);

			Assert.Equal(content1, createdNote1.Content);
			Assert.Equal(content2, createdNote2.Content);

			Assert.NotEqual(content1, content2);
			Assert.NotEqual(newContent, content1);
			Assert.NotEqual(newContent, content2);

			var nonExistingNote = NoteRepositoryFixture.MockNote(customerId);
			nonExistingNote.NoteId = noteId;
			nonExistingNote.Content = newContent;

			Assert.False(repo.ExistsForCustomer(noteId, customerId));

			// When
			repo.Update(nonExistingNote);

			// Then
			Assert.False(repo.ExistsForCustomer(noteId, customerId));

			var untouchedNote1 = repo.ReadForCustomer(noteId1, customerId1);
			var untouchedNote2 = repo.ReadForCustomer(noteId2, customerId2);

			Assert.NotEqual(untouchedNote1.Content, untouchedNote2.Content);
			Assert.NotEqual(newContent, untouchedNote1.Content);
			Assert.NotEqual(newContent, untouchedNote2.Content);

			Assert.True(untouchedNote1.EqualsByValue(createdNote1));
			Assert.True(untouchedNote2.EqualsByValue(createdNote2));
		}

		[Theory]
		// customerId = 1
		[InlineData(3, 4, 1, 2)]
		[InlineData(4, 3, 1, 2)]
		// customerId = 2
		[InlineData(1, 2, 2, 1)]
		[InlineData(2, 1, 2, 1)]
		public void ShouldUpdateNote(int noteId, int untouchedNoteId,
			int customerId, int untouchedCustomerId)
		{
			// Given
			var customersAmount = 2;
			var customerId1 = 1;
			var customerId2 = 2;
			AssertX.Unique(new[] { customerId1, customerId2 });

			var repo = NoteRepositoryFixture.CreateEmptyRepository(customersAmount);

			var noteId1 = 1;
			var noteId2 = 2;
			var noteId3 = 3;
			var noteId4 = 4;

			var content1 = "Content1";
			var content2 = "Content2";
			var content3 = "Content3";
			var content4 = "Content4";
			var newContent = "NewContent";
			AssertX.Unique(new[] { content1, content2, content3, content4, newContent });

			var createData = new[]
			{
				// First create notes for the customerId = 2!
				(noteId1, customerId2, content1),
				(noteId2, customerId2, content2),
				(noteId3, customerId1, content3),
				(noteId4, customerId1, content4)
			};

			BeforeUpdateTestAssert(createData, noteId, customerId);

			var note = repo.ReadForCustomer(noteId, customerId);
			note.Content = newContent;

			// When
			repo.Update(note);

			// Then
			var updatedNote = repo.ReadForCustomer(noteId, customerId);
			Assert.Equal(newContent, updatedNote.Content);

			AfterUpdateTestAssert(createData, noteId);
		}

		private static void BeforeUpdateTestAssert(
			(int noteId, int customerId, string content)[] createData, int noteId, int customerId)
		{
			var repo = NoteRepositoryFixture.CreateRepository();

			var isUpdateNoteIdsMatch = false;

			foreach (var item in createData)
			{
				if (item.noteId == noteId && item.customerId == customerId)
					NoteRepositoryFixture.CreateMockNote(item.customerId, content: item.content);

				var note = repo.ReadForCustomer(item.noteId, item.customerId);
				Assert.Equal(item.content, note.Content);

				if (item.noteId == noteId && item.customerId == customerId)
				{
					isUpdateNoteIdsMatch = true;
				}
			}

			Assert.True(isUpdateNoteIdsMatch);
		}
		private static void AfterUpdateTestAssert(
			(int noteId, int customerId, string content)[] createData, int noteId)
		{
			var repo = NoteRepositoryFixture.CreateRepository();

			foreach (var item in createData)
			{
				if (item.noteId == noteId)
				{
					continue;
				}

				var note = repo.ReadForCustomer(item.noteId, item.customerId);
				Assert.Equal(item.content, note.Content);
			}
		}

		//[Fact]
		//public void ShouldUpdateNote()
		//{
		//	// Given
		//	var noteId = 1;
		//	var customerId = 1;
		//	var newContent = "New content!";

		//	var repo = NoteRepositoryFixture.CreateEmptyRepository(2);
		//	NoteRepositoryFixture.CreateMockNote(customerId);

		//	var note = NoteRepositoryFixture.MockNote(customerId);
		//	note.

		//	var readNote = repo.Read(noteId);

		//	var note = readNote.Copy();
		//	var beforeUpdate = readNote.Copy();

		//	note.Content = newContent;
		//	note.CustomerId = newCustomerIdTry;

		//	// When
		//	repo.Update(note);

		//	// Then
		//	var afterUpdate = repo.Read(noteId);

		//	// CustomerId - untouched
		//	var customerIdBeforeUpdate = beforeUpdate.CustomerId;
		//	Assert.Equal(1, customerIdBeforeUpdate);
		//	Assert.NotEqual(customerIdBeforeUpdate, newCustomerIdTry);

		//	Assert.Equal(beforeUpdate.CustomerId, afterUpdate.CustomerId);

		//	// Content - updated
		//	var contentBeforeUpdate = beforeUpdate.Content;
		//	Assert.Equal("text", contentBeforeUpdate);
		//	Assert.NotEqual(contentBeforeUpdate, newContent);

		//	Assert.Equal(newContent, afterUpdate.Content);

		//	// Other properties - untouched
		//	Assert.Equal(1, beforeUpdate.NoteId);
		//	Assert.Equal(beforeUpdate.NoteId, afterUpdate.NoteId);
		//}

		//#endregion

		//#region Delete

		//[Fact]
		//public void ShouldDeleteNote()
		//{
		//	// Given
		//	var repo = NoteRepositoryFixture.CreateEmptyRepository();
		//	NoteRepositoryFixture.CreateMockNote(2);

		//	var createdNote1 = repo.Read(1);
		//	var createdNote2 = repo.Read(2);
		//	Assert.NotNull(createdNote1);
		//	Assert.NotNull(createdNote2);

		//	// When
		//	repo.Delete(1);

		//	// Then
		//	var deletedNote = repo.Read(1);
		//	var untouchedNote = repo.Read(2);

		//	Assert.Null(deletedNote);

		//	Assert.NotNull(untouchedNote);
		//	Assert.True(untouchedNote.EqualsByValue(createdNote2));
		//}

		//[Fact]
		//public void ShouldDeleteManyForCustomer()
		//{
		//	// Given
		//	var customerId1 = 1;
		//	var customerId2 = 2;

		//	var repo = NoteRepositoryFixture.CreateEmptyRepository(2);
		//	NoteRepositoryFixture.CreateMockNote(amount: 2, customerId1);
		//	NoteRepositoryFixture.CreateMockNote(amount: 3, customerId2);

		//	var readNotes1 = repo.ReadManyForCustomer(customerId1);
		//	var readNotes2 = repo.ReadManyForCustomer(customerId2);

		//	Assert.Equal(2, readNotes1.Count);
		//	Assert.Equal(3, readNotes2.Count);

		//	// When
		//	repo.DeleteManyForCustomer(customerId1);

		//	// Then
		//	var deletedNotes = repo.ReadManyForCustomer(customerId1);
		//	var untouchedNotes = repo.ReadManyForCustomer(customerId2);

		//	Assert.Empty(deletedNotes);
		//	Assert.Equal(3, untouchedNotes.Count);
		//}

		//[Fact]
		//public void ShouldDeleteAllNotes()
		//{
		//	// Given
		//	var repo = NoteRepositoryFixture.CreateEmptyRepository();
		//	NoteRepositoryFixture.CreateMockNote(2);

		//	var createdNotes = repo.ReadManyForCustomer(1);
		//	Assert.Equal(2, createdNotes.Count);

		//	// When
		//	repo.DeleteAll();

		//	// Then
		//	var deletedNotes = repo.ReadManyForCustomer(1);
		//	Assert.Empty(deletedNotes);
		//}

		#endregion

		public class NoteRepositoryFixture
		{
			private static readonly NoteEntityValidatorFixture _validatorFixture = new();

			/// <summary>
			/// Creates a note repository without modifying existing data. 
			/// </summary>
			/// <returns></returns>
			public static NoteRepository CreateRepository() => new(DbContextHelper.Context);

			/// <summary>
			/// Clears the database, then creates the specified amount of customers
			/// (<see cref="Customer.CustomerId"/> = 1 for the first customer and
			/// +1 for every next customer) and a note repository.
			/// </summary>
			/// <param name="customersAmount">The amount of customers to create.</param>
			/// <returns>The note repository.</returns>
			public static NoteRepository CreateEmptyRepository(int customersAmount = 1)
			{
				DatabaseHelper.Clear();

				CustomerRepositoryFixture.CreateMockCustomer(amount: customersAmount);

				return CreateRepository();
			}

			/// <summary>
			/// Creates the specified amount of mocked notes with valid properties 
			/// for the specified customer.
			/// </summary>
			/// <param name="amount">The amount of notes to create.</param>
			/// <param name="customerId">The Id of the customer to create notes for.</param>
			public static void CreateMockNote(int customerId, int amount = 1, string content = null)
			{
				var repo = CreateRepository();

				var note = MockNote(customerId, content);

				if (amount == 1)
				{
					repo.Create(note);
					return;
				}

				var notes = new NoteEntity[amount];
				Array.Fill(notes, note);

				repo.CreateManyForCustomer(notes, customerId);
			}

			/// <returns>The mocked note with valid properties.</returns>
			public static NoteEntity MockNote(int customerId = 1, string content = null)
			{
				var note = _validatorFixture.MockValid();
				note.CustomerId = customerId;

				if (content is not null)
				{
					note.Content = content;
					new NoteEntityValidator().Validate(note).WithInternalValidationException();
				}

				return note;
			}
		}

		public class NoteRepositoryFixtureTest
		{
			[Fact]
			public void ShouldValidateNote()
			{
				// Given
				var note = NoteRepositoryFixture.MockNote();

				// When
				var result = new NoteEntityValidator().Validate(note);

				// Then
				Assert.True(result.IsValid);
			}

			[Fact]
			public void ShouldThrowOnInvalid()
			{
				// Given
				var content = " ";

				// When
				var ex = Assert.Throws<InternalValidationException>(() =>
					NoteRepositoryFixture.MockNote(content: content));

				// Then
				var error = Assert.Single(ex.Errors);
				Assert.Equal(nameof(NoteEntity.Content), error.PropertyName);
			}
		}
	}
}
