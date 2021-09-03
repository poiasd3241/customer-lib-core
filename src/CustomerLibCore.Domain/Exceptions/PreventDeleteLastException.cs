using System;
using System.Runtime.Serialization;

namespace CustomerLibCore.Domain.Exceptions
{
	[Serializable]
	public class PreventDeleteLastException : Exception
	{
		public PreventDeleteLastException() : base() { }

		protected PreventDeleteLastException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
