namespace EcoGuardian_Backend.CRM.Interfaces.Rest.Resources;    public record RegisterQuestionResource(
        string UserId,
        int PlantId,
        string Title,
        string QuestionText,
        List<IFormFile>? ImageUrls
    );
