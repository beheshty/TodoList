using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.API.Middleware;

/// <summary>
/// Handles global exceptions and returns appropriate HTTP status codes and problem details.
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentNullException => (StatusCodes.Status400BadRequest, "A required argument was null."),
            KeyNotFoundException => (StatusCodes.Status400BadRequest, "A required resource was not found."),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "The operation is invalid."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        logger.LogError(exception, "Exception caught by global handler: {Message}", exception.Message);

        var isDevelopment = httpContext.RequestServices
            .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = isDevelopment ? exception.Message : "An error occurred. Please contact support.",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
} 