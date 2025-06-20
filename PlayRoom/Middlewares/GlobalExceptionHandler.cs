using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PlayRoom.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var isApiRequest = context.Request.Headers["Accept"].ToString().Contains("application/json")
                               || context.Request.Path.StartsWithSegments("/api");

            if (isApiRequest)
            {
                context.Response.ContentType = "application/json";
                int statusCode = StatusCodes.Status500InternalServerError;
                string message = "An unexpected error occurred.";

                switch (exception)
                {
                    case ArgumentNullException argNullEx:
                        statusCode = StatusCodes.Status400BadRequest;
                        message = $"Missing argument: {argNullEx.ParamName}";
                        break;
                    case UnauthorizedAccessException:
                        statusCode = StatusCodes.Status401Unauthorized;
                        message = "You are not authorized to perform this action.";
                        break;
                    case KeyNotFoundException:
                        statusCode = StatusCodes.Status404NotFound;
                        message = "The requested resource was not found.";
                        break;
                }

                context.Response.StatusCode = statusCode;

                var result = System.Text.Json.JsonSerializer.Serialize(new { error = message });
                await context.Response.WriteAsync(result);
            }
            else
            {
                context.Response.Redirect("/Home/Error");
            }
        }

    }

}