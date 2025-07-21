using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Classroom
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation Properties
        public virtual List<Student_Classroom>? Student_Classrooms { get; set; }
        public virtual Teacher_Classroom? Teacher_Classroom { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual List<QuestionBank>? Questions { get; set; }
        public virtual List<QuestionsForm>? Forms { get; set; }
    }
}
