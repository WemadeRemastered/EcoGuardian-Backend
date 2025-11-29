using System.Net.Http.Headers;
using System.Text.Json;
using EcoGuardian_Backend.CRM.Application.External;

namespace EcoGuardian_Backend.CRM.Infrastructure.External.Gemini;

public class GeminiService( 
    IConfiguration configuration,
    HttpClient httpClient
    ) : IGeminiService
{
private static string? ExtractTextFromJson(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.String)
            return element.GetString();
    
        if (element.ValueKind == JsonValueKind.Object)
        {
            var keys = new[] { "text", "message", "response", "content", "output", "candidates", "reply" };
            foreach (var key in keys)
            {
                if (element.TryGetProperty(key, out var prop))
                {
                    var found = ExtractTextFromJson(prop);
                    if (!string.IsNullOrEmpty(found)) return found;
                }
            }
            foreach (var found 
                     in element.EnumerateObject()
                         .Select(prop => 
                             ExtractTextFromJson(prop.Value)
                             ).Where(
                             found => !string.IsNullOrEmpty(found)
                             )
                     )
            {
                return found;
            }
        }
    
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                var found = ExtractTextFromJson(item);
                if (!string.IsNullOrEmpty(found)) return found;
            }
        }
    
        return null;
    }

   public async Task<string> GetGeminiResponseAsync(string prompt)
   {
       try
       {
           var apiKey = configuration["Gemini:ApiKey"];
           var requestBody = new
           {
               contents = new[] {
                   new {
                       parts = new[] {
                           new { text = prompt }
                       }
                   }
               }
           };
   
           var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
           httpClient.DefaultRequestHeaders.Remove("x-goog-api-key");
           httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
   
           var response = await httpClient.PostAsync(configuration["Gemini:Endpoint"], requestContent);
           var responseContent = await response.Content.ReadAsStringAsync();
   
           try
           {
               using var doc = JsonDocument.Parse(responseContent);
               var extracted = ExtractTextFromJson(doc.RootElement);
               return extracted ?? responseContent;
           }
           catch (JsonException)
           {
               return responseContent;
           }
       }
       catch (Exception ex)
       {
           throw new Exception("Error while communicating with Gemini API", ex);
       }
   }
   
    
}