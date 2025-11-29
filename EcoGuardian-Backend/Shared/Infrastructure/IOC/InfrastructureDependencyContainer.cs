using EcoGuardian_Backend.CRM.Application.External;
using EcoGuardian_Backend.IAM.Domain.Respositories;
using EcoGuardian_Backend.IAM.Infrastructure.Persistence.EFC.Respositories;
using EcoGuardian_Backend.CRM.Domain.Repositories;
using EcoGuardian_Backend.CRM.Infrastructure.External.Gemini;
using EcoGuardian_Backend.CRM.Infrastructure.Persistence.EFC.Repositories;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Repositories;
using EcoGuardian_Backend.OperationAndMonitoring.Infrastructure.Persistence.EFC.Repositories;
using EcoGuardian_Backend.Planning.Domain.Repositories;
using EcoGuardian_Backend.Planning.Infrastructure.Persistence.EFC.Repositories;
using EcoGuardian_Backend.ProfilePreferences.Domain.Repositories;
using EcoGuardian_Backend.ProfilePreferences.Infrastructure.Persistence.EFC.Repositories;
using EcoGuardian_Backend.Resources.Domain.Model.Aggregates;
using EcoGuardian_Backend.Resources.Domain.Repositories;
using EcoGuardian_Backend.Resources.Infrastructure.Persistence.EFC.Repositories;
using EcoGuardian_Backend.Shared.Domain.Repositories;
using EcoGuardian_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcoGuardian_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Entities;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Repositories;
using EcoGuardian_Backend.SubscriptionsAndPayment.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoGuardian_Backend.Shared.Infrastructure.IOC;

public static class InfrastructureDependencyContainer 
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, WebApplicationBuilder builder, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPlantRepository, PlantRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IWellnessStateRepository, WellnessStateRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ISubscriptionStateRepository, SubscriptionStateRepository>();
        services.AddScoped<ISubscriptionTypeRepository, SubscriptionTypeRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IGeminiService, GeminiService>();
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderStateRepository, OrderStateRepository>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IDeviceRepository, DeviceRepository>();

        services.AddDbContext<AppDbContext>(db =>
        {
            if (builder.Environment.IsDevelopment())
            {
                db.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
            }
            else if (builder.Environment.IsProduction())
            {
                db.UseMySql(configuration.GetConnectionString("ProductionConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("ProductionConnection")));
            }
        });
        return services;
    }
}
