namespace EcoGuardian_Backend.OperationAndMonitoring.Application.Internal.OutboundServices;

public interface IExternalUserService
{
    Task<int> CheckUserExists(string userId);
}