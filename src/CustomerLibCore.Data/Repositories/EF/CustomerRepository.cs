using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class CustomerRepository : ICustomerRepository
	{
		#region Private Members

		private readonly CustomerLibDataContext _context;

		#endregion

		#region Constructors

		public CustomerRepository(CustomerLibDataContext context)
		{
			_context = context;
		}

		public CustomerRepository()
		{
			_context = new();
		}

		#endregion

		#region Public Methods

		public bool Exists(int customerId) =>
			_context.Customers.Any(customer => customer.CustomerId == customerId);

		public int Create(Customer customer)
		{
			var createdCustomer = _context.Customers.Add(customer).Entity;

			_context.SaveChanges();

			return createdCustomer.CustomerId;
		}

		public Customer Read(int customerId) =>
			_context.Customers.Find(customerId);

		public IReadOnlyCollection<Customer> ReadMany() =>
			_context.Customers.ToArray();

		public int GetCount() =>
			_context.Customers.Count();

		public IReadOnlyCollection<Customer> ReadPage(int page, int pageSize) =>
			_context.Customers.OrderBy(customer => customer.CustomerId)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToArray();

		public void Update(Customer customer)
		{
			var customerDb = _context.Customers.Find(customer.CustomerId);

			if (customerDb is not null)
			{
				_context.Entry(customerDb).CurrentValues.SetValues(customer);

				_context.SaveChanges();
			}
		}

		public void Delete(int customerId)
		{
			var customer = _context.Customers.Find(customerId);

			if (customer is not null)
			{
				_context.Customers.Remove(customer);

				_context.SaveChanges();
			}
		}

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

		public void DeleteAll()
		{
			var customers = _context.Customers
				.Include("Addresses")
				.Include("Notes");

			foreach (var customer in customers)
			{
				_context.Customers.Remove(customer);
			}

			_context.Database.ExecuteSqlRaw(
				"DBCC CHECKIDENT ('dbo.Addresses', RESEED, 0);" +
				"DBCC CHECKIDENT ('dbo.Notes', RESEED, 0);" +
				"DBCC CHECKIDENT ('dbo.Customers', RESEED, 0);");

			_context.SaveChanges();
		}

		#endregion
	}
}
