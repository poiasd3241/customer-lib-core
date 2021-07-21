using System.Collections.Generic;
using CustomerLibCore.Business.Entities;

namespace CustomerLibCore.Data.Repositories
{
	public interface INoteRepository
	{
		bool Exists(int noteId);
		bool ExistsForCustomer(int noteId, int customerId);

		/// <returns>The Id of the created item.</returns>
		int Create(Note note);

		Note Read(int noteId);
		Note ReadForCustomer(int noteId, int customerId);

		/// <returns>An empty collection if no notes found; otherwise, the found notes.</returns>
		IReadOnlyCollection<Note> ReadManyForCustomer(int customerId);

		void Update(Note note);

		void Delete(int noteId);
		void DeleteForCustomer(int noteId, int customerId);
		void DeleteManyForCustomer(int customerId);
	}
}
