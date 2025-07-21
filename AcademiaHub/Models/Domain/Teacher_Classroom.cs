using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Teacher_Classroom
    {
        [Key]
        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        [ForeignKey(nameof(Classroom))]
        public int ClassroomId { get; set; }

        // Navigation Properties
        public virtual Teacher? Teacher { get; set; }
        public virtual Classroom? Classroom { get; set; }
    }
}
