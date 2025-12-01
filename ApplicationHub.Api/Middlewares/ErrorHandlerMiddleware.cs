using System.Text.Json;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Exceptions;

namespace ApplicationHub.Api.Middlewares;

/// <summary>
/// Middleware that globally handles unhandled exceptions in the request pipeline.
/// Logs the error and returns a standardized JSON error response.
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger used to log exceptions.</param>
    /// <param name="next">The next middleware in the ASP.NET Core pipeline.</param>
    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware and handles exceptions that occur in the request pipeline.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception error)
        {
            ArgumentNullException.ThrowIfNull(context);

            var response = context.Response;
            response.ContentType = "application/json";
            var errorResponse = new ErrorResponse();

            switch (error)
            {
                case DaoDbException ex:
                    _logger.LogError(
                        "Data access db exception caught Message: {ErrorMessage}, Exception:{AllException}",
                        ex.Message,
                        ex);

                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.Errors.Add("Database Error.");
                    break;

                case ApiException ex:
                    _logger.LogError(
                        "Api exception caught Message: {ErrorMessage}, Exception:{AllException}",
                        ex.Message,
                        ex);

                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.Errors.Add("Api Error.");
                    break;

                default:
                    _logger.LogError(
                        "Exception Message : {ErrorMessage}, Exception:{AllException}",
                        error.Message,
                        error);

                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.Errors.Add("Internal Server Error.");
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result).ConfigureAwait(false);
        }
    }
}