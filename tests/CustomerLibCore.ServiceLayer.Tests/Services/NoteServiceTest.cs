//using System;
//using System.Collections.Generic;
//using CustomerLibCore.Domain.Models;
//using CustomerLibCore.Domain.Exceptions;
//using CustomerLibCore.Domain.Validators;
//using CustomerLibCore.Data.Repositories;
//using CustomerLibCore.ServiceLayer.Services.Implementations;
//using CustomerLibCore.TestHelpers;
//using Moq;
//using Xunit;
//using static CustomerLibCore.TestHelpers.FluentValidation.ValidationFailureExtensions;

//namespace CustomerLibCore.ServiceLayer.Tests.Services
//{
//	public class NoteServiceTest
//	{
//		#region Constructors

//		[Fact]
//		public void ShouldCreateNoteServiceDefault()
//		{
//			var noteService = new NoteService();

//			Assert.NotNull(noteService);
//		}

//		[Fact]
//		public void ShouldCreateNoteService()
//		{
//			var mockCustomerRepo = new StrictMock<ICustomerRepository>();
//			var mockNoteRepo = new StrictMock<INoteRepository>();

//			var noteService = new NoteService(mockCustomerRepo.Object, mockNoteRepo.Object);

//			Assert.NotNull(noteService);
//		}

//		#endregion

//		#region Save

//		[Fact]
//		public void ShouldThrowOnSaveWhenProvidedBadId()
//		{
//			// Given
//			var note = new Note() { CustomerId = 0 };
//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.Save(note));

//			// Then
//			Assert.Equal("CustomerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnSaveWhenProvidedInvalidNote()
//		{
//			// Given
//			var customerId = 1;
//			var invalidNote = new Note() { CustomerId = customerId };

//			var expectedErrors = new NoteValidator().Validate(invalidNote).Errors;

//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var actualErrors = Assert.Throws<InternalValidationException>(() =>
//				service.Save(invalidNote)).Errors;

//			// Then
//			AssertValidationFailuresEqualByPropertyNameAndErrorMessage(
//					expectedErrors, actualErrors, 1);
//			AssertContainPropertyNames(actualErrors,
//				new[] { nameof(Note.Content) });
//		}

//		[Fact]
//		public void ShouldThrowOnSaveWhenCustomerNotFound()
//		{
//			// Given
//			var note = NoteServiceFixture.MockNote();
//			var customerId = 5;
//			note.CustomerId = customerId;

//			var fixture = new NoteServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When 
//			Assert.Throws<NotFoundException>(() => service.Save(note));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldSave()
//		{
//			// Given
//			var note = NoteServiceFixture.MockNote();
//			var customerId = 5;
//			note.CustomerId = customerId;

//			var fixture = new NoteServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);
//			fixture.MockNoteRepository.Setup(r => r.Create(note)).Returns(8);

//			var service = fixture.CreateService();

//			// When
//			service.Save(note);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.Create(note), Times.Once);
//		}

//		#endregion

//		#region Get single

//		[Theory]
//		[InlineData(0, 1, "noteId")]
//		[InlineData(1, 0, "customerId")]
//		public void ShouldThrowOnGetNoteByIdForCustomerWhenProvidedBadIds(
//		int noteId, int customerId, string paramName)
//		{
//			// Given
//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.GetForCustomer(noteId, customerId));

//			// Then
//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnGetNoteByIdForCustomerWhenResourceNotFound()
//		{
//			// Given
//			var noteId = 5;
//			var customerId = 7;

//			var fixture = new NoteServiceFixture();
//			fixture.MockNoteRepository.Setup(r => r.ReadForCustomer(noteId, customerId))
//				.Returns(() => null);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.GetForCustomer(noteId, customerId));

//			// Then
//			fixture.MockNoteRepository.Verify(r => r.ReadForCustomer(noteId, customerId),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldGetNoteByIdForCustomer()
//		{
//			// Given
//			var noteId = 5;
//			var customerId = 7;
//			var expectedNote = NoteServiceFixture.MockNote();

//			var fixture = new NoteServiceFixture();
//			fixture.MockNoteRepository.Setup(r => r.ReadForCustomer(noteId, customerId))
//				.Returns(expectedNote);

//			var service = fixture.CreateService();

//			// When
//			var note = service.GetForCustomer(noteId, customerId);

//			// Then
//			Assert.Equal(expectedNote, note);
//			fixture.MockNoteRepository.Verify(r => r.ReadForCustomer(noteId, customerId),
//				Times.Once);
//		}

//		#endregion

//		#region Find all

//		[Fact]
//		public void ShouldThrowOnFindAllForCustomerWhenProvidedBadId()
//		{
//			// Given
//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() => service.FindAllForCustomer(0));

//			// Then
//			Assert.Equal("customerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnFindAllForCustomerWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new NoteServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.FindAllForCustomer(customerId));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldFindAllForCustomer()
//		{
//			// Given
//			var customerId = 5;
//			var expectedNotes = NoteServiceFixture.MockNotes();

//			var fixture = new NoteServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);

//			fixture.MockNoteRepository.Setup(r => r.ReadManyForCustomer(customerId))
//				.Returns(expectedNotes);

//			var service = fixture.CreateService();

//			// When
//			var notes = service.FindAllForCustomer(customerId);

//			// Then
//			Assert.Equal(expectedNotes, notes);

//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.ReadManyForCustomer(customerId), Times.Once);
//		}

//		#endregion

//		#region Update

