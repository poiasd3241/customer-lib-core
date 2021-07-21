using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Business.Exceptions
{
	/// <summary>
	/// Throw when the entity fails validation.
	/// </summary>
	[Serializable]
	public class InternalValidationException : ValidationException
	{
		public InternalValidationException(IEnumerable<ValidationFailure> errors) : base(errors) { }

		public InternalValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
