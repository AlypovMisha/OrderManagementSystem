using CatalogService.Application.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace CatalogService.Presentation.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new { error = exception.Message });

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case ExceptionNameAlreadyExists:
                    code = HttpStatusCode.Conflict;
                    result = JsonSerializer.Serialize(new { error = "Product name already exists." });
                    break;
                case ProductNotFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = "Product not found." });
                    break;
                case QuantityException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = "The requested quantity is more than what is in stock." });
                    break;
                case Exception:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { error = "Something went wrong." });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}