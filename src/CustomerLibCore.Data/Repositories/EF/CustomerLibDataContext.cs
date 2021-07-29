using CustomerLibCore.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class CustomerLibDataContext : DbContext
	{
		public CustomerLibDataContext(DbContextOptions<CustomerLibDataContext> options)
			: base(options)
		{
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		public DbSet<Customer> Customers { set; get; }
		public DbSet<Address> Addresses { set; get; }
		public DbSet<Note> Notes { set; get; }
	}
}
