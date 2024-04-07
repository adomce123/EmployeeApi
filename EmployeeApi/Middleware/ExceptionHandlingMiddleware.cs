using EmployeeApi.Core.Exceptions;

namespace EmployeeApi.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException notFoundException)
            {
                _logger.LogError(notFoundException, "Resource not found.");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await JsonSerializer.SerializeAsync(httpContext.Response.Body, new
                {
                    error = notFoundException.Message
                });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                _logger.LogError(invalidOperationException, "Invalid operation.");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await JsonSerializer.SerializeAsync(httpContext.Response.Body, new
                {
                    error = invalidOperationException.Message
                });
            }
            catch (DbUpdateException ex)
            {
                string errorMessage = "Query failed while trying to update database. " +
                    "Check if employee with the role of CEO already exists.";
                _logger.LogError(ex, errorMessage);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await JsonSerializer.SerializeAsync(httpContext.Response.Body, new
                {
                    error = errorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occurred.");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await JsonSerializer.SerializeAsync(httpContext.Response.Body, new
                {
                    error = "An internal server error has occurred."
                });
            }
        }

    }
}
