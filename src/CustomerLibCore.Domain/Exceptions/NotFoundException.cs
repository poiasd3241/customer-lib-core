using System;
using System.Runtime.Serialization;

namespace CustomerLibCore.Domain.Exceptions
{
	[Serializable]
	public class NotFoundException : Exception
	{
		public NotFoundException() : base() { }

		protected NotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
