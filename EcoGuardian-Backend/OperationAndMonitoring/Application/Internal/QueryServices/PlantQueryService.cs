using EcoGuardian_Backend.OperationAndMonitoring.Application.Internal.OutboundServices;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Aggregates;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Queries;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Repositories;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Services;

namespace EcoGuardian_Backend.OperationAndMonitoring.Application.Internal.QueryServices;

public class PlantQueryService(IPlantRepository plantRepository, IExternalUserService externalUserService) : IPlantQueryService
{
    public async Task<IEnumerable<Plant>> Handle(GetPlantsByUserIdQuery query)
    {
        var userId = await externalUserService.CheckUserExists(query.UserId);
        return await plantRepository.GetPlantsByUserIdAsync(userId);
    }

    public async Task<Plant> Handle(GetPlantByIdQuery query)
    {
        var plant = await plantRepository.GetByIdAsync(query.Id);
        return plant ?? throw new KeyNotFoundException($"Plant with ID {query.Id} not found.");
    }
}