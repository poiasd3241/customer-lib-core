using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Domain.ArgumentCheckHelpers;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;

namespace CustomerLibCore.ServiceLayer.Services.Implementations
{
	public class AddressService : IAddressService
	{
		#region Private Members

		private readonly ICustomerRepository _customerRepository;
		private readonly IAddressRepository _addressRepository;
		private readonly IMapper _mapper;

		private readonly AddressValidator _validator = new();

		#endregion

		#region Constructors

		public AddressService(ICustomerRepository customerRepository,
			IAddressRepository addressRepository, IMapper mapper)
		{
			_customerRepository = customerRepository;
			_addressRepository = addressRepository;
			_mapper = mapper;
		}

		#endregion

		#region Public Methods

		public void Save(Address address)
		{
			CheckNumber.ValidId(address.CustomerId, nameof(address.CustomerId));

			_validator.Validate(address).WithInternalValidationException();

			using TransactionScope scope = new();

			if (_customerRepository.Exists(address.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			var addressEntity = _mapper.Map<AddressEntity>(address);

			_addressRepository.Create(addressEntity);

			scope.Complete();
		}

		public Address GetForCustomer(int addressId, int customerId)
		{
			CheckNumber.ValidId(addressId, nameof(addressId));
			CheckNumber.ValidId(customerId, nameof(customerId));

			var addressEntity = _addressRepository.ReadForCustomer(addressId, customerId);

			if (addressEntity is null)
			{
				throw new NotFoundException();
			}

			var address = _mapper.Map<Address>(addressEntity);

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

			var addressEntities = _addressRepository.ReadManyForCustomer(customerId);

			scope.Complete();

			var addresses = _mapper.Map<IEnumerable<Address>>(addressEntities);

			return addresses.ToArray();
		}

		public void UpdateForCustomer(Address address)
		{
			CheckNumber.ValidId(address.AddressId, nameof(address.AddressId));
			CheckNumber.ValidId(address.CustomerId, nameof(address.CustomerId));

			_validator.Validate(address).WithInternalValidationException();

			using TransactionScope scope = new();

			if (_addressRepository.ExistsForCustomer(address.AddressId, address.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			var addressEntity = _mapper.Map<AddressEntity>(address);

			_addressRepository.Update(addressEntity);

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
