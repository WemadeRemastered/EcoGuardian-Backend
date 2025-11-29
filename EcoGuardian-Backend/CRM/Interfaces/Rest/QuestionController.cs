
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;
using EcoGuardian_Backend.CRM.Domain.Model.Commands;
using EcoGuardian_Backend.CRM.Domain.Services;
using EcoGuardian_Backend.CRM.Interfaces.Rest.Resources;
using EcoGuardian_Backend.CRM.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoGuardian_Backend.CRM.Interfaces.Rest
{
    [ApiController]
    [ProducesResponseType(500)]
    [Route("api/v1/questions")]
    public class QuestionController(
        IQuestionCommandService questionCommandService,
        IQuestionQueryService questionQueryService,
        IAnswerCommandService answerCommandService,
        IAnswerQueryService answerQueryService,
        IAddedQuestionEventHandler addedQuestionEventHandler) : ControllerBase
    {

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterQuestion([FromForm] RegisterQuestionCommand command)
        {
            var question = await questionCommandService.Handle(command);
            var questionResource = QuestionResourceFromEntityAssembler.ToResourceFromEntity(question);
            return Ok(questionResource);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionCommand command)
        {
            await questionCommandService.Handle(command);
            var question = await questionQueryService.GetQuestionById(command.QuestionId);
            var updatedQuestion = QuestionResourceFromEntityAssembler.ToResourceFromEntity(question);
            return Ok(updatedQuestion);
        }

        [HttpGet("{questionId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionById(int questionId)
        {
            var question = await questionQueryService.GetQuestionById(questionId);
            var questionResource = QuestionResourceFromEntityAssembler.ToResourceFromEntity(question);

            return Ok(questionResource);
        }

        // By uSer ID
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionsByUserId(int userId)
        {
            var questions = await questionQueryService.GetQuestionsByUserId(userId);
            var enumerable = questions as Question[] ?? questions.ToArray();
            if (enumerable.Length == 0)
            {
                return NotFound();
            }
            var questionsResource = enumerable.Select(QuestionResourceFromEntityAssembler.ToResourceFromEntity).ToList();
            return Ok(questionsResource);
        }

        // By Plant ID
        [HttpGet("plant/{plantId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionsByPlantId(int plantId)
        {
            var questions = await questionQueryService.GetQuestionsByPlantId(plantId);
            var enumerable = questions.ToList();
            if (enumerable.Count == 0)
            {
                return NotFound();
            }
            var questionsResource = enumerable.Select(QuestionResourceFromEntityAssembler.ToResourceFromEntity).ToList();
            return Ok(questionsResource);
        }

        [HttpGet("{questionId:int}/answers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            var answers = await answerQueryService.GetAnswerByQuestionId(questionId);
            var question = await questionQueryService.GetQuestionById(questionId);
            var answersResource = AnswerResourceFromEntityAssembler.FromEntity(answers, question);
            return Ok(answersResource);
        }

        [HttpPost("{questionId:int}/answers")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterAnswer(int questionId, [FromBody] RegisterAnswerResource resource)
        {
            var command = RegisterAnswerCommandFromResourceAssembler.toCommandFromResource(resource, questionId);

            await answerCommandService.Handle(command);
            await addedQuestionEventHandler.HandleAnswerAddedAsync(questionId);

            var answer = await answerQueryService.GetAnswerByQuestionId(questionId);

            var question = await questionQueryService.GetQuestionById(questionId);
            var answersResource = AnswerResourceFromEntityAssembler.FromEntity(answer, question);
            return Ok(answersResource);
        }

        //Get all questions
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await questionQueryService.GetAllQuestions();
            var enumerable = questions as Question[] ?? questions.ToArray();
            if (enumerable.Length == 0)
            {
                return Ok(new List<QuestionResource>());
            }
            var questionsResource = enumerable.Select(QuestionResourceFromEntityAssembler.ToResourceFromEntity).ToList();
            return Ok(questionsResource);
        }
        
        [HttpGet("answers/specialist")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSpecialistAnswers([FromQuery] int specialistId)
        {
            var answers = await answerQueryService.GetAnswersBySpecialistId(specialistId);
            var enumerable = answers as Answer[] ?? answers.ToArray();
            if (enumerable.Length == 0)
            {
                return Ok(new List<AnswerResource>());
            }

            List<AnswerResource?> answerResource = [];
            foreach (var answer in enumerable)
            {
                var question = await questionQueryService.GetQuestionById(answer.QuestionId);
                {
                    var answerResourceItem = AnswerResourceFromEntityAssembler.FromEntity(answer, question);
                    answerResource.Add(answerResourceItem);
                }
            }
            return Ok(answerResource);
        }
        

    }
}