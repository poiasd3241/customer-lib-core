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

				// The last item (in the current context) cannot be deleted
				PreventDeleteLastException => FromPreventDeleteLastException(),

				// Paged resource request invalid
				PagedRequestInvalidException ex => FromPagedRequestInvalidException(ex),

				// Conflict between incoming and existing data
				ConflictWithExistingException ex => FromConflictWithExistingException(ex),

				// Invalid route argument
				RouteArgumentException ex => FromRouteArgumentException(ex),

				// Invalid query argument
				QueryArgumentException ex => FromQueryArgumentException(ex),

				// Invalid body
				InvalidBodyException ex => FromInvalidBodyException(ex),

				// Internal error
				_ => FromInternalServerError(),
			};

			base.OnException(context);
		}

		private static ObjectResult FromNotFoundException() =>
			Make(StatusCodes.Status404NotFound, "Resource not found");

		private static ObjectResult FromPreventDeleteLastException() =>
			Make(StatusCodes.Status409Conflict,
				"Delete impossible: the last item (in the current context) is preserved");

		private static ObjectResult FromPagedRequestInvalidException(
			PagedRequestInvalidException ex) =>
			Make(StatusCodes.Status400BadRequest,
				$"Paged resource request is invalid (page = {ex.Page}, pageSize = {ex.PageSize})");

		private static ObjectResult FromConflictWithExistingException(
			ConflictWithExistingException ex) =>
			Make(StatusCodes.Status409Conflict,
				$"Conflict between the incoming and existing data " +
				$"({ex.IncomingPropertyName} = '{ex.IncomingPropertyValue}'). " +
				$"Reason: {ex.ConflictMessage}");

		private static ObjectResult FromRouteArgumentException(RouteArgumentException ex) =>
			Make(StatusCodes.Status400BadRequest,
				$"Invalid route parameter '{ex.ParamName}' value ({ex.Message})");

		private static ObjectResult FromQueryArgumentException(QueryArgumentException ex) =>
			Make(StatusCodes.Status400BadRequest,
				$"Invalid query parameter '{ex.ParamName}' value ({ex.Message})");

		private static ObjectResult FromInvalidBodyException(InvalidBodyException ex) =>
			Make(StatusCodes.Status400BadRequest, ex.ValidationErrorsMessage);

		private static ObjectResult FromInternalServerError() =>
			Make(StatusCodes.Status500InternalServerError, "Internal server error");

		private static ObjectResult Make(int statusCode, string message) =>
			new(new ErrorModel(statusCode, message)) { StatusCode = statusCode };
	}
}
