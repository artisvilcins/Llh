using Llh.Server.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Llh.Server.Database
{
    public class LlhContext : DbContext
    {
        public LlhContext(DbContextOptions<LlhContext> options) : base(options)
        {

        }

        public DbSet<StudentQuestion> StudentQuestions { get; set; }
        public DbSet<LecturerQuestion> LecturerQuestions { get; set; }
        public DbSet<LecturerQuestionAnswer> LecturerQuestionAnswers { get; set; }
    }
}
