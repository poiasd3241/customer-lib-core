﻿using System;
using System.Runtime.Serialization;

namespace CustomerLibCore.Api.Exceptions
{
	[Serializable]
	public class QueryArgumentException : ArgumentException
	{
		private readonly string _message;

		public override string Message => _message;

		public QueryArgumentException(string message, string paramName)
			: base(message, paramName)
		{
			_message = message;
		}

		protected QueryArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_message = (string)info.GetValue(nameof(_message), typeof(string));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(_message), _message);

			base.GetObjectData(info, context);
		}
	}
}
