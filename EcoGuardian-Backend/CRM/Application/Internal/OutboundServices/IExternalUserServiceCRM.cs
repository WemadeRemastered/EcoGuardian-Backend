namespace EcoGuardian_Backend.CRM.Application.Internal.OutboundServices;

public interface IExternalUserServiceCRM
{
        Task<int> CheckUserExists(string userId);
        Task<bool> CheckPlantExists(int plantId);
}