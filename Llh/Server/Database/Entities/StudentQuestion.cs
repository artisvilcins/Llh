using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Llh.Server.Database.Entities
{
    [Table("StudentQuestions")]
    public class StudentQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column]
        public string Question { get; set; }

        [Column]
        public bool IsAnsered { get; set; }

        [Column]
        public DateTime Created { get; set; }
    }
}
