using System.IO;
using CustomerLibCore.Data.Repositories;
using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.ServiceLayer.Services;
using CustomerLibCore.ServiceLayer.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CustomerLibCore.Api.Tests
{
	public class StartupTest
	{
		private static IConfiguration _configuration;

		public static IConfiguration Configuration
		{
			get
			{
				if (_configuration is null)
				{
					_configuration = new ConfigurationBuilder()
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json")
						.Build();
				}

				return _configuration;
			}
		}

		private static ServiceProvider GetServiceProvider()
		{
			var startup = new Startup(Configuration);

			var services = new ServiceCollection();

			startup.ConfigureServices(services);

			return services.BuildServiceProvider();
		}

		[Fact]
		public void ShouldRegisterTransientRepositories()
		{
			var provider = GetServiceProvider();

			// Notes
			var noteRepo1 = provider.GetRequiredService<INoteRepository>();
			var noteRepo2 = provider.GetRequiredService<INoteRepository>();

			Assert.NotEqual(noteRepo1, noteRepo2);
			Assert.IsType<NoteRepository>(noteRepo1);

			// Addresses
			var addressRepo1 = provider.GetRequiredService<IAddressRepository>();
			var addressRepo2 = provider.GetRequiredService<IAddressRepository>();

			Assert.NotEqual(addressRepo1, addressRepo2);
			Assert.IsType<AddressRepository>(addressRepo1);

			// Customers
			var customerRepo1 = provider.GetRequiredService<ICustomerRepository>();
			var customerRepo2 = provider.GetRequiredService<ICustomerRepository>();

			Assert.NotEqual(customerRepo1, customerRepo2);
			Assert.IsType<CustomerRepository>(customerRepo1);
		}

		[Fact]
		public void ShouldRegisterTransientServices()
		{
			var provider = GetServiceProvider();

			// Notes
			var noteService1 = provider.GetRequiredService<INoteService>();
			var noteService2 = provider.GetRequiredService<INoteService>();

			Assert.NotEqual(noteService1, noteService2);
			Assert.IsType<NoteService>(noteService1);

			// Addresses
			var addressService1 = provider.GetRequiredService<IAddressService>();
			var addressService2 = provider.GetRequiredService<IAddressService>();

			Assert.NotEqual(addressService1, addressService2);
			Assert.IsType<AddressService>(addressService1);

			// Customers
			var customerService1 = provider.GetRequiredService<ICustomerService>();
			var customerService2 = provider.GetRequiredService<ICustomerService>();

			Assert.NotEqual(customerService1, customerService2);
			Assert.IsType<CustomerService>(customerService1);
		}

		[Fact]
		public void ShouldRegisterSqlServerDbContext()
		{
			var provider = GetServiceProvider();

			var connectionStringName = "CustomerLibDb";

			var dbContext = provider.GetRequiredService<CustomerLibDataContext>();

			Assert.Equal(Configuration.GetConnectionString(connectionStringName),
				dbContext.Database.GetConnectionString());
			Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer",
				dbContext.Database.ProviderName);
		}

		// TODO: test AutoMapper registration
	}
}
