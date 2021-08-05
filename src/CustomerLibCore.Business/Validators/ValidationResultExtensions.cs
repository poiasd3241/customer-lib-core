using CustomerLibCore.Domain.Exceptions;
using FluentValidation.Results;

namespace CustomerLibCore.Domain.Validators
{
	public static class ValidationResultExtensions
	{
		/// <summary>
		/// If the result is invalid, throws the <see cref="InternalValidationException"/> 
		/// containing the <see cref="ValidationResult.Errors"/> of the result;
		/// otherwise, does nothing.
		/// </summary>
		/// <param name="result">The validation result to check.</param>
		public static void WithInternalValidationException(this ValidationResult result)
		{
			if (result.IsValid == false)
			{
				throw new InternalValidationException(result.Errors);
			}
		}
	}
}
