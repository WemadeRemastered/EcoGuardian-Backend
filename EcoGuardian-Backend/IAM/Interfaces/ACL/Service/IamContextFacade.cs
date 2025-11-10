

using EcoGuardian_Backend.IAM.Domain.Model.Commands;
using EcoGuardian_Backend.IAM.Domain.Model.Queries;
using EcoGuardian_Backend.IAM.Domain.Services;

namespace EcoGuardian_Backend.IAM.Interfaces.ACL.Service;

public class IamContextFacade(IUserQueryService userQueryService, IUserCommandService commandService) : IIamContextFacade
{

    public async Task<int> UsersExists(string userId)
    {
       var getUserByIdQuery = new GetUserByIdQuery(userId);
       var user = await userQueryService.Handle(getUserByIdQuery);
       if (user == null)
       {
           throw new Exception($"User with ID {userId} does not exist.");
       }
       return user.Id;
    }

    public async Task UpdateRoleId(int userId, int roleId)
    {
        var command = new UpdateRoleCommand(roleId, userId);
        await commandService.Handle(command);
    }
}