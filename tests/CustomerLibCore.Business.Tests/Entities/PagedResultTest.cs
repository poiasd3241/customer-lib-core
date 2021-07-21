using System;
using System.Collections.Generic;
using CustomerLibCore.Business.Entities;
using Xunit;

namespace CustomerLibCore.Business.Tests.Entities
{
	public class PagedResultTest
	{
		private class PagedEntity : Entity
		{
			public override bool EqualsByValue(object obj)
			{
				throw new NotImplementedException();
			}
		}

		[Fact]
		public void ShouldThrowOnPagedEntityNotImplemented()
		{
			var pagedEntity = new PagedEntity();
			var equalsByValueObj = "whatever";

			Assert.Throws<NotImplementedException>(() =>
				pagedEntity.EqualsByValue(equalsByValueObj));
		}


		[Fact]
		public void ShouldCreatePagedResultDefault()
		{
			PagedResult<PagedEntity> result = new();

			Assert.Null(result.Items);
			Assert.Equal(0, result.Page);
			Assert.Equal(0, result.PageSize);
			Assert.Equal(0, result.TotalCount);
		}

		[Fact]
		public void ShouldCreatePagedResult()
		{
			var items = new List<PagedEntity>();
			var page = 5;
			var pageSize = 7;
			var totalCount = 32;

			PagedResult<PagedEntity> result = new(items, page, pageSize, totalCount);

			Assert.Equal(items, result.Items);
			Assert.Equal(page, result.Page);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(totalCount, result.TotalCount);
		}

		[Fact]
		public void ShouldSetAddressProperties()
		{
			var items = new List<PagedEntity>();
			var page = 5;
			var pageSize = 7;
			var totalCount = 32;

			PagedResult<PagedEntity> result = new();

			Assert.NotEqual(items, result.Items);
			Assert.NotEqual(page, result.Page);
			Assert.NotEqual(pageSize, result.PageSize);
			Assert.NotEqual(totalCount, result.TotalCount);

			result.Items = items;
			result.Page = page;
			result.PageSize = pageSize;
			result.TotalCount = totalCount;

			Assert.Equal(items, result.Items);
			Assert.Equal(page, result.Page);
			Assert.Equal(pageSize, result.PageSize);
			Assert.Equal(totalCount, result.TotalCount);
		}
	}
}
