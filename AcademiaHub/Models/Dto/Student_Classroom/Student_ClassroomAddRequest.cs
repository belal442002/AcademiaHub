using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Student_Classroom
{
    public class Student_ClassroomAddRequest
    {
        public Guid StudentId { get; set; }
        public int ClassroomId { get; set; }
    }
}
