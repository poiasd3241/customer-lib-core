using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Domain.ArgumentCheckHelpers;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;

namespace CustomerLibCore.ServiceLayer.Services.Implementations
{
	public class CustomerService : ICustomerService
	{
		#region Private Members

		private readonly ICustomerRepository _customerRepository;
		private readonly IAddressRepository _addressRepository;
		private readonly INoteRepository _noteRepository;
		private readonly IMapper _mapper;

		private readonly CustomerValidator _validator = new();

		#endregion

		#region Constructors

		public CustomerService(ICustomerRepository customerRepository,
			IAddressRepository addressRepository, INoteRepository noteRepository, IMapper mapper)
		{
			_customerRepository = customerRepository;

			_addressRepository = addressRepository;
			_noteRepository = noteRepository;
			_mapper = mapper;
		}

		#endregion

		#region Public Methods

		public void Save(Customer customer)
		{
			_validator.ValidateFull(customer).WithInternalValidationException();

			using TransactionScope scope = new();

			if (_customerRepository.IsEmailTaken(customer.Email))
			{
				throw new EmailTakenException();
			}

			var customerEntity = _mapper.Map<CustomerEntity>(customer);

			var customerId = _customerRepository.Create(customerEntity);

			// Save child properties.
			AddressEntity addressEntity;

			foreach (var address in customer.Addresses)
			{
				addressEntity = _mapper.Map<AddressEntity>(address);
				addressEntity.CustomerId = customerId;

				_addressRepository.Create(addressEntity);
			}

			NoteEntity noteEntity;

			foreach (var note in customer.Notes)
			{
				noteEntity = _mapper.Map<NoteEntity>(note);
				noteEntity.CustomerId = customerId;

				_noteRepository.Create(noteEntity);
			}

			scope.Complete();
		}

		public Customer Get(int customerId, bool includeAddresses, bool includeNotes)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			var customerEntity = _customerRepository.Read(customerId);

			if (customerEntity is null)
			{
				throw new NotFoundException();
			}

			var customer = _mapper.Map<Customer>(customerEntity);

			if (includeAddresses)
			{
				LoadAddresses(customer);
			}

			if (includeNotes)
			{
				LoadNotes(customer);
			}

			return customer;
		}

		public int GetCount() => _customerRepository.GetCount();

		public PagedResult<Customer> GetPage(int page, int pageSize,
			bool includeAddresses, bool includeNotes)
		{
			CheckNumber.NotLessThan(1, page, nameof(page));
			CheckNumber.NotLessThan(1, pageSize, nameof(pageSize));

			using TransactionScope scope = new();

			var pageCustomerEntities = _customerRepository.ReadPage(page, pageSize);

			if (pageCustomerEntities.Count == 0)
			{
				if (page == 1)
				{
					return new(Array.Empty<Customer>(), 1, pageSize, 1);
				}

				throw new PagedRequestInvalidException(page, pageSize);
			}

			var pageCustomers = _mapper.Map<IEnumerable<Customer>>(pageCustomerEntities);

			if (includeAddresses)
			{
				LoadAddresses(pageCustomers);
			}

			if (includeNotes)
			{
				LoadNotes(pageCustomers);
			}

			var lastPage = (int)Math.Ceiling((double)GetCount() / pageSize);

			return new(pageCustomers.ToArray(), page, pageSize, lastPage);
		}

		public void Update(Customer customer)
		{
			CheckNumber.ValidId(customer.CustomerId, nameof(customer.CustomerId));

			_validator.ValidateWithoutAddressesAndNotes(customer)
				.WithInternalValidationException();

			using TransactionScope scope = new();

			if (_customerRepository.Exists(customer.CustomerId) == false)
			{
				throw new NotFoundException();
			}

			var (isTaken, takenById) = _customerRepository.IsEmailTakenWithCustomerId(
				customer.Email);

			if (isTaken && takenById != customer.CustomerId)
			{
				throw new EmailTakenException();
			}

			var customerEntity = _mapper.Map<CustomerEntity>(customer);

			_customerRepository.Update(customerEntity);

			scope.Complete();
		}

		public void Delete(int customerId)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			if (_customerRepository.Exists(customerId) == false)
			{
				throw new NotFoundException();
			}

			// First delete child properties.
			_addressRepository.DeleteManyForCustomer(customerId);
			_noteRepository.DeleteManyForCustomer(customerId);

			_customerRepository.Delete(customerId);

			scope.Complete();
		}

		#endregion

		#region Private Methods

		private void LoadAddresses(Customer customer)
		{
			var addressEntities = _addressRepository.ReadManyForCustomer(customer.CustomerId);
			customer.Addresses = _mapper.Map<IEnumerable<Address>>(addressEntities);
		}

		private void LoadNotes(Customer customer)
		{
			var noteEntities = _noteRepository.ReadManyForCustomer(customer.CustomerId);
			customer.Notes = _mapper.Map<IEnumerable<Note>>(noteEntities);
		}

		private void LoadAddresses(IEnumerable<Customer> customers)
		{
			foreach (var customer in customers)
			{
				LoadAddresses(customer);
			}
		}

		private void LoadNotes(IEnumerable<Customer> customers)
		{
			foreach (var customer in customers)
			{
				LoadNotes(customer);
			}
		}

		#endregion
	}
}
