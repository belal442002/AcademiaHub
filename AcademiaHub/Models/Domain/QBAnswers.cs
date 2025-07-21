using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class QBAnswers
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
        public string? Text { get; set; }
        public bool Answer_TF { get; set; }

        // Navigation Properties
        public virtual QuestionBank? Question { get; set; }
    }
}
