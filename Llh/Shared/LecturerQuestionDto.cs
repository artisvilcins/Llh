using System;
using System.Collections.Generic;

namespace Llh.Shared
{
    public class LecturerQuestionDto
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public DateTime Created { get; set; }

        public bool IsClosed { get; set; }

        public List<LecturerQuestionAnswerDto> Answers { get; set; }
    }
}
