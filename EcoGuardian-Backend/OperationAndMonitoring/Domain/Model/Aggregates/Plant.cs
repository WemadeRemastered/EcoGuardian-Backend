using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Commands;
using EcoGuardian_Backend.Shared.Interfaces.Helpers;

namespace EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Aggregates;

public class Plant
{
    public int Id { get; }
    
    public string Name { get; private set; }
    
    public string Image { get; set; } = string.Empty; 
    public string Type { get; private set; }
    public int AreaCoverage { get; private set; }
    public int UserId { get; private set; }
    public double WaterThreshold { get; private set; }
    public double LightThreshold { get; private set; }
    public double TemperatureThreshold { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsPlantation { get; private set; }
    public int WellnessStateId { get; private set; }

    public Plant()
    {
         Name = string.Empty;
        Type = string.Empty;
        AreaCoverage = 0;
        UserId = 0;
        WaterThreshold = 0.0;
        LightThreshold = 0.0;
        TemperatureThreshold = 0.0;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        UpdatedAt = null;
        IsPlantation = false;
        WellnessStateId = 0;
        Image = string.Empty;
    }

    public Plant(
        string type,
        string name,
        int areaCoverage,
        int userId,
        double waterThreshold,
        double lightThreshold,
        double temperatureThreshold,
        bool isPlantation,
        int wellnessStateId)
    {
        Type = type;
        Name = name;
        AreaCoverage = areaCoverage;
        UserId = userId;
        WaterThreshold = waterThreshold;
        LightThreshold = lightThreshold;
        TemperatureThreshold = temperatureThreshold;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        UpdatedAt = null;
        IsPlantation = isPlantation;
        WellnessStateId = wellnessStateId;
        Image = string.Empty;
    }

    public void Update(
        string type,
        string name,
        int areaCoverage,
        int userId,
        double waterThreshold,
        double lightThreshold,
        double temperatureThreshold,
        bool isPlantation,
        int wellnessStateId)
    {
        Type = type;
        Name = name;
        AreaCoverage = areaCoverage;
        UserId = userId;
        WaterThreshold = waterThreshold;
        LightThreshold = lightThreshold;
        TemperatureThreshold = temperatureThreshold;
        UpdatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        IsPlantation = isPlantation;
        WellnessStateId = wellnessStateId;
    }

    public void UpdateState(UpdatePlantStateCommand command)
    {
        WellnessStateId = command.StateId;
    }
    
}