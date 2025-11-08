using EcoGuardian_Backend.Resources.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace EcoGuardian_Backend.IAM.Infrastructure.Pipeline.Middleware.Components;

public class DeviceAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public DeviceAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration["DeviceApiKey"] ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context, IDeviceRepository deviceRepository)
    {
        if (context.Request.Path.StartsWithSegments("/api/v1/metrics"))
        {
            var deviceIdHeader = context.Request.Headers["Device-Id"].ToString();
            var apiKeyHeader = context.Request.Headers["Api-Key"].ToString();

            if (!int.TryParse(deviceIdHeader, out var deviceId) || string.IsNullOrEmpty(apiKeyHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Device authentication required");
                return;
            }

            var device = await deviceRepository.GetByIdAsync(deviceId);
            if (device == null || apiKeyHeader != _apiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid device credentials");
                return;
            }

            context.Items["DeviceId"] = deviceId;
        }

        await _next(context);
    }
}
