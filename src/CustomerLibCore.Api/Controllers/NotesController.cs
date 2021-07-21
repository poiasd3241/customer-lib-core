using System.Collections.Generic;
using AutoMapper;
using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerLibCore.Api.Controllers
{
	[Route("api/customers/{customerId:int}/notes")]
	[ApiController]
	public class NotesController : ControllerBase
	{
		private readonly NoteDtoValidator _noteDtoValidator = new();

		private readonly INoteService _noteService;
		private readonly IMapper _mapper;

		public NotesController(INoteService noteService, IMapper mapper)
		{
			_noteService = noteService;
			_mapper = mapper;
		}

		// GET: api/customers/5/notes
		[HttpGet]
		public ActionResult<IEnumerable<NoteDto>> FindAllForCustomer([FromRoute] int customerId)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));

			var notes = _noteService.FindAllForCustomer(customerId);

			var notesDto = _mapper.Map<IEnumerable<NoteDto>>(notes);

			return Ok(notesDto);
		}

		// GET api/customers/5/notes/7
		[HttpGet("{noteId:int}")]
		public ActionResult<NoteDto> GetForCustomer(
			[FromRoute] int customerId, [FromRoute] int noteId)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));
			CheckRouteArgument.ValidId(noteId, nameof(noteId));

			var note = _noteService.GetForCustomer(noteId, customerId);

			var noteDto = _mapper.Map<NoteDto>(note);

			return Ok(noteDto);
		}

		// POST api/customers/5/notes
		[HttpPost]
		public IActionResult Save(int customerId, [FromBody] NoteDto noteDto)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));

			_noteDtoValidator.Validate(noteDto).WithInvalidBodyException();

			var note = _mapper.Map<Note>(noteDto);

			note.CustomerId = customerId;

			_noteService.Save(note);

			return Ok();
		}

		// PUT api/customers/5/notes/7
		[HttpPut("{noteId:int}")]
		public IActionResult Update([FromRoute] int customerId, [FromRoute] int noteId,
			[FromBody] NoteDto noteDto)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));
			CheckRouteArgument.ValidId(noteId, nameof(noteId));

			_noteDtoValidator.Validate(noteDto).WithInvalidBodyException();

			var note = _mapper.Map<Note>(noteDto);

			note.NoteId = noteId;
			note.CustomerId = customerId;

			_noteService.UpdateForCustomer(note);

			return Ok();
		}

		// DELETE api/customers/5/notes/7
		[HttpDelete("{noteId:int}")]
		public IActionResult Delete([FromRoute] int customerId, [FromRoute] int noteId)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));
			CheckRouteArgument.ValidId(noteId, nameof(noteId));

			_noteService.DeleteForCustomer(noteId, customerId);

			return Ok();
		}
	}
}
