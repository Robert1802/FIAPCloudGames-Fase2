using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text.Json;

namespace FIAPCloudGames.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<ExceptionMiddleware>(); // Usa Serilog
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Continua para o próximo middleware
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ocorreu um erro não tratado.");

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = "Ocorreu um erro inesperado no servidor.",
                    Detailed = ex.Message // opcional: você pode esconder isso em produção
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
