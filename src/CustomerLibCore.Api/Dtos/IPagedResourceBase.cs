namespace CustomerLibCore.Api.Dtos
{
	public interface IPagedResourceBase
	{
		/// <summary>
		/// The current page number.
		/// </summary>
		public int Page { get; set; }

		/// <summary>
		/// The maximum amount of items on the page.
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// The last page number.
		/// </summary>
		public int LastPage { get; set; }

		/// <summary>
		/// The link to the previous page.
		/// </summary>
		public string Previous { get; set; }

		/// <summary>
		/// The link to the next page.
		/// </summary>
		public string Next { get; set; }
	}
}