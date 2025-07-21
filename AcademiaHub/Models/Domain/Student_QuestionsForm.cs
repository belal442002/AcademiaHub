using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Student_QuestionsForm
    {
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(QuestionsForm))]
        public int QuestionsFormId { get; set; }
        public double Grade { get; set; }

        // Navigation Properties

        public virtual Student? Studnet { get; set; }
        public virtual QuestionsForm? QuestionsForm { get; set; }
    }
}
