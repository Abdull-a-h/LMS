using System.Text.Json;
using FluentValidation;
using LMS.API.Models;
using LMS.Domain.Exceptions;

namespace LMS.API.Middleware;

/// <summary>
/// Single global handler registered first in the pipeline. Maps known exceptions to a
/// consistent JSON envelope and logs every exception via Serilog. Never leaks stack traces.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception ex)
    {
        var traceId = context.TraceIdentifier;
        _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", traceId);

        ErrorResponse response = ex switch
        {
            ValidationException ve => new ErrorResponse
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Error = "Validation failed.",
                TraceId = traceId,
                Errors = ve.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
            },
            InvalidCredentialsException ic => Build(StatusCodes.Status401Unauthorized, ic.Message, traceId),
            NotFoundException nf => Build(StatusCodes.Status404NotFound, nf.Message, traceId),
            BusinessRuleException br => Build(StatusCodes.Status422UnprocessableEntity, br.Message, traceId),
            UnauthorizedAccessException => Build(StatusCodes.Status403Forbidden, "Forbidden.", traceId),
            _ => Build(StatusCodes.Status500InternalServerError, "An unexpected error occurred.", traceId)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }

    private static ErrorResponse Build(int statusCode, string error, string traceId) => new()
    {
        Success = false,
        StatusCode = statusCode,
        Error = error,
        TraceId = traceId
    };
}
