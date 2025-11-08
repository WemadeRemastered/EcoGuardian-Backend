using EcoGuardian_Backend.IAM.Infrastructure.Auth0.Configuration;
using EcoGuardian_Backend.Shared.Application.Helper;
using EcoGuardian_Backend.Shared.Interfaces.ASP.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EcoGuardian_Backend.Shared.Interfaces.IOC;

public static class InterfaceDependencyContainer
{
    public static IServiceCollection AddInterfaceDependencies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcoGuardian API", Version = "v1" });
            c.OperationFilter<FileUploadOperationFilter>();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        builder.Services.Configure<Auth0Settings>(builder.Configuration.GetSection("Auth0"));
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                
            });
        });
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers( options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));
        return services;
    }
}