using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.ServiceLayer.Services
{
	public interface ICustomerService
	{
		void Create(Customer customer);

		Customer Get(int customerId, bool includeAddresses, bool includeNotes);
		PagedResult<Customer> GetPage(int page, int pageSize,
			bool includeAddresses, bool includeNotes);

		/// <summary>
		/// Edits the basic customer details (does NOT edit <see cref="Customer.Addresses"/> 
		/// and <see cref="Customer.Notes"/>).
		/// </summary>
		/// <param name="customer"></param>
		void Edit(Customer customer);

		/// <summary>
		/// Deletes the customer, 
		/// including its <see cref="Customer.Addresses"/> and <see cref="Customer.Notes"/>.
		/// </summary>
		/// <param name="customerId"></param>
		void Delete(int customerId);
	}
}
