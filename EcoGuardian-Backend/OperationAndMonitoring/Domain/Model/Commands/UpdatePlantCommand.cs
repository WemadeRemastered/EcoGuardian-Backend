namespace EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Commands;

public record UpdatePlantCommand(
      int Id,
      string Name,
      string Type,
      int AreaCoverage,
      string UserId,
      double WaterThreshold,
      double LightThreshold,
      double TemperatureThreshold,
      bool IsPlantation,
      int WellnessStateId
    );