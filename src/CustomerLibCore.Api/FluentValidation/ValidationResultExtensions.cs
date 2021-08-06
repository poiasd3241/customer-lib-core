using CustomerLibCore.Api.Exceptions;
using FluentValidation.Results;

namespace CustomerLibCore.Api.FluentValidation
{
	public static class ValidationResultExtensions
	{
		/// <summary>
		/// If the result is invalid, throws the <see cref="InvalidBodyException"/> 
		/// containing the <see cref="ValidationResult.Errors"/> of the result;
		/// otherwise, does nothing.
		/// </summary>
		/// <param name="result">The validation result to check.</param>
		public static void WithInvalidBodyException(this ValidationResult result)
		{
			if (result.IsValid == false)
			{
				throw new InvalidBodyException(result.Errors);
			}
		}
	}
}
