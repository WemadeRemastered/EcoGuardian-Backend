using EcoGuardian_Backend.IAM.Interfaces.ACL;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.OutBoundServices;

public class ExternalPayerService(IIamContextFacade iamContextFacade) : IExternalPayerService
{
    public async Task<int> CheckExternalPayerExists(string externalPayerId)
    {
        return await iamContextFacade.UsersExists(externalPayerId);
    }
}