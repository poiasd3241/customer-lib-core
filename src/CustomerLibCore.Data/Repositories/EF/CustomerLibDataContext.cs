using CustomerLibCore.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.Repositories.EF
{
	public class CustomerLibDataContext : DbContext
	{
		public CustomerLibDataContext() : base() { }
		public CustomerLibDataContext(DbContextOptions<CustomerLibDataContext> options)
			: base(options) { }

		public DbSet<Customer> Customers { set; get; }
		public DbSet<Address> Addresses { set; get; }
		public DbSet<Note> Notes { set; get; }

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	//modelBuilder.Entity<Customer>().Property(e => e.CustomerId).ValueGeneratedNever();
		//	modelBuilder.Entity<Address>().Property(e => e.AddressId).ValueGeneratedNever();
		//	modelBuilder.Entity<Note>().Property(e => e.NoteId).ValueGeneratedNever();
		//}
	}
}
