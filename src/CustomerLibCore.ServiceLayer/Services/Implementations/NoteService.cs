using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Domain.ArgumentCheckHelpers;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;

namespace CustomerLibCore.ServiceLayer.Services.Implementations
{
	public class NoteService : INoteService
	{
		#region Private Members

		private readonly ICustomerRepository _customerRepository;
		private readonly INoteRepository _noteRepository;
		private readonly IMapper _mapper;

		private readonly NoteValidator _validator = new();

		#endregion

		#region Constructors

		public NoteService(ICustomerRepository customerRepository, INoteRepository noteRepository,
			IMapper mapper)
		{
			_customerRepository = customerRepository;
			_noteRepository = noteRepository;
			_mapper = mapper;
		}

		#endregion

		#region Public Methods

		public void Save(Note note)
		{
			CheckNumber.ValidId(note.CustomerId, nameof(note.CustomerId));

			_validator.Validate(note).WithInternalValidationException();

			using TransactionScope scope = new();

			if (_customerRepository.Exists(note.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			var noteEntity = _mapper.Map<NoteEntity>(note);

			_noteRepository.Create(noteEntity);

			scope.Complete();
		}

		public Note GetForCustomer(int noteId, int customerId)
		{
			CheckNumber.ValidId(noteId, nameof(noteId));
			CheckNumber.ValidId(customerId, nameof(customerId));

			var noteEntity = _noteRepository.ReadForCustomer(noteId, customerId);

			if (noteEntity is null)
			{
				throw new NotFoundException();
			}

			var note = _mapper.Map<Note>(noteEntity);

			return note;
		}

		public IReadOnlyCollection<Note> FindAllForCustomer(int customerId)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_customerRepository.Exists(customerId) == false)
			{
				throw new NotFoundException();
			}

			var noteEntities = _noteRepository.ReadManyForCustomer(customerId);

			scope.Complete();

			var notes = _mapper.Map<IEnumerable<Note>>(noteEntities);

			return notes.ToArray();
		}

		public void UpdateForCustomer(Note note)
		{
			CheckNumber.ValidId(note.NoteId, nameof(note.NoteId));
			CheckNumber.ValidId(note.CustomerId, nameof(note.CustomerId));

			_validator.Validate(note).WithInternalValidationException();

			using TransactionScope scope = new();

			if (_noteRepository.ExistsForCustomer(note.NoteId, note.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			var noteEntity = _mapper.Map<NoteEntity>(note);

			_noteRepository.Update(noteEntity);

			scope.Complete();
		}

		public void DeleteForCustomer(int noteId, int customerId)
		{
			CheckNumber.ValidId(noteId, nameof(noteId));
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_noteRepository.ExistsForCustomer(noteId, customerId) == false)
			{
				throw new NotFoundException();
			}

			// TODO: prevent delete of the last address.

			_noteRepository.Delete(noteId);

			scope.Complete();
		}

		#endregion
	}
}
