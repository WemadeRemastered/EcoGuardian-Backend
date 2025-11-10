namespace EcoGuardian_Backend.Planning.Domain.Model.Commands;

public record UpdateOrderCommand(
    int Id, 
    string Action,
    int StateId,
    string ConsumerId,
    string? SpecialistId,
    DateTime? InstallationDate
);