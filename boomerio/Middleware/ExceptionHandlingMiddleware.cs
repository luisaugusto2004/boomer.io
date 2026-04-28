using boomerio.DTOs;
using boomerio.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace boomerio.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env
        )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request");
                await HandleException(context, 400, ex);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found");
                await HandleException(context, 404, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleException(context, 500, ex);
            }
        }

        private async Task HandleException(HttpContext context, int statusCode, Exception ex)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            if (statusCode == 500)
            {
                var problem = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = statusCode,
                    Detail = _env.IsDevelopment() ? ex.Message : "Internal server error",
                    Type = "ServerError"
                };

                await context.Response.WriteAsJsonAsync(problem);
                _logger.LogInformation(string.IsNullOrEmpty(ex.InnerException!.Message) ? ex.InnerException.Message : string.Empty);
                return;
            }

            var apiError = new ApiError(
                statusCode == 400 ? "BadRequest" : "NotFound",
                statusCode,
                ex.Message
            );

            await context.Response.WriteAsJsonAsync(apiError);
        }
    }
}
