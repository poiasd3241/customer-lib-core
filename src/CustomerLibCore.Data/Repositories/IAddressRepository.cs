using System.Collections.Generic;
using CustomerLibCore.Business.Entities;

namespace CustomerLibCore.Data.Repositories
{
	public interface IAddressRepository
	{
		bool Exists(int addressId);

		/// <returns>The Id of the created item.</returns>
		int Create(Address address);
		Address Read(int addressId);

		/// <returns>An empty collection if no customers found; 
		/// otherwise, the found customers.</returns>
		IReadOnlyCollection<Address> ReadByCustomer(int customerId);
		void Update(Address address);
		void Delete(int addressId);
		void DeleteByCustomer(int customerId);
	}
}
