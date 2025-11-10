namespace EcoGuardian_Backend.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    
   Task<int> UsersExists(string userId);
   Task UpdateRoleId(int userId, int roleId);
}