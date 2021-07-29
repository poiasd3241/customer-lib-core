using CustomerLibCore.Business.Entities;
using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.TestHelpers;
using Xunit;
using static CustomerLibCore.Data.IntegrationTests.Repositories.EF.CustomerRepositoryTest;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class NoteRepositoryTest
	{
		#region Constructors

		[Fact]
		public void ShouldCreateNoteRepository()
		{
			var context = new StrictMock<CustomerLibDataContext>();

			var repo = new NoteRepository(context.Object);

			Assert.NotNull(repo);
		}

		#endregion

		#region Exists

		[Theory]
		[InlineData(2, true)]
		[InlineData(3, false)]
		public void ShouldCheckIfNoteExistsById(int noteId, bool expectedExists)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			NoteRepositoryFixture.CreateMockNote(amount: 2);

			// When
			var exists = repo.Exists(noteId);

			// Then
			Assert.Equal(expectedExists, exists);
		}

		[Theory]
		[InlineData(2, 1, true)]
		[InlineData(2, 55, false)]
		[InlineData(3, 1, false)]
		[InlineData(3, 55, false)]
		public void ShouldCheckIfNoteExistsForCustomerId(
			int noteId, int customerId, bool expectedExists)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			NoteRepositoryFixture.CreateMockNote(amount: 2);

			// When
			var exists = repo.ExistsForCustomer(noteId, customerId);

			// Then
			Assert.Equal(expectedExists, exists);
		}

		#endregion

		#region Create

		[Fact]
		public void ShouldCreateNote()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();

			var note = NoteRepositoryFixture.MockNote();
			note.CustomerId = 1;

			// When
			var createdId = repo.Create(note);

			// Then
			Assert.Equal(1, createdId);
		}

		#endregion

		#region Read by Id

		[Fact]
		public void ShouldReadNoteNotFound()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();

			// When
			var readNote = repo.Read(1);

			// Then
			Assert.Null(readNote);
		}

		[Fact]
		public void ShouldReadNote()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			var note = NoteRepositoryFixture.CreateMockNote();

			// When
			var readNote = repo.Read(1);

			// Then
			Assert.Equal(1, readNote.NoteId);
			Assert.Equal(note.CustomerId, readNote.CustomerId);
			Assert.Equal(note.Content, readNote.Content);
		}

		#endregion

		#region Read by customer

		[Fact]
		public void ShouldReadSingleForCustomerNotFound()
		{
			// Given
			var noteId = 5;
			var customerId = 7;

			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();

			// When
			var readNote = repo.ReadForCustomer(noteId, customerId);

			// Then
			Assert.Null(readNote);
		}

		[Fact]
		public void ShouldReadSingleForCustomer()
		{
			// Given
			var noteId1 = 1;
			var noteId2 = 2;
			var customerId1 = 1;
			var customerId2 = 2;

			var content1 = "one";
			var content2 = "two";

			var expectedNote1 = NoteRepositoryFixture.MockNote(customerId1);
			expectedNote1.Content = content1;

			var expectedNote2 = NoteRepositoryFixture.MockNote(customerId2);
			expectedNote2.Content = content2;

			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);
			expectedNote1.NoteId = repo.Create(expectedNote1);
			expectedNote2.NoteId = repo.Create(expectedNote2);

			// When
			var readNote1 = repo.ReadForCustomer(noteId1, customerId1);
			var readNote2 = repo.ReadForCustomer(noteId2, customerId2);

			// Then
			Assert.Equal(noteId1, readNote1.NoteId);
			Assert.Equal(noteId2, readNote2.NoteId);

			Assert.True(expectedNote1.EqualsByValue(readNote1));
			Assert.True(expectedNote2.EqualsByValue(readNote2));
		}

		[Fact]
		public void ShouldReadManyForCustomer()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			var note = NoteRepositoryFixture.CreateMockNote(2);

			// When
			var readNotes = repo.ReadManyForCustomer(note.CustomerId);

			// Then
			Assert.Equal(2, readNotes.Count);

			foreach (var readNote in readNotes)
			{
				Assert.Equal(note.CustomerId, readNote.CustomerId);
				Assert.Equal(note.Content, readNote.Content);
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldReadManyForCustomerBothNotFoundAndEmpty(int customerId)
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();

			// When
			var readNotes = repo.ReadManyForCustomer(customerId);

			// Then
			Assert.Empty(readNotes);
		}

		#endregion

		#region Update

		[Fact]
		public void ShouldUpdateNoteWithoutCustomerIdChange()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);
			NoteRepositoryFixture.CreateMockNote();
			var noteId = 1;

			var newContent = "New content!";
			var newCustomerIdTry = 2;

			var readNote = repo.Read(noteId);

			var note = readNote.Copy();
			var beforeUpdate = readNote.Copy();

			note.Content = newContent;
			note.CustomerId = newCustomerIdTry;

			// When
			repo.Update(note);

			// Then
			var afterUpdate = repo.Read(noteId);

			// CustomerId - untouched
			var customerIdBeforeUpdate = beforeUpdate.CustomerId;
			Assert.Equal(1, customerIdBeforeUpdate);
			Assert.NotEqual(customerIdBeforeUpdate, newCustomerIdTry);

			Assert.Equal(beforeUpdate.CustomerId, afterUpdate.CustomerId);

			// Content - updated
			var contentBeforeUpdate = beforeUpdate.Content;
			Assert.Equal("text", contentBeforeUpdate);
			Assert.NotEqual(contentBeforeUpdate, newContent);

			Assert.Equal(newContent, afterUpdate.Content);

			// Other properties - untouched
			Assert.Equal(1, beforeUpdate.NoteId);
			Assert.Equal(beforeUpdate.NoteId, afterUpdate.NoteId);
		}

		#endregion

		#region Delete

		[Fact]
		public void ShouldDeleteNote()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			NoteRepositoryFixture.CreateMockNote(2);

			var createdNote1 = repo.Read(1);
			var createdNote2 = repo.Read(2);
			Assert.NotNull(createdNote1);
			Assert.NotNull(createdNote2);

			// When
			repo.Delete(1);

			// Then
			var deletedNote = repo.Read(1);
			var untouchedNote = repo.Read(2);

			Assert.Null(deletedNote);

			Assert.NotNull(untouchedNote);
			Assert.True(untouchedNote.EqualsByValue(createdNote2));
		}

		[Fact]
		public void ShouldDeleteNoteForCustomerId()
		{
			// Given
			var noteRepo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);

			// Create 2 notes for the customer 1
			noteRepo.Create(NoteRepositoryFixture.MockNote(1));
			noteRepo.Create(NoteRepositoryFixture.MockNote(1));

			// Create 1 note for the customer 2
			noteRepo.Create(NoteRepositoryFixture.MockNote(2));

			var createdNotes1 = noteRepo.ReadManyForCustomer(1);
			var createdNotes2 = noteRepo.ReadManyForCustomer(2);
			Assert.Equal(2, createdNotes1.Count);
			Assert.Single(createdNotes2);

			var noteId = 1;
			var customerId = 1;

			Assert.True(noteRepo.ExistsForCustomer(noteId, customerId));

			// When
			noteRepo.DeleteForCustomer(noteId, customerId);

			// Then
			var leftoverNotes = noteRepo.ReadManyForCustomer(1);
			var untouchedNotes = noteRepo.ReadManyForCustomer(2);

			var leftoverNote = Assert.Single(leftoverNotes);
			Assert.Equal(2, leftoverNote.NoteId);
			Assert.Equal(1, leftoverNote.CustomerId);

			var untouchedNote = Assert.Single(untouchedNotes);
			Assert.Equal(3, untouchedNote.NoteId);
			Assert.Equal(2, untouchedNote.CustomerId);
		}

		[Fact]
		public void ShouldNotDeleteNoteForCustomerIdWhenNotFound()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			NoteRepositoryFixture.CreateMockNote();

			var customerId = 1;
			var noteId = 666;

			var createdNote1 = repo.Read(customerId);
			Assert.NotNull(createdNote1);

			Assert.False(repo.ExistsForCustomer(noteId, customerId));

			// When
			repo.DeleteForCustomer(noteId, customerId);

			// Then
			var untouchedNote = repo.Read(1);
			Assert.True(untouchedNote.EqualsByValue(createdNote1));
		}

		[Fact]
		public void ShouldDeleteManyForCustomer()
		{
			// Given
			var customerId1 = 1;
			var customerId2 = 2;

			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);
			NoteRepositoryFixture.CreateMockNote(amount: 2, customerId1);
			NoteRepositoryFixture.CreateMockNote(amount: 3, customerId2);

			var readNotes1 = repo.ReadManyForCustomer(customerId1);
			var readNotes2 = repo.ReadManyForCustomer(customerId2);

			Assert.Equal(2, readNotes1.Count);
			Assert.Equal(3, readNotes2.Count);

			// When
			repo.DeleteManyForCustomer(customerId1);

			// Then
			var deletedNotes = repo.ReadManyForCustomer(customerId1);
			var untouchedNotes = repo.ReadManyForCustomer(customerId2);

			Assert.Empty(deletedNotes);
			Assert.Equal(3, untouchedNotes.Count);
		}

		[Fact]
		public void ShouldDeleteAllNotes()
		{
			// Given
			var repo = NoteRepositoryFixture.CreateEmptyRepositoryWithCustomer();
			NoteRepositoryFixture.CreateMockNote(2);

			var createdNotes = repo.ReadManyForCustomer(1);
			Assert.Equal(2, createdNotes.Count);

			// When
			repo.DeleteAll();

			// Then
			var deletedNotes = repo.ReadManyForCustomer(1);
			Assert.Empty(deletedNotes);
		}

		#endregion

		public class NoteRepositoryFixture
		{
			/// <summary>
			/// Creates the empty repository, containing the specified amount of customers
			/// (<see cref="Customer.CustomerId"/> = 1 for the first customer and
			/// +1 for every next customer) and no notes.
			/// </summary>
			/// <param name="customersAmount">The amount of customers to create.</param>
			/// <returns>The empty note repository.</returns>
			public static NoteRepository CreateEmptyRepositoryWithCustomer(int customersAmount = 1)
			{
				CustomerRepositoryFixture.CreateMockCustomer(amount: customersAmount);

				return new(DbContextHelper.Context);
			}

			/// <summary>
			/// Creates the specified amount of mocked notes
			/// with repo-relevant valid properties, <see cref="Note.CustomerId"/> = 1.
			/// </summary>
			/// <param name="amount">The amount of notes to create.</param>
			/// <param name="customerId">The Id of the customer for created notes.</param>
			/// <returns>The mocked note with repo-relevant valid properties, 
			/// <see cref="Note.CustomerId"/> = 1.</returns>
			public static Note CreateMockNote(int amount = 1, int customerId = 1)
			{
				var repo = new NoteRepository(DbContextHelper.Context);

				Note note;

				for (int i = 0; i < amount; i++)
				{
					note = MockNote(customerId);

					repo.Create(note);
				}

				return MockNote();
			}

			/// <returns>The mocked note with repo-relevant valid properties.</returns>
			public static Note MockNote(int customerId = 1) => new()
			{
				CustomerId = customerId,
				Content = "text"
			};
		}
	}
}
