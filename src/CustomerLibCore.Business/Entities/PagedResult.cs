using System.Collections.Generic;

namespace CustomerLibCore.Business.Entities
{
	public class PagedResult<T> where T : Entity
	{
		public IReadOnlyCollection<T> Items { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int LastPage { get; set; }

		public PagedResult() { }

		public PagedResult(IReadOnlyCollection<T> items, int page, int pageSize, int totalCount)
		{
			Items = items;
			Page = page;
			PageSize = pageSize;
			LastPage = totalCount;
		}
	}
}
