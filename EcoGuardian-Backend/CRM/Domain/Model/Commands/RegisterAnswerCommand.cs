

namespace EcoGuardian_Backend.CRM.Domain.Model.Commands;

public record RegisterAnswerCommand(
    int QuestionId,
    string SpecialistId
);
