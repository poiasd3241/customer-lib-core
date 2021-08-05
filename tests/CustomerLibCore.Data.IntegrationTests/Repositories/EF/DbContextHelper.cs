using CustomerLibCore.Data.Repositories.EF;
using Microsoft.EntityFrameworkCore;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	public class DbContextHelper
	{
		private static DbContextOptions<CustomerLibDataContext> _options;

		public static DbContextOptions<CustomerLibDataContext> Options
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

		public static CustomerLibDataContext Context => new(Options);

		public static void ClearDatabase()
		{

		}
	}
}
