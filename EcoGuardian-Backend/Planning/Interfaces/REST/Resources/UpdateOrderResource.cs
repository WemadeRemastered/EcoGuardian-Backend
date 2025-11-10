namespace EcoGuardian_Backend.Planning.Interfaces.REST.Resources;

public record UpdateOrderResource(
    string Action,
    int StateId,
    string ConsumerId,
    string? SpecialistId,
    DateTime? InstallationDate
);
