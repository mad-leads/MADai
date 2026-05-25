using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using MADai.Application.Common;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MADai.Api.Middleware;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;

	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (ValidationFailedException vex)
		{
			await WriteAsync(context, HttpStatusCode.BadRequest, vex.Message, vex.Errors);
		}
		catch (NotFoundException nex)
		{
			await WriteAsync(context, HttpStatusCode.NotFound, nex.Message);
		}
		catch (ForbiddenException fex)
		{
			await WriteAsync(context, HttpStatusCode.Forbidden, fex.Message);
		}
		catch (ConflictException cex)
		{
			await WriteAsync(context, HttpStatusCode.Conflict, cex.Message);
		}
		catch (AppException aex)
		{
			await WriteAsync(context, HttpStatusCode.BadRequest, aex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception");
			await WriteAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
		}
	}

	private static Task WriteAsync(HttpContext context, HttpStatusCode status, string error, IReadOnlyList<string>? issues = null)
	{
		context.Response.StatusCode = (int)status;
		context.Response.ContentType = "application/json";
		ApiResult payload = ApiResult.Fail(error, issues);
		return context.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		}));
	}
}
