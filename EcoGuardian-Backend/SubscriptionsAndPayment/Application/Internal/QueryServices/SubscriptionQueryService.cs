using EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.OutBoundServices;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Aggregates;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Queries;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Repositories;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Services;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository, IExternalPayerService externalPayerService) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetSubscriptionById query)
    {
        return await subscriptionRepository.FindByIdAsync(query.SubscriptionId);
    }

    public async Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query)
    {
        var userId = await externalPayerService.CheckExternalPayerExists(query.UserId);
        return await subscriptionRepository.FindByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptions query)
    {
        return await subscriptionRepository.ListAllAsync();
    }
}