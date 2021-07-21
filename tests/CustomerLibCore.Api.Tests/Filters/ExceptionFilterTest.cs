using System;
using System.Collections.Generic;
using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Api.Filters;
using CustomerLibCore.Business.Exceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace CustomerLibCore.Api.Tests.Filters
{
	public class ExceptionFilterTest
	{
		[Fact]
		public void ShouldReturnNotFoundFromNotFoundException()
		{
			// Given
			var ex = new NotFoundException();

			var exceptionContext = GetExceptionContext(ex);

			var filter = new ExceptionFilter();

			// When
			filter.OnException(exceptionContext);

			// Then
			var result = Assert.IsType<ObjectResult>(exceptionContext.Result);
			var errorModel = Assert.IsType<ErrorModel>(result.Value);

			var statusCode = 404;
			Assert.Equal(statusCode, result.StatusCode);
			Assert.Equal(statusCode, errorModel.Code);

			Assert.Equal("Resource not found", errorModel.Message);
		}

		[Fact]
		public void ShouldReturnBadRequestFromPagedRequestInvalidException()
		{
			// Given
			var page = 5;
			var pageSize = 7;

			var ex = new PagedRequestInvalidException(page, pageSize);

			var exceptionContext = GetExceptionContext(ex);

			var filter = new ExceptionFilter();

			// When
			filter.OnException(exceptionContext);

			// Then
			var result = Assert.IsType<ObjectResult>(exceptionContext.Result);
			var errorModel = Assert.IsType<ErrorModel>(result.Value);

			var statusCode = 400;
			Assert.Equal(statusCode, result.StatusCode);
			Assert.Equal(statusCode, errorModel.Code);

			Assert.Equal("Paged resource request is invalid (page = 5, pageSize = 7)",
				errorModel.Message);
		}

		[Fact]
		public void ShouldReturnConflictFromConflictWithExistingException()
		{
			// Given
			var incomingPropertyName = "MyProp";
			var incomingPropertyValue = "veryBad";
			var conflictMessage = "Such value already exists";

			var ex = new ConflictWithExistingException(
				incomingPropertyName, incomingPropertyValue, conflictMessage);

			var exceptionContext = GetExceptionContext(ex);

			var filter = new ExceptionFilter();

			// When
			filter.OnException(exceptionContext);

			// Then
			var result = Assert.IsType<ObjectResult>(exceptionContext.Result);
			var errorModel = Assert.IsType<ErrorModel>(result.Value);

			var statusCode = 409;
			Assert.Equal(statusCode, result.StatusCode);
			Assert.Equal(statusCode, errorModel.Code);

			Assert.Equal("Conflict between the incoming and existing data (MyProp = 'veryBad'). " +
				"Reason: Such value already exists",
				errorModel.Message);
		}

		[Fact]
		public void ShouldReturnBadRequestFromRouteArgumentException()
		{
			// Given
			var paramName = "MyProp";
			var message = "Cannot be equal to 5";

			var ex = new RouteArgumentException(message, paramName);

			var exceptionContext = GetExceptionContext(ex);

			var filter = new ExceptionFilter();

			// When
			filter.OnException(exceptionContext);

			// Then
			var result = Assert.IsType<ObjectResult>(exceptionContext.Result);
			var errorModel = Assert.IsType<ErrorModel>(result.Value);

			var statusCode = 400;
			Assert.Equal(statusCode, result.StatusCode);
			Assert.Equal(statusCode, errorModel.Code);

			Assert.Equal("Invalid route/query parameter 'MyProp' value (Cannot be equal to 5)",
				errorModel.Message);
		}

		[Fact]
		public void ShouldReturnBadRequestFromInvalidBodyException()
		{
			// Given
			var propertyName1 = "name1";
			var propertyName2 = "name2";

			var errorMessage1 = "msg1";
			var errorMessage2 = "msg2";

			var errors = new List<ValidationFailure>() { new(propertyName1,errorMessage1),
				new(propertyName2,errorMessage2)};

			var ex = new InvalidBodyException(errors);

			var exceptionContext = GetExceptionContext(ex);

			var filter = new ExceptionFilter();

			// When
			filter.OnException(exceptionContext);

			// Then
			var result = Assert.IsType<ObjectResult>(exceptionContext.Result);
			var errorModel = Assert.IsType<ErrorModel>(result.Value);

			var statusCode = 400;
			Assert.Equal(statusCode, result.StatusCode);
			Assert.Equal(statusCode, errorModel.Code);

			Assert.Equal("Body validation errors:\r\n" +
				" -- name1: msg1\r\n" +
				" -- name2: msg2",
				errorModel.Message);
		}

		[Fact]
		public void ShouldReturnInternalServerErrorFromInternalServerError()
		{
			// Given
			var message = "whatever";

			var ex = new Exception(message);

			var exceptionContext = GetExceptionContext(ex);

			var filter = new ExceptionFilter();

			// When
			filter.OnException(exceptionContext);

			// Then
			var result = Assert.IsType<ObjectResult>(exceptionContext.Result);
			var errorModel = Assert.IsType<ErrorModel>(result.Value);

			var statusCode = 500;
			Assert.Equal(statusCode, result.StatusCode);
			Assert.Equal(statusCode, errorModel.Code);

			Assert.Equal("Internal server error", errorModel.Message);
		}

		private static ExceptionContext GetExceptionContext(Exception ex)
		{
			var actionContext = new ActionContext()
			{
				HttpContext = new DefaultHttpContext(),
				RouteData = new RouteData(),
				ActionDescriptor = new ActionDescriptor()
			};

			return new ExceptionContext(actionContext, new List<IFilterMetadata>())
			{
				Exception = ex
			};
		}
	}
}
