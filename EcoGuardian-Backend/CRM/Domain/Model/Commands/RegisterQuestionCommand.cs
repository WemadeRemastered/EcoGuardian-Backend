using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EcoGuardian_Backend.CRM.Domain.Model.Commands;

public record RegisterQuestionCommand(
    string Title,
    string Content,
    string UserId,
    int PlantId,
    List<IFormFile>? ImageUrls
);
