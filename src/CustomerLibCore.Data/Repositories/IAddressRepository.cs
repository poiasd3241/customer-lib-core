using System.Collections.Generic;
using CustomerLibCore.Data.Entities;

namespace CustomerLibCore.Data.Repositories
{
	public interface IAddressRepository
	{
		bool Exists(int addressId);
		bool ExistsForCustomer(int addressId, int customerId);

		/// <returns>The Id of the created item.</returns>
		int Create(AddressEntity address);

		AddressEntity Read(int addressId);
		AddressEntity ReadForCustomer(int addressId, int customerId);

		/// <returns>An empty collection if no customers found; 
		/// otherwise, the found customers.</returns>
		IReadOnlyCollection<AddressEntity> ReadManyForCustomer(int customerId);

		void Update(AddressEntity address);
		void UpdateForCustomer(AddressEntity address);

		void Delete(int addressId);
		void DeleteManyForCustomer(int customerId);
	}
}
