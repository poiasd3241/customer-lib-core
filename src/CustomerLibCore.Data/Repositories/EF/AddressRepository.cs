using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class AddressRepository : IAddressRepository
	{
		#region Private Members

		private readonly CustomerLibDataContext _context;

		#endregion

		#region Constructors

		public AddressRepository(CustomerLibDataContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public bool Exists(int addressId) =>
			_context.Addresses.Any(address => address.AddressId == addressId);

		public bool ExistsForCustomer(int addressId, int customerId) =>
			_context.Addresses.Any(address =>
				address.AddressId == addressId &&
				address.CustomerId == customerId);

		public int Create(AddressEntity address)
		{
			var createdAddress = _context.Addresses.Add(address).Entity;

			_context.SaveChanges();

			return createdAddress.AddressId;
		}

		public AddressEntity Read(int addressId) =>
			_context.Addresses.Find(addressId);

		public AddressEntity ReadForCustomer(int addressId, int customerId)
			=> _context.Addresses.FirstOrDefault(address =>
				address.AddressId == addressId &&
				address.CustomerId == customerId);

		public IReadOnlyCollection<AddressEntity> ReadManyForCustomer(int customerId) =>
			_context.Addresses.Where(address => address.CustomerId == customerId)
				.ToArray();

		public void Update(AddressEntity address)
		{
			//var addressDb = _context.Addresses.First(a => a.AddressId == 1);

			var addressDb = _context.Addresses.Find(address.AddressId);

			if (addressDb is not null)
			{
				_context.Entry(addressDb).CurrentValues.SetValues(address);
				_context.Entry(addressDb).State = EntityState.Modified;

				_context.Update(addressDb);

				_context.SaveChanges();
			}
		}

		public void UpdateForCustomer(AddressEntity address)
		{
			var addressDb = _context.Addresses.FirstOrDefault(repoAddress =>
				repoAddress.AddressId == address.AddressId &&
				repoAddress.CustomerId == address.CustomerId);

			if (addressDb is not null)
			{
				_context.Entry(addressDb).CurrentValues.SetValues(address);

				_context.SaveChanges();
			}
		}

		public void Delete(int addressId) =>
			_context.Database.ExecuteSqlInterpolated(
				$"DELETE FROM [dbo].[Addresses] WHERE [AddressId] = {addressId};");

		public void DeleteManyForCustomer(int customerId) =>
			_context.Database.ExecuteSqlRaw(
				$"DELETE FROM [dbo].[Addresses] WHERE [CustomerId] = {customerId};");

		public void DeleteAll() =>
			_context.Database.ExecuteSqlRaw(
				"DELETE FROM [dbo].[Addresses];" +
				"DBCC CHECKIDENT ('dbo.Addresses', RESEED, 0);");

		#endregion
	}
}
