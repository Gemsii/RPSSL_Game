using System.Text.Json;
using RPSSL.API.Domain.Exceptions;
using RPSSL.API.Infrastructure.External.Exceptions;

namespace RPSSL.API.Middleware
{
    /// <summary>
    /// Catches unhandled exceptions and maps them to appropriate HTTP responses.
    /// <list type="bullet">
    ///   <item><see cref="PlayerNotFoundException"/> → 404 Not Found</item>
    ///   <item><see cref="PlayerAlreadyExistsException"/> → 409 Conflict</item>
    ///   <item><see cref="DomainException"/> → 400 Bad Request</item>
    ///   <item><see cref="ExternalServiceUnavailableException"/> → 502 Bad Gateway</item>
    ///   <item>Everything else → 500 Internal Server Error</item>
    /// </list>
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

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
            catch (PlayerNotFoundException ex)
            {
                await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (PlayerAlreadyExistsException ex)
            {
                await WriteResponse(context, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (DomainException ex)
            {
                await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ExternalServiceUnavailableException ex)
            {
                _logger.LogError(ex, "External service unavailable.");
                await WriteResponse(context, StatusCodes.Status502BadGateway, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                await WriteResponse(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        private static Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(new { error = message }, JsonOptions);
            return context.Response.WriteAsync(body);
        }
    }
}
