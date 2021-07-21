using System.Collections.Generic;
using System.Transactions;
using CustomerLibCore.Business.ArgumentCheckHelpers;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Exceptions;
using CustomerLibCore.Business.Validators;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Data.Repositories.EF;

namespace CustomerLibCore.ServiceLayer.Services.Implementations
{
	public class NoteService : INoteService
	{
		#region Private Members

		private readonly ICustomerRepository _customerRepository;
		private readonly INoteRepository _noteRepository;

		#endregion

		#region Constructors

		public NoteService()
		{
			_customerRepository = new CustomerRepository();
			_noteRepository = new NoteRepository();
		}

		public NoteService(ICustomerRepository customerRepository, INoteRepository noteRepository)
		{
			_customerRepository = customerRepository;
			_noteRepository = noteRepository;
		}

		#endregion

		#region Public Methods

		public void Save(Note note)
		{
			CheckNumber.ValidId(note.CustomerId, nameof(note.CustomerId));

			var validationResult = new NoteValidator().Validate(note);

			if (validationResult.IsValid == false)
			{
				throw new InternalValidationException(validationResult.Errors);
			}

			using TransactionScope scope = new();

			if (_customerRepository.Exists(note.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			_noteRepository.Create(note);

			scope.Complete();
		}

		public Note GetForCustomer(int noteId, int customerId)
		{
			CheckNumber.ValidId(noteId, nameof(noteId));
			CheckNumber.ValidId(customerId, nameof(customerId));

			var note = _noteRepository.ReadForCustomer(noteId, customerId);

			if (note is null)
			{
				throw new NotFoundException();
			}

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

			var notes = _noteRepository.ReadManyForCustomer(customerId);

			scope.Complete();

			return notes;
		}

		public void UpdateForCustomer(Note note)
		{
			CheckNumber.ValidId(note.NoteId, nameof(note.NoteId));
			CheckNumber.ValidId(note.CustomerId, nameof(note.CustomerId));

			var validationResult = new NoteValidator().Validate(note);

			if (validationResult.IsValid == false)
			{
				throw new InternalValidationException(validationResult.Errors);
			}

			using TransactionScope scope = new();

			if (_noteRepository.ExistsForCustomer(note.NoteId, note.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			_noteRepository.Update(note);

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

			_noteRepository.Delete(noteId);

			scope.Complete();
		}

		public void DeleteAllForCustomer(int customerId)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_customerRepository.Exists(customerId) == false)
			{
				throw new NotFoundException();
			}

			_noteRepository.DeleteManyForCustomer(customerId);

			scope.Complete();
		}

		#endregion
	}
}
