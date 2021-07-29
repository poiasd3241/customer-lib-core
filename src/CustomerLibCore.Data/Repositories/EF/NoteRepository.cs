using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Business.Entities;
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

		public int Create(Note note)
		{
			var createdNote = _context.Notes.Add(note).Entity;

			_context.SaveChanges();

			return createdNote.NoteId;
		}

		public Note Read(int noteId) =>
			_context.Notes.Find(noteId);

		public Note ReadForCustomer(int noteId, int customerId) =>
			_context.Notes.FirstOrDefault(note =>
				note.NoteId == noteId &&
				note.CustomerId == customerId);

		public IReadOnlyCollection<Note> ReadManyForCustomer(int customerId) =>
			_context.Notes.Where(note => note.CustomerId == customerId)
				.ToArray();

		public void Update(Note note)
		{
			var noteDb = _context.Notes.Find(note.NoteId);

			if (noteDb is not null)
			{
				_context.Entry(noteDb).CurrentValues.SetValues(note);

				_context.Entry(noteDb).Property(note => note.CustomerId).IsModified = false;

				_context.SaveChanges();
			}
		}

		public void Delete(int noteId)
		{
			var noteDb = _context.Notes.Find(noteId);

			if (noteDb is not null)
			{
				_context.Notes.Remove(noteDb);

				_context.SaveChanges();
			}
		}

		public void DeleteForCustomer(int noteId, int customerId)
		{
			var noteDb = _context.Notes.FirstOrDefault(note =>
				note.NoteId == noteId &&
				note.CustomerId == customerId);

			if (noteDb is not null)
			{
				_context.Notes.Remove(noteDb);

				_context.SaveChanges();
			}
		}

		public void DeleteManyForCustomer(int customerId)
		{
			var notesDb = _context.Notes.Where(note => note.CustomerId == customerId)
				.ToArray();

			foreach (var note in notesDb)
			{
				_context.Notes.Remove(note);
			}

			_context.SaveChanges();
		}

		public void DeleteAll()
		{
			var notesDb = _context.Notes.ToArray();

			foreach (var note in notesDb)
			{
				_context.Notes.Remove(note);
			}

			_context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Notes', RESEED, 0);");

			_context.SaveChanges();
		}

		#endregion
	}
}
