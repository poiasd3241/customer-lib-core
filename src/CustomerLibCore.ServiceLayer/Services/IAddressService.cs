using System.Collections.Generic;
using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.ServiceLayer.Services
{
	public interface IAddressService
	{
		void Save(Address address);

		Address GetForCustomer(int addressId, int customerId);

		/// <returns>An empty collection if no addresses found; 
		/// otherwise, the found addresses.</returns>
		IReadOnlyCollection<Address> FindAllForCustomer(int customerId);

		void UpdateForCustomer(Address address);

		void DeleteForCustomer(int addressId, int customerId);
		void DeleteAllForCustomer(int customerId);
	}
}
