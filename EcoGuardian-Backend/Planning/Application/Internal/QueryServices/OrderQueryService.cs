using EcoGuardian_Backend.Planning.Application.Internal.OutboundServices;
using EcoGuardian_Backend.Planning.Domain.Model.Aggregates;
using EcoGuardian_Backend.Planning.Domain.Model.Queries;
using EcoGuardian_Backend.Planning.Domain.Repositories;
using EcoGuardian_Backend.Planning.Domain.Services;

namespace EcoGuardian_Backend.Planning.Application.Internal.QueryServices;

public class OrderQueryService(IOrderRepository repository, IExternalUserService externalUserService) : IOrderQueryService
{
    public async Task<IEnumerable<Order>> Handle(GetOrdersByConsumerIdQuery query)
    {
        var userId = await externalUserService.CheckUserExists(query.ConsumerId);
        return await repository.GetOrdersByConsumerIdAsync(userId);
    }

    public Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query)
    {
        return repository.GetAllOrdersAsync();
    }
}