using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Domain.FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class CustomerRepository : ICustomerRepository
	{
		#region Private Members

		private readonly CustomerLibDataContext _context;

		private readonly CustomerEntityValidator _validator = new();

		#endregion

		#region Constructors

		public CustomerRepository(CustomerLibDataContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public bool Exists(int customerId) =>
			_context.Customers.Any(customer => customer.CustomerId == customerId);

		public int Create(CustomerEntity customer)
		{
			_validator.Validate(customer).WithInternalValidationException();

			var createdCustomer = _context.Customers.Add(customer).Entity;

			_context.SaveChanges();

			return createdCustomer.CustomerId;
		}

		public int GetCount() =>
			_context.Customers.Count();

		public CustomerEntity Read(int customerId) =>
			_context.Customers.Find(customerId);

		public IReadOnlyCollection<CustomerEntity> ReadMany() =>
			_context.Customers.ToArray();

		public IReadOnlyCollection<CustomerEntity> ReadPage(int page, int pageSize) =>
			_context.Customers.OrderBy(customer => customer.CustomerId)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToArray();

		public void Update(CustomerEntity customer)
		{
			_validator.Validate(customer).WithInternalValidationException();

			var customerDb = _context.Customers.Find(customer.CustomerId);

			if (customerDb is not null)
			{
				_context.Entry(customerDb).CurrentValues.SetValues(customer);
				_context.Entry(customerDb).State = EntityState.Modified;

				_context.SaveChanges();
			}
		}

		public void Delete(int customerId) =>
			_context.Database.ExecuteSqlInterpolated(
				$"DELETE FROM [dbo].[Customers] WHERE [CustomerId] = {customerId};");

		public bool IsEmailTaken(string email) =>
			_context.Customers.Any(customer => customer.Email == email);

		public (bool, int) IsEmailTakenWithCustomerId(string email)
		{
			var customerWithEmail = _context.Customers.FirstOrDefault(customer =>
				customer.Email == email);

			var isTaken = customerWithEmail is not null;
			var takenById = isTaken ? customerWithEmail.CustomerId : 0;

			return (isTaken, takenById);
		}

		public void DeleteAll() =>
			_context.Database.ExecuteSqlRaw(
				"DELETE FROM [dbo].[Customers];" +
				"DBCC CHECKIDENT ('dbo.Customers', RESEED, 0);");

		#endregion
	}
}
