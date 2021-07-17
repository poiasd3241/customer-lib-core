using CustomerLibCore.Data.Repositories.EF;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	public class DbContextOptionsHelper
	{
		private static DbContextOptions<CustomerLibDataContext> _options;

		public static DbContextOptions<CustomerLibDataContext> CustomerLibDbContextOptions
		{
			get
			{
				if (_options is null)
				{
					var connectionString = ConfigurationHelper.ConnectionString;

					_options = new DbContextOptionsBuilder<CustomerLibDataContext>()
						.UseSqlServer(connectionString).Options;
				}

				return _options;
			}
		}
	}
}
