using System.Collections.Generic;

namespace CustomerLibCore.Api.Dtos
{
	public interface IListResourceBase<T>
	{
		/// <summary>
		/// The items of the response.
		/// </summary>
		IEnumerable<T> Items { get; set; }
	}
}