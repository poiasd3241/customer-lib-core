using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using CustomerLibCore.Domain.ArgumentCheckHelpers;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.Domain.Validators;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Data.Repositories.EF;

namespace CustomerLibCore.ServiceLayer.Services.Implementations
{
	public class CustomerService : ICustomerService
	{
		#region Private Members

		private readonly ICustomerRepository _customerRepository;

		private readonly IAddressRepository _addressRepository;
		private readonly INoteRepository _noteRepository;

		#endregion

		#region Constructors

		public CustomerService(ICustomerRepository customerRepository,
			IAddressRepository addressRepository, INoteRepository noteRepository)
		{
			_customerRepository = customerRepository;

			_addressRepository = addressRepository;
			_noteRepository = noteRepository;
		}

		#endregion

		#region Public Methods

		public void Save(Customer customer)
		{
			new CustomerValidator().ValidateFull(customer).WithInternalValidationException();

			using TransactionScope scope = new();

			if (_customerRepository.IsEmailTaken(customer.Email))
			{
				throw new EmailTakenException();
			}

			var customerId = _customerRepository.Create(customer);

			scope.Complete();
		}

		public Customer Get(int customerId, bool includeAddresses, bool includeNotes)
		{
			CheckNumber.ValidId(customerId, nameof(customerId));

			using TransactionScope scope = new();

			var customer = _customerRepository.Read(customerId);

			if (customer is null)
			{
				throw new NotFoundException();
			}

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

			var pageCustomers = _customerRepository.ReadPage(page, pageSize);

			if (pageCustomers.Count == 0)
			{
				if (page == 1)
				{
					return new(pageCustomers, 1, pageSize, 1);
				}

				throw new PagedRequestInvalidException(page, pageSize);
			}

			if (includeAddresses)
			{
				LoadAddresses(pageCustomers);
			}

			if (includeNotes)
			{
				LoadNotes(pageCustomers);
			}

			var lastPage = (int)Math.Ceiling((double)GetCount() / pageSize);

			return new(pageCustomers, page, pageSize, lastPage);
		}

		public void Update(Customer customer)
		{
			CheckNumber.ValidId(customer.CustomerId, nameof(customer.CustomerId));

			// TODO: replace with CustomerBasicDetailsValidator!?
			new CustomerValidator().ValidateWithoutAddressesAndNotes(customer)
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

			_customerRepository.Update(customer);

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

			_addressRepository.DeleteManyForCustomer(customerId);
			_noteRepository.DeleteManyForCustomer(customerId);

			_customerRepository.Delete(customerId);

			scope.Complete();
		}

		#endregion

		#region Private Methods

		private void LoadAddresses(Customer customer)
		{
			customer.Addresses = _addressRepository.ReadManyForCustomer(customer.CustomerId)
				.ToList();
		}

		private void LoadNotes(Customer customer)
		{
			customer.Notes = _noteRepository.ReadManyForCustomer(customer.CustomerId).ToList();
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
