using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Domain.Extensions;
using CustomerLibCore.Domain.FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class NoteRepository : INoteRepository
	{
		#region Private Members

		private readonly CustomerLibDataContext _context;

		private readonly NoteEntityValidator _validator = new();

		#endregion

		#region Constructor

		public NoteRepository(CustomerLibDataContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public int Create(NoteEntity note)
		{
			ValidateEntity(note);

			var createdNote = _context.Notes.Add(note).Entity;

			_context.SaveChanges();

			return createdNote.NoteId;
		}

		public void CreateManyForCustomer(IEnumerable<NoteEntity> notes, int customerId)
		{
			foreach (var note in notes)
			{
				ValidateEntity(note);

				note.CustomerId = customerId;
			}

			_context.Notes.AddRange(notes);

			_context.SaveChanges();
		}

		public bool Exists(int noteId) =>
			_context.Notes.Any(note => note.NoteId == noteId);

		public bool ExistsForCustomer(int noteId, int customerId) =>
			_context.Notes.Any(note =>
				note.NoteId == noteId &&
				note.CustomerId == customerId
			);

		public int GetCountForCustomer(int customerId) =>
			_context.Notes.Where(note => note.CustomerId == customerId).Count();

		public NoteEntity ReadForCustomer(int noteId, int customerId) =>
			_context.Notes.FirstOrDefault(note =>
				note.NoteId == noteId &&
				note.CustomerId == customerId);

		public IReadOnlyCollection<NoteEntity> ReadManyForCustomer(int customerId) =>
			_context.Notes.Where(note => note.CustomerId == customerId)
				.ToArray();

		/// <summary>
		/// Updates the note with the provided details, if found. The lookup is performed using both
		/// <see cref="NoteEntity.NoteId"/> and <see cref="NoteEntity.CustomerId"/> values.
		/// </summary>
		/// <param name="note">The note details for the update.</param>
		public void Update(NoteEntity note)
		{
			ValidateEntity(note);

			var noteDb = _context.Notes.FirstOrDefault(n =>
				n.NoteId == note.NoteId &&
				n.CustomerId == note.CustomerId);

			if (noteDb is not null)
			{
				_context.Entry(noteDb).CurrentValues.SetValues(note);

				_context.SaveChanges();
			}
		}

		public void Delete(int noteId) =>
			_context.Database.ExecuteSqlInterpolated(
				$"DELETE FROM [dbo].[Notes] WHERE [NoteId] = {noteId};");

		public void DeleteManyForCustomer(int customerId) =>
			_context.Database.ExecuteSqlRaw(
				$"DELETE FROM [dbo].[Notes] WHERE [CustomerId] = {customerId};");

		public void DeleteAll() =>
			_context.Database.ExecuteSqlRaw(
				"DELETE FROM [dbo].[Notes];" +
				"DBCC CHECKIDENT ('dbo.Notes', RESEED, 0);");

		#endregion

		#region Private Methods

		private void ValidateEntity(NoteEntity note) =>
			_validator.Validate(note).WithInternalValidationException();

		#endregion
	}
}
