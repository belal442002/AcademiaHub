using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.Teacher_Classroom
{
    public class Teacher_ClassroomAddRequest
    {
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public int ClassroomId { get; set; }
    }
}
