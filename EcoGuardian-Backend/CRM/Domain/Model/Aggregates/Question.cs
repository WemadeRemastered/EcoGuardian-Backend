using EcoGuardian_Backend.CRM.Domain.Model.Commands;
using EcoGuardian_Backend.CRM.Domain.Model.Entities;
using EcoGuardian_Backend.CRM.Domain.Model.ValueObjects;

namespace EcoGuardian_Backend.CRM.Domain.Model.Aggregates;

public class Question
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;    public int UserId { get; set; }
    public int PlantId { get; set; }
    public QuestionState State { get; set; } = QuestionState.Pending;
    
    // Navigation property para las imágenes
    public virtual ICollection<QuestionImage> Images { get; set; } = new List<QuestionImage>();

    public Question()
    {
    }   public Question(string title, string content, int userId, int plantId)
    {
        Title = title;
        Content = content;
        UserId = userId;
        PlantId = plantId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddImages(List<string> imageUrls)
    {
        if (imageUrls is null || !imageUrls.Any())
        {
            Console.WriteLine("No se proporcionaron imágenes para agregar.");
            return;
        }

        foreach (var url in imageUrls)
        {
            Images.Add(new QuestionImage(Id, url));
        }
    }

    public void UpdateState(QuestionState newState)
    {
        State = newState;
        UpdatedAt = DateTime.UtcNow;
    }
}
