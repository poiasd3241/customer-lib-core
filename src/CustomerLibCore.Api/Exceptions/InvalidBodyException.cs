using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.Api.Exceptions
{
	[Serializable]
	public class InvalidBodyException : ValidationException
	{
		public string ValidationErrorsMessage { get; }

		public InvalidBodyException(IEnumerable<ValidationFailure> errors) : base(errors)
		{
			var arr = errors.Select(x =>
				$"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage}");
			ValidationErrorsMessage = "Body validation errors:" + string.Join(string.Empty, arr);
		}
		public InvalidBodyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ValidationErrorsMessage = (string)info.GetValue(nameof(ValidationErrorsMessage),
				typeof(string));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(ValidationErrorsMessage), ValidationErrorsMessage);

			base.GetObjectData(info, context);
		}
	}
}
