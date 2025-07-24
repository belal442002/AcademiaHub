using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Student_Classroom
{
    public class Student_ClassroomAddRequest
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public int ClassroomId { get; set; }
    }
}
