using EcoGuardian_Backend.CRM.Application.Internal.OutboundServices;
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;
using EcoGuardian_Backend.CRM.Domain.Model.Commands;
using EcoGuardian_Backend.CRM.Domain.Repositories;
using EcoGuardian_Backend.CRM.Domain.Services;
using EcoGuardian_Backend.Shared.Domain.Repositories;

namespace EcoGuardian_Backend.CRM.Application.Internal.CommandServices;

public class AnswerCommandService(
    IAnswerRepository answerRepository, 
    IUnitOfWork unitOfWork,
    IAddedQuestionEventHandler eventHandler, IExternalUserServiceCRM externalUserService) : IAnswerCommandService
{
    public async Task Handle(RegisterAnswerCommand command)
    {
        var userId = await externalUserService.CheckUserExists(command.SpecialistId);
        var answer = new Answer(
            command.QuestionId,
            command.AnswerText,
            userId
        );

        
        await answerRepository.AddAsync(answer);
        await unitOfWork.CompleteAsync();
        
        //Update the question state to Resolved when an answer is added
        await eventHandler.HandleAnswerAddedAsync(command.QuestionId);
    }

}