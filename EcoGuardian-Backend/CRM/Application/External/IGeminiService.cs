namespace EcoGuardian_Backend.CRM.Application.External;

public interface IGeminiService
{
    Task <string> GetGeminiResponseAsync(string prompt);
}