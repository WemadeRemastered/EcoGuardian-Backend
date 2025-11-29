using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;
using EcoGuardian_Backend.CRM.Domain.Repositories;
using EcoGuardian_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcoGuardian_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoGuardian_Backend.CRM.Infrastructure.Persistence.EFC.Repositories
{
    public class AnswerRepository(AppDbContext context) : BaseRepository<Answer>(context), IAnswerRepository
    {
        public async Task<Answer?> GetAnswersByQuestionId(int questionId)
        {

            return await context.Set<Answer>()
                .Where(a => a.QuestionId == questionId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Answer>> GetAnswersBySpecialistId(int specialistId)
        {

            return await context.Set<Answer>()
                .Where(a => a.SpecialistId == specialistId)
                .ToListAsync();
        }
        
    }
}