using System.Collections.Generic;
using AutoMapper;
using CustomerLibCore.Api.Controllers;
using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.ServiceLayer.Services;
using CustomerLibCore.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerLibCore.Api.Tests.Controllers
{
	public class NotesControllerTest
	{
		#region Constructors

		[Fact]
		public void ShouldCreateNotesController()
		{
			var mockNoteService = new StrictMock<INoteService>();
			var mockMapper = new StrictMock<IMapper>();

			var controller = new NotesController(mockNoteService.Object, mockMapper.Object);

			Assert.NotNull(controller);
		}

		#endregion

		#region Find all by customer ID

		[Fact]
		public void ShouldThrowOnFindAllForCustomerWhenProvidedBadId()
		{
			// Given
			var customerId = 0;

			var controller = new NotesControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.FindAllForCustomer(customerId));

			// Then
			Assert.Equal("customerId", exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldFindAllForCustomerEmtpy()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var notes = new List<Note>();
			var notesDto = mapper.Map<IEnumerable<NoteDto>>(notes);

			var fixture = new NotesControllerFixture();
			fixture.MockNoteService.Setup(s => s.FindAllForCustomer(customerId)).Returns(notes);
			fixture.MockMapper.Setup(m => m.Map<IEnumerable<NoteDto>>(notes)).Returns(notesDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.FindAllForCustomer(customerId).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var items = Assert.IsAssignableFrom<IEnumerable<NoteDto>>(value);
			Assert.Empty(items);

			fixture.MockNoteService.Verify(s => s.FindAllForCustomer(customerId), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<IEnumerable<NoteDto>>(notes), Times.Once);
		}

		[Fact]
		public void ShouldFindAllForCustomer()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var notes = new List<Note>() { MockNote(7, customerId), MockNote(9, customerId) };
			var notesDto = mapper.Map<IEnumerable<NoteDto>>(notes);

			var fixture = new NotesControllerFixture();
			fixture.MockNoteService.Setup(s => s.FindAllForCustomer(customerId)).Returns(notes);
			fixture.MockMapper.Setup(m => m.Map<IEnumerable<NoteDto>>(notes)).Returns(notesDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.FindAllForCustomer(customerId).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var items = Assert.IsAssignableFrom<IEnumerable<NoteDto>>(value);
			Assert.Equal(notesDto, items);

			fixture.MockNoteService.Verify(s => s.FindAllForCustomer(customerId), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<IEnumerable<NoteDto>>(notes), Times.Once);
		}

		#endregion

		#region Get single for customer

		[Theory]
		[InlineData(0, 1, "customerId")]
		[InlineData(1, 0, "noteId")]
		public void ShouldThrowOnGetSingleForCustomerWhenProvidedBadIds(
			int customerId, int noteId, string paramName)
		{
			// Given
			var controller = new NotesControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.GetForCustomer(customerId, noteId));

			// Then
			Assert.Equal(paramName, exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldGetSingleForCustomer()
		{
			// Given
			var mapper = CreateMapper();
			var noteId = 3;
			var customerId = 5;

			var note = MockNote(noteId, customerId);
			var noteDto = mapper.Map<NoteDto>(note);

			var fixture = new NotesControllerFixture();
			fixture.MockNoteService.Setup(s => s.GetForCustomer(noteId, customerId)).Returns(note);
			fixture.MockMapper.Setup(m => m.Map<NoteDto>(note)).Returns(noteDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.GetForCustomer(customerId, noteId).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var item = Assert.IsAssignableFrom<NoteDto>(value);
			Assert.Equal(noteDto, item);

			fixture.MockNoteService.Verify(s => s.GetForCustomer(noteId, customerId), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<NoteDto>(note), Times.Once);
		}

		#endregion

		#region Save

		[Fact]
		public void ShouldThrowOnSaveWhenProvidedBadId()
		{
			// Given
			var customerId = 0;
			var noteDto = new NoteDto();

			var controller = new NotesControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.Save(customerId, noteDto));

			// Then
			Assert.Equal("customerId", exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldThrowOnSaveWhenProvidedInvalidBody()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var badNoteDto = MockNoteDto();
			badNoteDto.Content = null;

			var controller = new NotesControllerFixture().CreateController();

			// When
			var errors = Assert.Throws<InvalidBodyException>(() =>
				controller.Save(customerId, badNoteDto)).Errors;

			//Then
			var error = Assert.Single(errors);

			Assert.Equal(nameof(NoteDto.Content), error.PropertyName);
		}

		[Fact]
		public void ShouldSave()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var noteDto = MockNoteDto();
			var note = mapper.Map<Note>(noteDto);

			var fixture = new NotesControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Note>(noteDto)).Returns(note);
			fixture.MockNoteService.Setup(s => s.Save(note));

			var controller = fixture.CreateController();

			// When
			var result = controller.Save(customerId, noteDto);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockNoteService.Verify(s => s.Save(note), Times.Once);
			fixture.MockNoteService.Verify(s =>
				s.Save(It.Is<Note>(n => n.CustomerId == customerId)), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<Note>(noteDto), Times.Once);
		}

		#endregion

		#region Update

		[Theory]
		[InlineData(0, 1, "customerId")]
		[InlineData(1, 0, "noteId")]
		public void ShouldThrowOnUpdateWhenProvidedBadIds(
			int customerId, int noteId, string paramName)
		{
			// Given
			var noteDto = new NoteDto();

			var controller = new NotesControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.Update(customerId, noteId, noteDto));

			// Then
			Assert.Equal(paramName, exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldThrowOnUpdateWhenProvidedInvalidBody()
		{
			// Given
			var mapper = CreateMapper();
			var noteId = 3;
			var customerId = 5;

			var badNoteDto = MockNoteDto();
			badNoteDto.Content = null;

			var controller = new NotesControllerFixture().CreateController();

			// When
			var errors = Assert.Throws<InvalidBodyException>(() =>
				controller.Update(customerId, noteId, badNoteDto)).Errors;

			//Then
			var error = Assert.Single(errors);

			Assert.Equal(nameof(NoteDto.Content), error.PropertyName);
		}

		[Fact]
		public void ShouldUpdate()
		{
			// Given
			var mapper = CreateMapper();
			var noteId = 3;
			var customerId = 5;

			var noteDto = MockNoteDto();
			var note = mapper.Map<Note>(noteDto);

			var fixture = new NotesControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Note>(noteDto)).Returns(note);
			fixture.MockNoteService.Setup(s => s.UpdateForCustomer(note));

			var controller = fixture.CreateController();

			// When
			var result = controller.Update(customerId, noteId, noteDto);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockNoteService.Verify(s => s.UpdateForCustomer(note), Times.Once);
			fixture.MockNoteService.Verify(s => s.UpdateForCustomer(It.Is<Note>(n =>
				n.NoteId == noteId && n.CustomerId == customerId)), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<Note>(noteDto), Times.Once);
		}

		#endregion

		#region Delete

		[Theory]
		[InlineData(0, 1, "customerId")]
		[InlineData(1, 0, "noteId")]
		public void ShouldThrowOnDeleteWhenProvidedBadIds(
			int customerId, int noteId, string paramName)
		{
			// Given
			var controller = new NotesControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.Delete(customerId, noteId));

			// Then
			Assert.Equal(paramName, exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldDelete()
		{
			// Given
			var noteId = 3;
			var customerId = 5;

			var fixture = new NotesControllerFixture();
			fixture.MockNoteService.Setup(s => s.DeleteForCustomer(noteId, customerId));

			var controller = fixture.CreateController();

			// When
			var result = controller.Delete(customerId, noteId);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockNoteService.Verify(s => s.DeleteForCustomer(noteId, customerId),
				Times.Once);
		}

		#endregion

		#region Fixture, object mock helpers

		public class NotesControllerFixture
		{
			public StrictMock<INoteService> MockNoteService { get; set; }
			public StrictMock<IMapper> MockMapper { get; set; }

			public NotesControllerFixture()
			{
				MockNoteService = new();
				MockMapper = new();
			}

			public NotesController CreateController() =>
				new(MockNoteService.Object, MockMapper.Object);
		}

		public static IMapper CreateMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AutoMapperApiProfile());
			});

			return config.CreateMapper();
		}

		public static Note MockNote(int noteId, int customerId) => new()
		{
			NoteId = noteId,
			CustomerId = customerId,
			Content = "MockNote content"
		};

		public static NoteDto MockNoteDto() => new()
		{
			Content = "MockNoteDto content"
		};

		#endregion
	}
}
