using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Customers.Response;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Customers
{
	public class CustomerPagedResponseTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var customers = new CustomerPagedResponse();

			Assert.Null(customers.Self);
			Assert.Null(customers.Previous);
			Assert.Null(customers.Next);

			Assert.Equal(0, customers.Page);
			Assert.Equal(0, customers.PageSize);
			Assert.Equal(0, customers.LastPage);

			Assert.Null(customers.Items);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var self = "self1";
			var previous = "previous1";
			var next = "next1";

			var page = 1;
			var pageSize = 2;
			var lastPage = 3;

			var items = new List<CustomerResponse>();

			var customers = new CustomerPagedResponse();

			Assert.NotEqual(self, customers.Self);
			Assert.NotEqual(previous, customers.Previous);
			Assert.NotEqual(next, customers.Next);

			Assert.NotEqual(page, customers.Page);
			Assert.NotEqual(pageSize, customers.PageSize);
			Assert.NotEqual(lastPage, customers.LastPage);

			Assert.NotEqual(items, customers.Items);

			// When
			customers.Self = self;
			customers.Previous = previous;
			customers.Next = next;

			customers.Page = page;
			customers.PageSize = pageSize;
			customers.LastPage = lastPage;

			customers.Items = items;

			// Then
			Assert.Equal(self, customers.Self);
			Assert.Equal(previous, customers.Previous);
			Assert.Equal(next, customers.Next);

			Assert.Equal(page, customers.Page);
			Assert.Equal(pageSize, customers.PageSize);
			Assert.Equal(lastPage, customers.LastPage);

			Assert.Equal(items, customers.Items);
		}
	}
}