//		[Theory]
//		[InlineData(0, 1, "NoteId")]
//		[InlineData(1, 0, "CustomerId")]
//		public void ShouldThrowOnUpdateForCustomerWhenProvidedBadIds(
//			int noteId, int customerId, string paramName)
//		{
//			// Given
//			var note = new Note() { NoteId = noteId, CustomerId = customerId };

//			Assert.Equal(noteId, note.NoteId);
//			Assert.Equal(customerId, note.CustomerId);

//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.UpdateForCustomer(note));

//			// Then
//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnUpdateForCustomerWhenProvidedInvalidNote()
//		{
//			var noteId = 5;
//			var customerId = 7;
//			var invalidNote = new Note() { NoteId = noteId, CustomerId = customerId };

//			var expectedErrors = new NoteValidator().Validate(invalidNote).Errors;

//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var actualErrors = Assert.Throws<InternalValidationException>(() =>
//				service.UpdateForCustomer(invalidNote)).Errors;

//			// Then
//			AssertValidationFailuresEqualByPropertyNameAndErrorMessage(
//					expectedErrors, actualErrors, 1);
//			AssertContainPropertyNames(actualErrors,
//				new[] { nameof(Note.Content) });
//		}

//		[Fact]
//		public void ShouldThrowOnUpdateForCustomerWhenResourceNotFound()
//		{
//			// Given
//			var noteId = 5;
//			var customerId = 7;

//			var note = NoteServiceFixture.MockNote();
//			note.NoteId = noteId;
//			note.CustomerId = customerId;

//			var fixture = new NoteServiceFixture();
//			fixture.MockNoteRepository.Setup(r => r.ExistsForCustomer(noteId, customerId))
//				.Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.UpdateForCustomer(note));

//			// Then
//			fixture.MockNoteRepository.Verify(r => r.ExistsForCustomer(noteId, customerId),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldUpdateForCustomer()
//		{
//			// Given
//			var noteId = 5;
//			var customerId = 7;

//			var note = NoteServiceFixture.MockNote();
//			note.NoteId = noteId;
//			note.CustomerId = customerId;

//			var fixture = new NoteServiceFixture();
//			fixture.MockNoteRepository.Setup(r => r.ExistsForCustomer(noteId, customerId))
//				.Returns(true);
//			fixture.MockNoteRepository.Setup(r => r.Update(note));

//			var service = fixture.CreateService();

//			// When
//			service.UpdateForCustomer(note);

//			// Then
//			fixture.MockNoteRepository.Verify(r => r.ExistsForCustomer(noteId, customerId),
//				Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.Update(note), Times.Once);
//		}

//		#endregion

//		#region Delete single

//		[Theory]
//		[InlineData(0, 1, "noteId")]
//		[InlineData(1, 0, "customerId")]
//		[InlineData(0, 0, "noteId")]
//		public void ShouldThrowOnDeleteForCustomerWhenProvidedBadIds(
//			int noteId, int customerId, string paramName)
//		{
//			// Given
//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.DeleteForCustomer(noteId, customerId));

//			// Then
//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnDeleteForCustomerWhenResourceNotFound()
//		{
//			// Given
//			var noteId = 5;
//			var customerId = 7;

//			var fixture = new NoteServiceFixture();
//			fixture.MockNoteRepository.Setup(r => r.ExistsForCustomer(noteId, customerId))
//				.Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.DeleteForCustomer(noteId, customerId));

//			// Then
//			fixture.MockNoteRepository.Verify(r => r.ExistsForCustomer(noteId, customerId),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldDeleteForCustomer()
//		{
//			// Given
//			var noteId = 5;
//			var customerId = 7;

//			var fixture = new NoteServiceFixture();
//			fixture.MockNoteRepository.Setup(r => r.ExistsForCustomer(noteId, customerId))
//				.Returns(true);
//			fixture.MockNoteRepository.Setup(r => r.Delete(noteId));

//			var service = fixture.CreateService();

//			// When
//			service.DeleteForCustomer(noteId, customerId);

//			// Then
//			fixture.MockNoteRepository.Verify(r => r.ExistsForCustomer(noteId, customerId),
//				Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.Delete(noteId), Times.Once);
//		}

//		#endregion

//		#region Delete all

//		[Fact]
//		public void ShouldThrowOnDeleteAllForCustomerWhenProvidedBadId()
//		{
//			// Given
//			var service = new NoteServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() => service.DeleteAllForCustomer(0));

//			// Then
//			Assert.Equal("customerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnDeleteAllForCustomerWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new NoteServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.DeleteAllForCustomer(customerId));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldDeleteAllForCustomer()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new NoteServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);

//			fixture.MockNoteRepository.Setup(r => r.DeleteManyForCustomer(customerId));

//			var service = fixture.CreateService();

//			// When
//			service.DeleteAllForCustomer(customerId);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.DeleteManyForCustomer(customerId), Times.Once);
//		}

//		#endregion
//	}

//	public class NoteServiceFixture
//	{
//		/// <returns>The mocked note with repo-relevant valid properties.</returns>
//		public static Note MockNote() => new()
//		{
//			Content = "text"
//		};

//		/// <returns>The list containing 2 mocked notes with repo-relevant properties.</returns>
//		public static List<Note> MockNotes() => new() { MockNote(), MockNote() };

//		public StrictMock<ICustomerRepository> MockCustomerRepository { get; set; }
//		public StrictMock<INoteRepository> MockNoteRepository { get; set; }

//		public NoteServiceFixture()
//		{
//			MockCustomerRepository = new();
//			MockNoteRepository = new();
//		}

//		public NoteService CreateService() =>
//			new(MockCustomerRepository.Object, MockNoteRepository.Object);
//	}
//}
