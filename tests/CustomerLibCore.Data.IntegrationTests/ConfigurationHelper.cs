using System.IO;
using Microsoft.Extensions.Configuration;

namespace CustomerLibCore.Data.IntegrationTests
{
	public class ConfigurationHelper
	{
		private static string _connectionString;

		public static string ConnectionString
		{
			get
			{
				if (_connectionString is null)
				{
					var configuration = new ConfigurationBuilder()
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json")
						.Build();
					_connectionString = configuration.GetConnectionString("CustomerLibDb");
				}

				return _connectionString;
			}
		}
	}
}
