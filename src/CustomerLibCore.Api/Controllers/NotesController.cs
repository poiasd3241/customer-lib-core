using AutoMapper;
using CustomerLibCore.Api.Dtos.Notes.Response;
using CustomerLibCore.Api.Dtos.Validators.Notes.Request;
using CustomerLibCore.Api.Dtos.Validators.Notes.Response;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerLibCore.Api.Controllers
{
	[Route("api/customers/{customerId:int}/notes")]
	[ApiController]
	public class NotesController : ControllerBase
	{
		private readonly NoteRequestValidator _requestValidator = new();
		private readonly NoteListResponseValidator _listResponseValidator = new();
		private readonly NoteResponseValidator _responseValidator = new();

		private readonly INoteService _noteService;
		private readonly IMapper _mapper;

		public NotesController(INoteService noteService, IMapper mapper)
		{
			_noteService = noteService;
			_mapper = mapper;
		}

		// GET: api/customers/5/notes
		[HttpGet]
		public ActionResult<NoteListResponse> FindAllForCustomer([FromRoute] int customerId)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));

			var notes = _noteService.FindAllForCustomer(customerId);

			var response = _mapper.Map<NoteListResponse>(notes);
			response.Self = LinkHelper.Notes(customerId);

			return Ok(response);
		}

		// GET api/customers/5/notes/7
		[HttpGet("{noteId:int}")]
		public ActionResult<NoteResponse> GetForCustomer(
			[FromRoute] int customerId, [FromRoute] int noteId)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));
			CheckRouteArgument.Id(noteId, nameof(noteId));

			var note = _noteService.GetForCustomer(noteId, customerId);

			var noteResponse = _mapper.Map<NoteResponse>(note);

			//TODO: add validation

			return Ok(noteResponse);
		}

		// POST api/customers/5/notes
		[HttpPost]
		public IActionResult Save(int customerId, [FromBody] NoteRequest noteRequest)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));

			_noteRequestValidator.Validate(noteRequest).WithInvalidBodyException();

			var note = _mapper.Map<Note>(noteRequest);

			note.CustomerId = customerId;

			_noteService.Save(note);

			return Ok();
		}

		// PUT api/customers/5/notes/7
		[HttpPut("{noteId:int}")]
		public IActionResult Update([FromRoute] int customerId, [FromRoute] int noteId,
			[FromBody] NoteDto noteDto)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));
			CheckRouteArgument.Id(noteId, nameof(noteId));

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
			CheckRouteArgument.Id(customerId, nameof(customerId));
			CheckRouteArgument.Id(noteId, nameof(noteId));

			_noteService.DeleteForCustomer(noteId, customerId);

			return Ok();
		}
	}
}
