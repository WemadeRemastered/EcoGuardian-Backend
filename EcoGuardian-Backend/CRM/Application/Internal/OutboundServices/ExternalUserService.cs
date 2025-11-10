using EcoGuardian_Backend.IAM.Interfaces.ACL;
using EcoGuardian_Backend.OperationAndMonitoring.Interfaces.ACL;


//En este contexto se usa para validar que el usuario existe al intentar generar, responder o eliminar(En un futuro) la pregunta
//Mas que nada valdiacion
namespace EcoGuardian_Backend.CRM.Application.Internal.OutboundServices;

public class ExternalUserServiceCRM(IIamContextFacade iamContextFacade, IMonitorinContextFacade monitorinContextFacade) : IExternalUserServiceCRM
{
    public async Task<int> CheckUserExists(string userId)
    {
        return await iamContextFacade.UsersExists(userId);
    }

    public async Task<bool> CheckPlantExists(int plantId)
    {
        return await monitorinContextFacade.CheckPlantExists(plantId);
    }

}