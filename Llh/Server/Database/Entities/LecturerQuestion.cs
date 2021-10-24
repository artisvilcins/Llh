using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Llh.Server.Database.Entities
{
    [Table("LecturerQuestions")]
    public class LecturerQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column]
        public string Question { get; set; }

        [Column]
        public bool IsClosed { get; set; }

        [Column]
        public DateTime Created { get; set; }

        public List<LecturerQuestionAnswer> Answers { get; set; }
    }
}
