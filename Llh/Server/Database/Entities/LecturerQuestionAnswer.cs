using Llh.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Llh.Server.Database.Entities
{
    [Table("LecturerQuestions")]
    public class LecturerQuestionAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column]
        public int QuestionId { get; set; }
        public LecturerQuestion Question { get; set; }

        [Column]
        public YesNoQuestionAnswer Answer { get; set; }
    }
}
