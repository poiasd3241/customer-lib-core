using CustomerLibCore.Data.Entities;
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

		public DbSet<CustomerEntity> Customers { set; get; }
		public DbSet<AddressEntity> Addresses { set; get; }
		public DbSet<NoteEntity> Notes { set; get; }
	}
}
