using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Business.Entities;
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

		public int Create(Address address)
		{
			var createdAddress = _context.Addresses.Add(address).Entity;

			_context.SaveChanges();

			return createdAddress.AddressId;
		}

		public Address Read(int addressId) =>
			_context.Addresses.Find(addressId);

		public Address ReadForCustomer(int addressId, int customerId)
			=> _context.Addresses.FirstOrDefault(address =>
				address.AddressId == addressId &&
				address.CustomerId == customerId);

		public IReadOnlyCollection<Address> ReadManyForCustomer(int customerId) =>
			_context.Addresses.Where(address => address.CustomerId == customerId)
				.ToArray();

		public void Update(Address address)
		{
			//var addressDb = _context.Addresses.First(a => a.AddressId == 1);

			var addressDb = _context.Addresses.Find(address.AddressId);

			if (addressDb is not null)
			{
				_context.Entry(addressDb).CurrentValues.SetValues(address);

				_context.Update(addressDb);

				_context.SaveChanges();
			}
		}

		public void UpdateForCustomer(Address address)
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

		public void Delete(int addressId)
		{
			var address = _context.Addresses.Find(addressId);

			if (address is not null)
			{
				_context.Addresses.Remove(address);

				_context.SaveChanges();
			}
		}

		public void DeleteForCustomer(int addressId, int customerId)
		{
			var address = _context.Addresses.FirstOrDefault(address =>
				address.AddressId == addressId &&
				address.CustomerId == customerId);

			if (address is not null)
			{
				_context.Addresses.Remove(address);

				_context.SaveChanges();
			}
		}

		public void DeleteManyForCustomer(int customerId)
		{
			var addresses = _context.Addresses
				.Where(address => address.CustomerId == customerId);

			foreach (var address in addresses)
			{
				_context.Addresses.Remove(address);
			}

			_context.SaveChanges();
		}

		public void DeleteAll()
		{
			var addresses = _context.Addresses.ToArray();

			foreach (var address in addresses)
			{
				_context.Addresses.Remove(address);
			}

			_context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Addresses', RESEED, 0);");

			_context.SaveChanges();
		}

		#endregion
	}
}
