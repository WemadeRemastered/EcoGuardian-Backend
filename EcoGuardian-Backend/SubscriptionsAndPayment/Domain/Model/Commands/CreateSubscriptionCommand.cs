namespace EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Commands;

public record CreateSubscriptionCommand(
    string UserId,
    int SubscriptionTypeId,
    int SubscriptionStateId,
    string Currency,
    decimal Amount
);