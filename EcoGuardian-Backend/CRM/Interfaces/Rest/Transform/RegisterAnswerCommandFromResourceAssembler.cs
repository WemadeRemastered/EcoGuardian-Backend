using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoGuardian_Backend.CRM.Domain.Model.Commands;
using EcoGuardian_Backend.CRM.Interfaces.Rest.Resources;

namespace EcoGuardian_Backend.CRM.Interfaces.Rest.Transform
{
    public class RegisterAnswerCommandFromResourceAssembler
    {
        public static RegisterAnswerCommand toCommandFromResource(
            RegisterAnswerResource resource,int questionId)
        {
            return new RegisterAnswerCommand(
                QuestionId: questionId,
                SpecialistId: resource.SpecialistId
            );
        }

    }
}