using System.Data.SqlClient;

namespace CustomerLibCore.Data.IntegrationTests
{
	public class DatabaseHelper
	{
		/// <summary>
		/// Removes all data from the following tables in the database:
		/// <br/>
		/// [dbo].[Addresses]
		/// <br/>
		/// [dbo].[Notes]
		/// <br/>
		/// [dbo].[Customers];
		/// <br/>
		/// then reseeds the identity columns on these tables.
		/// </summary>
		public static void Clear()
		{
			using var connection = GetSqlConnection();
			connection.Open();

			var deleteCommand = new SqlCommand(
				"DELETE FROM [dbo].[Addresses];" +
				"DELETE FROM [dbo].[Notes];" +
				"DELETE FROM [dbo].[Customers];", connection);
			deleteCommand.ExecuteNonQuery();

			var reseedCommand = new SqlCommand(
				"DBCC CHECKIDENT ('dbo.Addresses', RESEED, 0);" +
				"DBCC CHECKIDENT ('dbo.Notes', RESEED, 0);" +
				"DBCC CHECKIDENT ('dbo.Customers', RESEED, 0);", connection);
			reseedCommand.ExecuteNonQuery();
		}

		private static SqlConnection GetSqlConnection() =>
			new(ConfigurationHelper.ConnectionString);
	}
}
