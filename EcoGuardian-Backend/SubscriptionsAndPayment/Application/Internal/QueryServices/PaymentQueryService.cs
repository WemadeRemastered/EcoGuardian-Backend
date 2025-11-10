using EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.OutBoundServices;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Aggregates;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Queries;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Repositories;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Services;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.QueryServices;

public class PaymentQueryService(IPaymentRepository paymentRepository, IExternalPayerService externalPayerService) : IPaymentQueryService
{
    public async Task<IEnumerable<Payment>> Handle(GetAllPayments query)
    {
        return await paymentRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Payment>> Handle(GetPaymentsByUserId query)
    {
        var userId = await externalPayerService.CheckExternalPayerExists(query.UserId);
        return await paymentRepository.GetPaymentsByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Payment>> Handle(GetPaymentsBySubscriptionType query)
    {
        return await paymentRepository.GetPaymentsBySubscriptionType(query.SubscriptionType);
    }

    public Task<Payment?> Handle(GetPaymentByPaymentIntentId query)
    {
        return paymentRepository.FindByPaymentIntentIdAsync(query.PaymentIntentId);
    }

    public Task<Payment?> Handle(GetPaymentById query)
    {
        return paymentRepository.GetPaymentByIdAsync(query.PaymentId);
    }
}