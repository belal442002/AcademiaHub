using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Domain
{
    public class Difficulty
    {
        [Key]
        public int Id { get; set; }
        public string DifficultyLevel { get; set; }

        // Navigation Properties
        public virtual List<QuestionBank>? Questions { get; set; }
    }
}
