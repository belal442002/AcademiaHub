using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.Now;
        public bool Active_TF { get; set; } = true;

        // Navigation Properties
        public virtual IdentityUser? Account { get; set; }
        public virtual List<Student_Classroom>? Student_Classrooms { get; set; }
        public virtual List<Student_QuestionsForm>? Student_QuestionsForms { get; set; }
        public virtual List<FormStudentAnswers>? FormStudentAnswers { get; set; }

    }
}
