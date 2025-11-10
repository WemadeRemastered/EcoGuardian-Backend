using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.ValueObjects;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Interfaces.REST.Resources;

public record CreatePaymentResource(
    string PaymentIntentId,
    string PaymentMethodId,
    decimal Amount,
    string Currency,
    string PaymentStatus,
    string UserId, 
    int ReferenceId,
    string ReferenceType
);