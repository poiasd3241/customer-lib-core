using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerLibCore.Api.Filters
{
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			context.ExceptionHandled = true;
			var exception = context.Exception;

			context.Result = exception switch
			{
				// Not found
				NotFoundException => FromNotFoundException(),

				// Paged resource request invalid
				PagedRequestInvalidException ex => FromPagedRequestInvalidException(ex),

				// Conflict between incoming and existing data
				ConflictWithExistingException ex => FromConflictWithExistingException(ex),

				// Invalid route/query arguments
				RouteArgumentException ex => FromRouteArgumentException(ex),

				// Invalid body
				InvalidBodyException ex => FromInvalidBodyException(ex),

				// Internal error
				_ => FromInternalServerError(),
			};

			base.OnException(context);
		}

		private static ObjectResult FromNotFoundException()
		{
			var statusCode = StatusCodes.Status404NotFound;

			var errorModel = new ErrorModel(statusCode, "Resource not found");

			return MakeObjectResult(errorModel, statusCode);
		}

		private static ObjectResult FromPagedRequestInvalidException(
			PagedRequestInvalidException ex)
		{
			var statusCode = StatusCodes.Status400BadRequest;

			var errorModel = new ErrorModel(statusCode,
				$"Paged resource request is invalid (page = {ex.Page}, pageSize = {ex.PageSize})");

			return MakeObjectResult(errorModel, statusCode);
		}
		private static ObjectResult FromConflictWithExistingException(
			ConflictWithExistingException ex)
		{
			var statusCode = StatusCodes.Status409Conflict;

			var errorModel = new ErrorModel(statusCode,
			$"Conflict between the incoming and existing data " +
			$"({ex.IncomingPropertyName} = '{ex.IncomingPropertyValue}'). Reason: {ex.ConflictMessage}");

			return MakeObjectResult(errorModel, statusCode);
		}

		private static ObjectResult FromRouteArgumentException(RouteArgumentException ex)
		{
			var statusCode = StatusCodes.Status400BadRequest;

			var errorModel = new ErrorModel(statusCode,
				$"Invalid route/query parameter '{ex.ParamName}' value ({ex.Message})");

			return MakeObjectResult(errorModel, statusCode);
		}

		private static ObjectResult FromInvalidBodyException(InvalidBodyException ex)
		{
			var statusCode = StatusCodes.Status400BadRequest;

			var errorModel = new ErrorModel(statusCode, ex.ValidationErrorsMessage);

			return MakeObjectResult(errorModel, statusCode);
		}

		private static ObjectResult FromInternalServerError()
		{
			var statusCode = StatusCodes.Status500InternalServerError;

			var errorModel = new ErrorModel(statusCode, "Internal server error");

			return MakeObjectResult(errorModel, statusCode);
		}

		private static ObjectResult MakeObjectResult(object value, int statusCode) =>
			new(value) { StatusCode = statusCode };
	}
}
