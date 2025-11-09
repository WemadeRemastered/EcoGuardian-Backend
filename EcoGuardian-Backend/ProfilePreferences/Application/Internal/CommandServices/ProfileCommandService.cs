using System.Text.Json;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcoGuardian_Backend.ProfilePreferences.Domain.Model.Aggregates;
using EcoGuardian_Backend.ProfilePreferences.Domain.Model.Commands;
using EcoGuardian_Backend.ProfilePreferences.Domain.Model.Services;
using EcoGuardian_Backend.ProfilePreferences.Domain.Repositories;
using EcoGuardian_Backend.ProfilePreferences.Domain.Services;
using EcoGuardian_Backend.Shared.Application.Internal.CloudinaryStorage;
using EcoGuardian_Backend.Shared.Domain.Repositories;

namespace EcoGuardian_Backend.ProfilePreferences.Application.Internal.CommandServices;

public class ProfileCommandService(IProfileRepository profileRepository, IUnitOfWork unitOfWork, ICloudinaryStorage cloudinaryStorage) : IProfileCommandService
{
    public async Task Handle(CreateProfileCommand command)
    {
        if(profileRepository.IsEmailExists(command.Email))
        {
            throw new BadHttpRequestException($"Profile with email {command.Email} already exists.");
        }

        if (profileRepository.IsUserNameExists(command. Name+ command.Email + command.LastName))
        {
            throw new BadHttpRequestException($"Profile with username {command.Name} {command.LastName} already exists.");
        }
        var profile = new Profile(command);
        await profileRepository.AddAsync(profile);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(UpdateProfileCommand command)
    {
        var profile = await profileRepository.GetByIdAsync(command.Id);
        if (profile == null)
        {
            throw new BadHttpRequestException($"Profile with ID {command.Id} not found.");
        }

        if (command.AvatarUrl != null)
        {
            using var stream = new MemoryStream();
            await command.AvatarUrl.CopyToAsync(stream);
            if (stream.Length > 5 * 1024 * 1024)
            {
                throw new BadHttpRequestException("Avatar image size exceeds the 5 MB limit.");
            }
            stream.Position = 0; 
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(command.AvatarUrl.FileName, stream),
                PublicId = $"{profile.UserId}/profile/avatar/{command.AvatarUrl.FileName}",
                Overwrite = true,
            };
            await cloudinaryStorage.UploadImage(imageUploadParams);
            var url = await cloudinaryStorage.GetImage($"{profile.UserId}/profile/avatar/{command.AvatarUrl.FileName}");
            profile.UpateImage(url ?? "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Default_pfp.svg/2048px-Default_pfp.svg.png");
        }
        
        profile.Update(command);
        profileRepository.Update(profile);
        await unitOfWork.CompleteAsync();
    }
}