using System.Collections.Generic;
using CustomerLibCore.Business.Entities;

namespace CustomerLibCore.Data.Repositories
{
	public interface IAddressRepository
	{
		bool Exists(int addressId);
		bool ExistsForCustomer(int addressId, int customerId);

		/// <returns>The Id of the created item.</returns>
		int Create(Address address);

		Address Read(int addressId);
		Address ReadForCustomer(int addressId, int customerId);

		/// <returns>An empty collection if no customers found; 
		/// otherwise, the found customers.</returns>
		IReadOnlyCollection<Address> ReadManyForCustomer(int customerId);

		void Update(Address address);
		void UpdateForCustomer(Address address);

		void Delete(int addressId);
		void DeleteForCustomer(int addressId, int customerid);
		void DeleteManyForCustomer(int customerId);
	}
}
