using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace FIAPCloudGames.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ExceptionMiddleware(RequestDelegate next, TelemetryClient telemetryClient, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _telemetryClient = telemetryClient;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro não tratado.");
                _telemetryClient.TrackException(ex);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = "Ocorreu um erro inesperado no servidor.",
                    Detailed = ex.Message
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}