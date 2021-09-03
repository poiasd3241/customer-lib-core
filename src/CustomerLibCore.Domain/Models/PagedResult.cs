using System.Collections.Generic;

namespace CustomerLibCore.Domain.Models
{
	public class PagedResult<T>
	{
		public IReadOnlyCollection<T> Items { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int LastPage { get; set; }

		public PagedResult() { }

		public PagedResult(IReadOnlyCollection<T> items, int page, int pageSize, int lastPage)
		{
			Items = items;
			Page = page;
			PageSize = pageSize;
			LastPage = lastPage;
		}
	}
}
