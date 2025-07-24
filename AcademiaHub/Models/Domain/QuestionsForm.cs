using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class QuestionsForm
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Classroom))]
        public int ClassroomId { get; set; }
        [ForeignKey(nameof(FormDetails))]
        public int FormDetailsId { get; set; }
        [ForeignKey(nameof(FormType))]
        public int FormTypeId { get; set; }
        public bool Active { get; set; } = false;

        // Navigation Properties
        public virtual List<Student_QuestionsForm>? Student_QuestionsForms { get; set; }
        public virtual Classroom? Classroom { get; set; }
        public virtual FormDetails? FormDetails { get; set; }
        public virtual FormType? FormType { get; set; }
        public virtual List<Form_Questions>? Form_Questions { get; set; }
        public virtual List<FormStudentAnswers>? FormStudentAnswers { get; set; }

    }
}
