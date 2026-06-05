using FinananzasAPI.Domain.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FinananzasAPI.API.Middlewares
{
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

                _logger.LogError(ex, "Excepion no manejada: {Message}", ex.Message);
                await HandledExeptionAsync(context, ex);
            }
        }

        public async Task HandledExeptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, message) = ex switch
            {
                InvalidOperationException => (HttpStatusCode.Conflict, ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, ex.Message)

            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(message);
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });


            await context.Response.WriteAsync(json);
        }
    }
}
