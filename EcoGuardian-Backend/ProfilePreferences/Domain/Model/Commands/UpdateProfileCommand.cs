namespace EcoGuardian_Backend.ProfilePreferences.Domain.Model.Commands;

public record UpdateProfileCommand(int Id, string Name, string LastName, string Address, IFormFile? AvatarUrl);