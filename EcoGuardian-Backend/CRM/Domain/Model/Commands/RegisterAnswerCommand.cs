using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoGuardian_Backend.CRM.Domain.Model.Commands;

public record RegisterAnswerCommand(
    int QuestionId,
    string AnswerText,
    string SpecialistId
);
