using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Form_Questions
    {
        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(Form))]
        public int FormId { get; set; }

        // Navigation Properties
        public virtual QuestionsForm? Form { get; set; }
        public virtual QuestionBank? Question { get; set; }

    }
}
