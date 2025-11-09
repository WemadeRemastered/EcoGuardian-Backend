namespace EcoGuardian_Backend.ProfilePreferences.Interfaces.REST.Resources;

public record UpdateProfileResource(string Name, string LastName, string Address, IFormFile? AvatarUrl);