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
    public class StudentQuestionsController : ControllerBase
    {
        private readonly LlhContext _db;
        private readonly IHubContext<QuestionsHub> _questionsHub;
        private readonly ILogger<StudentQuestionsController> _logger;

        public StudentQuestionsController(LlhContext db,
            IHubContext<QuestionsHub> questionsHub,
            ILogger<StudentQuestionsController> logger)
        {
            _db = db;
            _questionsHub = questionsHub;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<StudentQuestion>> Get()
        {
            return await _db.StudentQuestions
                .OrderBy(q => q.IsAnsered)
                .ThenByDescending(q => q.Created)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<StudentQuestion> Add(StudentQuestionDto questionDto)
        {
            var question = new StudentQuestion
            {
                Question = questionDto.Question,
                IsAnsered = questionDto.IsAnsered,
                Created = DateTime.UtcNow
            };

            _db.StudentQuestions.Add(question);
            await _db.SaveChangesAsync();

            questionDto.Id = question.Id;
            questionDto.Created = question.Created;

            await _questionsHub.Clients.All.SendAsync("NewStudentQuestion", questionDto);

            return question;
        }

        [HttpPost("{id:int}/mark-ansered")]
        public async Task<IActionResult> MarkAnsered(int id)
        {
            var question = await _db.StudentQuestions.FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            question.IsAnsered = true;
            await _db.SaveChangesAsync();

            await _questionsHub.Clients.All.SendAsync("StudentQuestionAnswered", new { Id = id });

            return Ok();
        }
    }
}
