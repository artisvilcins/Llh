namespace Llh.Shared
{
    public class LecturerQuestionAnswerDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public LecturerQuestionDto Question { get; set; }

        public YesNoQuestionAnswer Answer { get; set; }
    }
}
