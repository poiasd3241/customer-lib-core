using System.Collections.Generic;
using System.Transactions;
using CustomerLibCore.Business.ArgumentCheckHelpers;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Exceptions;
using CustomerLibCore.Business.Validators;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Data.Repositories.EF;

namespace CustomerLibCore.ServiceLayer.Services.Implementations
{
	public class AddressService : IAddressService
	{
		#region Private Members

		private readonly ICustomerRepository _customerRepository;
		private readonly IAddressRepository _addressRepository;

		#endregion

		#region Constructors

		public AddressService()
		{
			_customerRepository = new CustomerRepository();
			_addressRepository = new AddressRepository();
		}

		public AddressService(ICustomerRepository customerRepository,
			IAddressRepository addressRepository)
		{
			_customerRepository = customerRepository;
			_addressRepository = addressRepository;
		}

		#endregion

		#region Public Methods

		public void Save(Address address)
		{
			CheckNumber.ValidId(address.CustomerId, nameof(address.CustomerId));

			var validationResult = new AddressValidator().Validate(address);

			if (validationResult.IsValid == false)
			{
				throw new InternalValidationException(validationResult.Errors);
			}

			using TransactionScope scope = new();

			if (_customerRepository.Exists(address.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			_addressRepository.Create(address);

			scope.Complete();
		}

		public Address GetForCustomer(int addressId, int customerId)
		{
			CheckNumber.ValidId(addressId, nameof(addressId));
			CheckNumber.ValidId(customerId, nameof(customerId));

			var address = _addressRepository.ReadForCustomer(addressId, customerId);

			if (address is null)
			{
				throw new NotFoundException();
			}

			return address;
		}

		public IReadOnlyCollection<Address> FindAllForCustomer(int customerId)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_customerRepository.Exists(customerId) == false)
			{
				throw new NotFoundException();
			}

			var addresses = _addressRepository.ReadManyForCustomer(customerId);

			scope.Complete();

			return addresses;
		}

		public void UpdateForCustomer(Address address)
		{
			CheckNumber.ValidId(address.AddressId, nameof(address.AddressId));
			CheckNumber.ValidId(address.CustomerId, nameof(address.CustomerId));

			var validationResult = new AddressValidator().Validate(address);

			if (validationResult.IsValid == false)
			{
				throw new InternalValidationException(validationResult.Errors);
			}

			using TransactionScope scope = new();

			if (_addressRepository.ExistsForCustomer(address.AddressId, address.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			_addressRepository.Update(address);

			scope.Complete();
		}

		public void DeleteForCustomer(int addressId, int customerId)
		{
			CheckNumber.ValidId(addressId, nameof(addressId));
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_addressRepository.ExistsForCustomer(addressId, customerId) == false)
			{
				throw new NotFoundException();
			}

			_addressRepository.Delete(addressId);

			scope.Complete();
		}

		public void DeleteAllForCustomer(int customerId)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_customerRepository.Exists(customerId) == false)
			{
				throw new NotFoundException();
			}

			_addressRepository.DeleteManyForCustomer(customerId);

			scope.Complete();
		}

		#endregion
	}
}
