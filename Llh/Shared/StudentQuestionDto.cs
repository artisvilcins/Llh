using System;

namespace Llh.Shared
{
    public class StudentQuestionDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsAnsered { get; set; }
        public DateTime Created { get; set; }
    }
}
