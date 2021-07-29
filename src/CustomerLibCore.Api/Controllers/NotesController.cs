//using System.Collections.Generic;
//using AutoMapper;
//using CustomerLibCore.Api.Dtos;
//using CustomerLibCore.Api.Dtos.Notes;
//using CustomerLibCore.Api.Dtos.Validators;
//using CustomerLibCore.Api.Dtos.Validators.Notes;
//using CustomerLibCore.Business.Entities;
//using CustomerLibCore.Business.Validators;
//using CustomerLibCore.ServiceLayer.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Routing;

//namespace CustomerLibCore.Api.Controllers
//{
//	[Route("api/customers/{customerId:int}/notes")]
//	[ApiController]
//	public class NotesController : ControllerBase
//	{
//		private readonly NoteDtoValidator _noteDtoValidator = new();
//		private readonly NoteRequestValidator _noteRequestValidator = new();
//		private readonly NoteDetailsValidator _noteDetailsValidator = new();

//		private readonly INoteService _noteService;
//		private readonly IMapper _mapper;

//		public NotesController(INoteService noteService, IMapper mapper)
//		{
//			_noteService = noteService;
//			_mapper = mapper;
//		}

//		// GET: api/customers/5/notes
//		[HttpGet]
//		public ActionResult<NoteListResponse> FindAllForCustomer([FromRoute] int customerId)
//		{
//			CheckRouteArgument.ValidId(customerId, nameof(customerId));

//			var notes = _noteService.FindAllForCustomer(customerId);

//			var noteListResponse = _mapper.Map<NoteListResponse>(notes);
//			noteListResponse.Self = LinkHelper.Notes(customerId);

//			return Ok(noteListResponse);
//		}

//		// GET api/customers/5/notes/7
//		[HttpGet("{noteId:int}")]
//		public ActionResult<NoteResponse> GetForCustomer(
//			[FromRoute] int customerId, [FromRoute] int noteId)
//		{
//			CheckRouteArgument.ValidId(customerId, nameof(customerId));
//			CheckRouteArgument.ValidId(noteId, nameof(noteId));

//			var note = _noteService.GetForCustomer(noteId, customerId);

//			var noteResponse = _mapper.Map<NoteResponse>(note);

//			//TODO: add validation

//			return Ok(noteResponse);
//		}

//		// POST api/customers/5/notes
//		[HttpPost]
//		public IActionResult Save(int customerId, [FromBody] NoteRequest noteRequest)
//		{
//			CheckRouteArgument.ValidId(customerId, nameof(customerId));

//			_noteRequestValidator.Validate(noteRequest).WithInvalidBodyException();

//			var note = _mapper.Map<Note>(noteRequest);

//			note.CustomerId = customerId;

//			_noteService.Save(note);

//			return Ok();
//		}

//		// PUT api/customers/5/notes/7
//		[HttpPut("{noteId:int}")]
//		public IActionResult Update([FromRoute] int customerId, [FromRoute] int noteId,
//			[FromBody] NoteDto noteDto)
//		{
//			CheckRouteArgument.ValidId(customerId, nameof(customerId));
//			CheckRouteArgument.ValidId(noteId, nameof(noteId));

//			_noteDtoValidator.Validate(noteDto).WithInvalidBodyException();

//			var note = _mapper.Map<Note>(noteDto);

//			note.NoteId = noteId;
//			note.CustomerId = customerId;

//			_noteService.UpdateForCustomer(note);

//			return Ok();
//		}

//		// DELETE api/customers/5/notes/7
//		[HttpDelete("{noteId:int}")]
//		public IActionResult Delete([FromRoute] int customerId, [FromRoute] int noteId)
//		{
//			CheckRouteArgument.ValidId(customerId, nameof(customerId));
//			CheckRouteArgument.ValidId(noteId, nameof(noteId));

//			_noteService.DeleteForCustomer(noteId, customerId);

//			return Ok();
//		}
//	}
//}
