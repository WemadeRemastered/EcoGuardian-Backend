using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;
using EcoGuardian_Backend.Shared.Domain.Repositories;

namespace EcoGuardian_Backend.CRM.Domain.Repositories
{
    public interface IAnswerRepository: IBaseRepository<Answer>
    {
        Task<Answer> GetAnswersByQuestionId(int questionId);
        Task<IEnumerable<Answer>> GetAnswersBySpecialistId(int specialistId);
    }
}