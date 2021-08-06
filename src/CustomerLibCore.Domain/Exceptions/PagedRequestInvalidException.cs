using System;
using System.Runtime.Serialization;

namespace CustomerLibCore.Domain.Exceptions
{
	/// <summary>
	/// Throw when the requested page of a resource is greater than 1 and 
	/// there are not enough items to start filling the requested page.
	/// </summary>
	[Serializable]
	public class PagedRequestInvalidException : Exception
	{
		public int Page { get; }
		public int PageSize { get; }

		public PagedRequestInvalidException(int page, int pageSize) : base()
		{
			Page = page;
			PageSize = pageSize;
		}

		protected PagedRequestInvalidException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Page = (int)info.GetValue(nameof(Page), typeof(int));
			PageSize = (int)info.GetValue(nameof(PageSize), typeof(int));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(Page), Page);
			info.AddValue(nameof(PageSize), PageSize);

			base.GetObjectData(info, context);
		}
	}
}
