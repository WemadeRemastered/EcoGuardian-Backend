using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcoGuardian_Backend.CRM.Application.Internal.OutboundServices;
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;
using EcoGuardian_Backend.CRM.Domain.Model.Commands;
using EcoGuardian_Backend.CRM.Domain.Repositories;
using EcoGuardian_Backend.CRM.Domain.Services;
using EcoGuardian_Backend.Shared.Application.Internal.CloudinaryStorage;
using EcoGuardian_Backend.Shared.Domain.Repositories;

namespace EcoGuardian_Backend.CRM.Application.Internal.CommandServices;

public class QuestionCommandService(IQuestionRepository questionRepository, IUnitOfWork unitOfWork, IExternalUserServiceCRM externalUserService, ICloudinaryStorage cloudinaryStorage) : IQuestionCommandService
{
    public async Task<Question> Handle(RegisterQuestionCommand command)
    {
        // Check if the user exists before adding the question
        var userId = await externalUserService.CheckUserExists(command.UserId);

// Check if the plant exists
        if (!await externalUserService.CheckPlantExists(command.PlantId))
        {
            throw new BadHttpRequestException($"Plant with ID {command.PlantId} does not exist.");
        }

        var question = new Question(
            command.Title,
            command.Content,
            userId,
            command.PlantId
        );
        if (command.ImageUrls != null && command.ImageUrls.Any())
        {
            var imageUrls = new List<string>();
            foreach (var image in command.ImageUrls)
            {
                var publicId = $"{command.UserId}/{command.PlantId}/{Guid.NewGuid()}";
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(image.FileName, image.OpenReadStream()),
                    PublicId = publicId,
                    Overwrite = true,
                    AllowedFormats = ["jpg", "png", "gif", "webp"],
                };
                await cloudinaryStorage.UploadImage(uploadParams);
                var result = await cloudinaryStorage.GetImage(publicId);
                imageUrls.Add(result ?? string.Empty);
            }
            question.AddImages(imageUrls);
        }
        await questionRepository.AddAsync(question);
        await unitOfWork.CompleteAsync();
        return question;
    }

    public async Task Handle(UpdateQuestionCommand command)
    {
        var question = await questionRepository.GetByIdAsync(command.QuestionId);
        if (question == null)
        {
            throw new BadHttpRequestException($"Question with ID {command.QuestionId} not found.");
        }

        question.UpdateState(command.State);
        questionRepository.Update(question);
        await unitOfWork.CompleteAsync();
    }
}