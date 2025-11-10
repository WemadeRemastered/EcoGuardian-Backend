namespace EcoGuardian_Backend.Planning.Application.Internal.OutboundServices;

public interface IExternalUserService
{
    Task<int> CheckUserExists(string userId);
}