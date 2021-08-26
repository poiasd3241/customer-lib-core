using System.Collections.Generic;
using CustomerLibCore.Data.Entities;

namespace CustomerLibCore.Data.Repositories
{
	public interface INoteRepository
	{
		bool Exists(int noteId);
		bool ExistsForCustomer(int noteId, int customerId);

		/// <returns>The Id of the created item.</returns>
		int Create(NoteEntity note);
		void CreateManyForCustomer(IEnumerable<NoteEntity> notes, int customerId);

		bool GetCountForCustomer(int customerId);
		NoteEntity ReadForCustomer(int noteId, int customerId);

		/// <returns>An empty collection if no notes found; otherwise, the found notes.</returns>
		IReadOnlyCollection<NoteEntity> ReadManyForCustomer(int customerId);

		void Update(NoteEntity note);
		void Delete(int noteId);
		void DeleteManyForCustomer(int customerId);
	}
}
