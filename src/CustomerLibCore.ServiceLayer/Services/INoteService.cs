using System.Collections.Generic;
using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.ServiceLayer.Services
{
	public interface INoteService
	{
		void Create(Note note);

		Note GetForCustomer(int noteId, int customerId);

		/// <returns>An empty collection if no notes found; 
		/// otherwise, the found notes.</returns>
		IReadOnlyCollection<Note> FindAllForCustomer(int customerId);

		void EditForCustomer(Note note);

		void DeleteForCustomer(int noteId, int customerId);
	}
}
