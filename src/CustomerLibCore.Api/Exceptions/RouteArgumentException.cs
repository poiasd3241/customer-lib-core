using System;
using System.Runtime.Serialization;

namespace CustomerLibCore.Api.Exceptions
{
	public class RouteArgumentException : ArgumentException
	{
		private readonly string _message;

		public override string Message => _message;

		public RouteArgumentException(string message, string paramName)
			: base(message, paramName)
		{
			_message = message;
		}

		protected RouteArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
