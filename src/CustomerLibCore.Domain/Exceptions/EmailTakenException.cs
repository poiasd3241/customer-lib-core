using System;

namespace CustomerLibCore.Domain.Exceptions
{
	/// <summary>
	/// Throw when the email is already taken.
	/// </summary>
	[Serializable]
	public class EmailTakenException : Exception
	{
		public EmailTakenException() { }
		protected EmailTakenException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
