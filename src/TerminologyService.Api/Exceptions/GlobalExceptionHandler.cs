using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TerminologyService.Api.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "Internal Server Error",
			Detail = "An unexpected error occurred.",
			Instance = httpContext.Request.Path
		};

		httpContext.Response.StatusCode = problemDetails.Status!.Value;
		
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
		
		return true;
	}
}
