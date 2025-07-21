using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.Teacher_Classroom
{
    public class Teacher_ClassroomAddRequest
    {
        public Guid TeacherId { get; set; }
        public int ClassroomId { get; set; }
    }
}
