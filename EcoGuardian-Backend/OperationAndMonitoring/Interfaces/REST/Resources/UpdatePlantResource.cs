namespace EcoGuardian_Backend.OperationAndMonitoring.Interfaces.REST.Resources;

public record UpdatePlantResource( 
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