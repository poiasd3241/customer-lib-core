using System;

namespace AssertX
{
	public class AssertXException : Exception
	{
		public AssertXException(string message) : base(message) { }

		public AssertXException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}
