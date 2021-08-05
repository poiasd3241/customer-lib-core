using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class NoteRepository : INoteRepository
	{
		#region Private Members

		private readonly CustomerLibDataContext _context;

		#endregion

		#region Constructors

		public NoteRepository(CustomerLibDataContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public bool Exists(int noteId) =>
			_context.Notes.Any(note => note.NoteId == noteId);

		public bool ExistsForCustomer(int noteId, int customerId) =>
			_context.Notes.Any(note =>
				note.NoteId == noteId &&
				note.CustomerId == customerId
			);

		public int Create(NoteEntity note)
		{
			var createdNote = _context.Notes.Add(note).Entity;

			_context.SaveChanges();

			return createdNote.NoteId;
		}

		public NoteEntity Read(int noteId) =>
			_context.Notes.Find(noteId);

		public NoteEntity ReadForCustomer(int noteId, int customerId) =>
			_context.Notes.FirstOrDefault(note =>
				note.NoteId == noteId &&
				note.CustomerId == customerId);

		public IReadOnlyCollection<NoteEntity> ReadManyForCustomer(int customerId) =>
			_context.Notes.Where(note => note.CustomerId == customerId)
				.ToArray();

		public void Update(NoteEntity note)
		{
			var noteDb = _context.Notes.Find(note.NoteId);

			if (noteDb is not null)
			{
				_context.Entry(noteDb).CurrentValues.SetValues(note);
				_context.Entry(noteDb).State = EntityState.Modified;
				_context.Entry(noteDb).Property(note => note.CustomerId).IsModified = false;

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
	}
}
