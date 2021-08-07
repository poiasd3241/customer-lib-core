using System.Collections.Generic;
using CustomerLibCore.Domain.Models;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Entities
{
	public class PagedResultTest
	{
		private class Whatever { }

		[Fact]
		public void ShouldCreateObjectDefault()
		{
			var result = new PagedResult<Whatever>();

			Assert.Null(result.Items);
			Assert.Equal(0, result.Page);
			Assert.Equal(0, result.PageSize);
			Assert.Equal(0, result.LastPage);
		}

		[Fact]
		public void ShouldCreateObject()
		{
			var items = new List<Whatever>();
			var page = 5;
			var pageSize = 7;
			var totalCount = 32;

			var result = new PagedResult<Whatever>(items, page, pageSize, totalCount);

			Assert.Equal(items, result.Items);
			Assert.Equal(page, result.Page);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(totalCount, result.LastPage);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var items = new List<Whatever>();
			var page = 5;
			var pageSize = 7;
			var totalCount = 32;

			var result = new PagedResult<Whatever>();

			Assert.NotEqual(items, result.Items);
			Assert.NotEqual(page, result.Page);
			Assert.NotEqual(pageSize, result.PageSize);
			Assert.NotEqual(totalCount, result.LastPage);

			// When
			result.Items = items;
			result.Page = page;
			result.PageSize = pageSize;
			result.LastPage = totalCount;

			// Then
			Assert.Equal(items, result.Items);
			Assert.Equal(page, result.Page);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(totalCount, result.LastPage);
		}
	}
}
