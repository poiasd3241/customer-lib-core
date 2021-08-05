using System.Data;
using System.Data.SqlClient;
using CustomerLibCore.Domain.Enums;

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

		/// <summary>
		/// Repopulates the AddressTypes table with the <see cref="AddressType"/> values.
		/// <br/>
		/// Doesn't clear FK-dependent table (Addresses), therefore must be used
		/// after the Addresses table is cleared.
		/// </summary>
		public static void UnsafeRepopulateAddressTypes()
		{
			using var connection = GetSqlConnection();
			connection.Open();

			var deleteAddresseTypesCommand = new SqlCommand(
				"DELETE FROM [dbo].[AddressTypes]", connection);
			deleteAddresseTypesCommand.ExecuteNonQuery();

			var populateAddresseTypesCommand = new SqlCommand(
				"INSERT INTO [dbo].[AddressTypes] " +
				"([AddressTypeId], [Type]) " +
				"VALUES " +
				"(@ShippingId, @ShippingType), (@BillingId, @BillingType)", connection);

			var shippingIdParam = new SqlParameter("@ShippingId", SqlDbType.Int)
			{
				Value = (int)AddressType.Shipping
			};
			var shippingTypeParam = new SqlParameter("@ShippingType", SqlDbType.VarChar, 8)
			{
				Value = AddressType.Shipping.ToString()
			};
			var billingIdParam = new SqlParameter("@BillingId", SqlDbType.Int)
			{
				Value = (int)AddressType.Billing
			};
			var billingTypeParam = new SqlParameter("@BillingType", SqlDbType.VarChar, 8)
			{
				Value = AddressType.Billing.ToString()
			};

			populateAddresseTypesCommand.Parameters.Add(shippingIdParam);
			populateAddresseTypesCommand.Parameters.Add(shippingTypeParam);
			populateAddresseTypesCommand.Parameters.Add(billingIdParam);
			populateAddresseTypesCommand.Parameters.Add(billingTypeParam);

			populateAddresseTypesCommand.ExecuteNonQuery();
		}

		private static SqlConnection GetSqlConnection() =>
			new(ConfigurationHelper.ConnectionString);
	}
}
