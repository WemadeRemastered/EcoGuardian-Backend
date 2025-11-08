namespace EcoGuardian_Backend.IAM.Application.Internal.OutboundServices;

public interface IAuth0ManagementService
{
    Task<string> CreateUserAsync(string email, string auth0UserId);
    Task AssignRoleToUserAsync(string auth0UserId, string roleName);
    Task<string> GetUserRoleAsync(string auth0UserId);
}
