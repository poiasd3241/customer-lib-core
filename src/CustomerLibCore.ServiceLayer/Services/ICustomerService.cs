using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.ServiceLayer.Services
{
	public interface ICustomerService
	{
		void Save(Customer customer);
		Customer Get(int customerId, bool includeAddresses, bool includeNotes);

		int GetCount();

		PagedResult<Customer> GetPage(int page, int pageSize,
			bool includeAddresses, bool includeNotes);

		/// <summary>
		/// Updates the basic customer details (does NOT update <see cref="Customer.Addresses"/> 
		/// and <see cref="Customer.Notes"/>).
		/// </summary>
		/// <param name="customer"></param>
		void Update(Customer customer);

		/// <summary>
		/// Deletes the customer, 
		/// including its <see cref="Customer.Addresses"/> and <see cref="Customer.Notes"/>.
		/// </summary>
		/// <param name="customerId"></param>
		void Delete(int customerId);
	}
}
