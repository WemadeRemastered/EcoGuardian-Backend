
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;
using EcoGuardian_Backend.CRM.Domain.Repositories;
using EcoGuardian_Backend.CRM.Domain.Services;
using Microsoft.Extensions.Caching.Memory;
namespace EcoGuardian_Backend.CRM.Application.Internal.QueryServices
{
    public class AnswerQueryService(
        IAnswerRepository answerRepository,
        IMemoryCache cache
        ) : IAnswerQueryService
    {
        public async Task<Answer> GetAnswerByQuestionId(int questionId)
        {
            var cachedAnswer = cache.Get<Answer>(questionId);
            if (cachedAnswer != null) return cachedAnswer;
            var answer = await answerRepository.GetAnswersByQuestionId(questionId);
            if (answer == null)
            {
                throw new KeyNotFoundException($"No answers found for question ID {questionId}");
            }
            cache.Set(
                    questionId, 
                    answer,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60)
                        )
                    );
            return answer;

        }

        public Task<IEnumerable<Answer>> GetAnswersBySpecialistId(int userId)
        {

           throw new NotImplementedException("This method is unvailable in the current version.");

        }
    }
}