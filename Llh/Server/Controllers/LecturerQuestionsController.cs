using Llh.Server.Database;
using Llh.Server.Database.Entities;
using Llh.Server.Hubs;
using Llh.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Llh.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LecturerQuestionsController : ControllerBase
    {
        private readonly LlhContext _db;
        private readonly IHubContext<QuestionsHub> _questionsHub;
        private readonly ILogger<LecturerQuestionsController> _logger;

        public LecturerQuestionsController(LlhContext db,
            IHubContext<QuestionsHub> questionsHub,
            ILogger<LecturerQuestionsController> logger)
        {
            _db = db;
            _questionsHub = questionsHub;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<LecturerQuestionDto>> Get()
        {
            return await _db.LecturerQuestions
                .OrderByDescending(q => q.Created)
                .Select(q => new LecturerQuestionDto
                {
                    Id = q.Id,
                    Question = q.Question,
                    IsClosed = q.IsClosed,
                    Answers = q.Answers.Select(x => new LecturerQuestionAnswerDto
                    {
                        Id= x.Id,
                        Answer= x.Answer

                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<LecturerQuestion> Add(LecturerQuestionDto questionDto)
        {
            var question = new LecturerQuestion
            {
                Question = questionDto.Question,
                IsClosed = false,
                Created = DateTime.UtcNow
            };

            _db.LecturerQuestions.Add(question);
            await _db.SaveChangesAsync();

            questionDto.Id = question.Id;
            questionDto.Created = question.Created;

            await _questionsHub.Clients.All.SendAsync("NewLecturerQuestion", questionDto);

            return question;
        }

        [HttpPost("{id:int}/mark-closed")]
        public async Task<IActionResult> MarkClosed(int id)
        {
            var question = await _db.LecturerQuestions.FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            question.IsClosed = true;
            await _db.SaveChangesAsync();

            await _questionsHub.Clients.All.SendAsync("LecturerQuestionMarkClosed", new { Id = id });

            return Ok();
        }

        [HttpPost("{id}/answer/{answer}")]
        public async Task<IActionResult> AddAnswer(int id, YesNoQuestionAnswer answer)
        {
            var question = await _db.LecturerQuestions.FindAsync(id);

            if (question == null)
            {
                _logger.LogWarning("No question found by id: {questionId}", id);
                return BadRequest();
            }

            var questionAnswer = new LecturerQuestionAnswer
            {
                Answer = answer,
                QuestionId = question.Id,
            };

            _db.LecturerQuestionAnswers.Add(questionAnswer);
            await _db.SaveChangesAsync();

            await _questionsHub.Clients.All.SendAsync("LecturerQuestionAddAnswer", new LecturerQuestionAnswerDto
            {
                Id = questionAnswer.Id,
                Answer = questionAnswer.Answer,
                QuestionId = questionAnswer.QuestionId
            });

            return Ok();
        }
    }
}
