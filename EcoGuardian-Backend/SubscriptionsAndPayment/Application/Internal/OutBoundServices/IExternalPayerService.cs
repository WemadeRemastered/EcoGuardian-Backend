namespace EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.OutBoundServices;

public interface IExternalPayerService
{
    Task<int> CheckExternalPayerExists(string externalPayerId);
}