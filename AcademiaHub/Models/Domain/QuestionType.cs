using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Domain
{
    public class QuestionType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        // Navigation Properties
        public virtual List<QuestionBank>? Questions { get; set; }
    }
}
