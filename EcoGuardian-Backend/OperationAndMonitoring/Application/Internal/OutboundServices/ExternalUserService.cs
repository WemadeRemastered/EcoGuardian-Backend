using EcoGuardian_Backend.IAM.Interfaces.ACL;

namespace EcoGuardian_Backend.OperationAndMonitoring.Application.Internal.OutboundServices;

public class ExternalUserService(IIamContextFacade iamContextFacade) : IExternalUserService
{
    public async Task<int> CheckUserExists(string userId)
    {
        return await iamContextFacade.UsersExists(userId);
    }
}