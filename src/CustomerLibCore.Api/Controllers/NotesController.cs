using AutoMapper;
using CustomerLibCore.Api.Dtos.Notes.Request;
using CustomerLibCore.Api.Dtos.Notes.Response;
using CustomerLibCore.Api.Dtos.Validators.Notes.Request;
using CustomerLibCore.Api.Dtos.Validators.Notes.Response;
using CustomerLibCore.Api.FluentValidation;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerLibCore.Api.Controllers
{
	[Route("api/customers/{customerId:int}/notes")]
	[ApiController]
	public class NotesController : ControllerBase
	{
		#region Private Members

		private readonly INoteService _noteService;
		private readonly IMapper _mapper;

		private readonly NoteRequestValidator _requestValidator = new();
		private readonly NoteListResponseValidator _listResponseValidator = new();
		private readonly NoteResponseValidator _responseValidator = new();

		#endregion

		#region Constructor

		public NotesController(INoteService noteService, IMapper mapper)
		{
			_noteService = noteService;
			_mapper = mapper;
		}

		#endregion

		#region Public Methods

		// GET: api/customers/5/notes
		[HttpGet]
		public ActionResult<NoteListResponse> FindAllForCustomer([FromRoute] int customerId)
		{
			CheckUrlArgument.Id(customerId, nameof(customerId));

			var notes = _noteService.FindAllForCustomer(customerId);

			var response = _mapper.Map<NoteListResponse>(notes);
			_listResponseValidator.Validate(response).WithInternalValidationException();

			return Ok(response);
		}

		// GET api/customers/5/notes/7
		[HttpGet("{noteId:int}")]
		public ActionResult<NoteResponse> GetForCustomer(
			[FromRoute] int customerId, [FromRoute] int noteId)
		{
			CheckUrlArgument.Id(customerId, nameof(customerId));
			CheckUrlArgument.Id(noteId, nameof(noteId));

			var note = _noteService.GetForCustomer(noteId, customerId);

			var response = _mapper.Map<NoteResponse>(note);
			_responseValidator.Validate(response).WithInternalValidationException();

			return Ok(response);
		}

		// POST api/customers/5/notes
		[HttpPost]
		public IActionResult Create(int customerId, [FromBody] NoteRequest request)
		{
			CheckUrlArgument.Id(customerId, nameof(customerId));

			_requestValidator.Validate(request).WithInvalidBodyException();

			var note = _mapper.Map<Note>(request);
			note.CustomerId = customerId;

			_noteService.Create(note);

			return Ok();
		}

		// PUT api/customers/5/notes/7
		[HttpPut("{noteId:int}")]
		public IActionResult Edit([FromRoute] int customerId, [FromRoute] int noteId,
			[FromBody] NoteRequest request)
		{
			CheckUrlArgument.Id(customerId, nameof(customerId));
			CheckUrlArgument.Id(noteId, nameof(noteId));

			_requestValidator.Validate(request).WithInvalidBodyException();

			var note = _mapper.Map<Note>(request);
			note.CustomerId = customerId;
			note.NoteId = noteId;

			_noteService.EditForCustomer(note);

			return Ok();
		}

		// DELETE api/customers/5/notes/7
		[HttpDelete("{noteId:int}")]
		public IActionResult Delete([FromRoute] int customerId, [FromRoute] int noteId)
		{
			CheckUrlArgument.Id(customerId, nameof(customerId));
			CheckUrlArgument.Id(noteId, nameof(noteId));

			_noteService.DeleteForCustomer(noteId, customerId);

			return Ok();
		}

		#endregion
	}
}
