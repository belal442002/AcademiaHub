using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Student_Classroom
    {
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(Classroom))]
        public int ClassroomId { get; set; }

        // Navigation Properties

        public virtual Student? Student { get; set; }
        public virtual Classroom? Classroom { get; set; }
    }
}
