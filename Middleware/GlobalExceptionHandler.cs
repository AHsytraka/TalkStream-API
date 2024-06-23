using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TalkStream_API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        httpContext.Response.StatusCode = exception switch
        {
            // Check for specific exception types and set appropriate status code
            NullReferenceException => (int) HttpStatusCode.NotFound,
            TimeoutException => (int)HttpStatusCode.RequestTimeout,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            BadHttpRequestException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var details = new ProblemDetails()
        {
            Detail = $"{exception.Message}",
            Instance = "API",
            Status = httpContext.Response.StatusCode,
            Title = "API Error",
            Type = httpContext.GetType().Name
        };
        
        var response = JsonSerializer.Serialize(details);
        httpContext.Response.ContentType = "application/json";
        
        await httpContext.Response.WriteAsync(response, cancellationToken);
        return true;
    }
}