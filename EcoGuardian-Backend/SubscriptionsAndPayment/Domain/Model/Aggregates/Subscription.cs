using EcoGuardian_Backend.Shared.Interfaces.Helpers;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Commands;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Aggregates;

public class Subscription
{
    public int Id { get; }

    public int UserId { get; set; }

    public int SubscriptionTypeId { get; set; }

    public int SubscriptionStateId { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Currency { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime ExpirationDate { get; set; }

    public Subscription()
    {
        UserId = 0;
        SubscriptionTypeId = 0;
        SubscriptionStateId = 0;
        Amount = 0;
        Currency = string.Empty;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        UpdatedAt = null;
        ExpirationDate = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow.AddMonths(1));
    }

    public Subscription(
        int userId,
        int subscriptionTypeId,
        decimal amount,
        string currency)
    {
        UserId = userId;
        SubscriptionTypeId = subscriptionTypeId;
        SubscriptionStateId = 1; 
        Amount = amount;
        Currency = currency;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        UpdatedAt = null;
        ExpirationDate = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow.AddMonths(1)); 
    }

    public Subscription(
        int id,
        int userId,
        int subscriptionTypeId)
    {
        SubscriptionStateId = id;
        UserId = userId;
        SubscriptionTypeId = subscriptionTypeId;
        UpdatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
    }
}