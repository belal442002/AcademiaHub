using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class QuestionBank
    {
        [Key]
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(Classroom))]
        public int ClassroomId { get; set; }
        [ForeignKey(nameof(Difficulty))]
        public int DifficultyId { get; set; }
        [ForeignKey(nameof(QuestionType))]
        public int QuestionTypeId { get; set; }
        public string QuestionText { get; set; }

        // Navigation Properties
        public virtual Classroom? Classroom { get; set; }
        public virtual Difficulty? Difficulty { get; set; }
        public virtual QuestionType? QuestionType { get; set; }
        public virtual List<QBAnswers>? Answers { get; set; }
        public virtual List<Form_Questions>? Form_Questions { get; set; }
        public virtual List<FormStudentAnswers>? FormStudentAnswers { get; set; }
    }
}
