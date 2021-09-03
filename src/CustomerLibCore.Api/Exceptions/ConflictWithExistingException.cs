using System;
using System.Runtime.Serialization;

namespace CustomerLibCore.Domain.Exceptions
{
	[Serializable]
	public class ConflictWithExistingException : Exception
	{
		private static readonly string _email_taken = "email is already taken";

		public string IncomingPropertyName { get; }
		public string IncomingPropertyValue { get; }
		public string ConflictMessage { get; }

		public ConflictWithExistingException(string incomingPropertyName,
			string incomingPropertyValue, string conflictMessage) : base()
		{
			IncomingPropertyName = incomingPropertyName;
			IncomingPropertyValue = incomingPropertyValue;
			ConflictMessage = conflictMessage;
		}

		public static ConflictWithExistingException EmailTaken(
			string emailPropertyName, string emailValue) =>
				new(emailPropertyName, emailValue, _email_taken);

		protected ConflictWithExistingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			IncomingPropertyName = (string)info.GetValue(nameof(IncomingPropertyName),
				typeof(string));
			IncomingPropertyValue = (string)info.GetValue(nameof(IncomingPropertyValue),
				typeof(string));
			ConflictMessage = (string)info.GetValue(nameof(ConflictMessage), typeof(string));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(IncomingPropertyName), IncomingPropertyName);
			info.AddValue(nameof(IncomingPropertyValue), IncomingPropertyValue);
			info.AddValue(nameof(ConflictMessage), ConflictMessage);

			base.GetObjectData(info, context);
		}
	}
}
