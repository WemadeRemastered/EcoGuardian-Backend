using EcoGuardian_Backend.Shared.Interfaces.Helpers;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Commands;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.ValueObjects;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Aggregates;

public class Payment
{
    public int Id { get; private set; }
    
    public string PaymentIntentId { get; set; }
    
    public string PaymentMethodId { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public string Currency { get; private set; }
    
    public string PaymentStatus { get; set; }
    
    public int UserId { get; private set; }
    
    public int ReferenceId { get; set; }
    
    public string ReferenceType { get; set; } = EPaymentReferenceType.None.ToString(); // Default value for ReferenceType
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }

    public Payment()
    {
        CreatedAt = DateTime.UtcNow;
        PaymentStatus = "Pending";
        PaymentIntentId = string.Empty;
        PaymentMethodId = string.Empty;
        Currency = string.Empty;
        UpdatedAt = null;
        Amount = 0;
        UserId = 0;
        ReferenceId = 0;
        ReferenceType = EPaymentReferenceType.None.ToString(); // Default value for ReferenceType
        // we will use the enum and convert it to string when we need to return it as a resource or persist it in the database.
    }

    public Payment(
        string paymentIntentId,
        string paymentMethodId,
        decimal amount,
        string currency,
        string paymentStatus,
        int userId,
        int referenceId,
        string referenceType)
    {
        PaymentIntentId = paymentIntentId;
        PaymentMethodId = paymentMethodId;
        Amount = amount;
        Currency = currency;
        PaymentStatus = paymentStatus;
        UserId = userId;
        ReferenceId = referenceId;
        ReferenceType = referenceType;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        UpdatedAt = null;
    }

    public Payment(CreatePaymentIntentCommand command)
    {
        Amount = command.Amount;
        Currency = command.Currency;
        PaymentMethodId = command.PaymentMethodId;
    }

    public Payment(ConfirmPaymentIntentCommand command)
    {
        PaymentIntentId = command.PaymentIntentId;
        PaymentStatus = "Confirmed";
    }
    
    public void UpdatePayment(UpdatePaymentCommand command)
    {
        Id = command.Id;
        Amount = command.Amount;
        Currency = command.Currency;
        PaymentIntentId = command.PaymentIntentId;
        PaymentMethodId = command.PaymentMethodId;
        PaymentStatus = command.PaymentStatus;
        UserId = command.UserId;
        ReferenceId = command.ReferenceId;
        ReferenceType = command.ReferenceType;
    }
}