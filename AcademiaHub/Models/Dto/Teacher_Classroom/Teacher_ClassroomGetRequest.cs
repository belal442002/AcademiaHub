using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.Teacher_Classroom
{
    public class Teacher_ClassroomGetRequest
    {
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int ClassroomId { get; set; }
        public string ClassroomName { get; set; }
    }
}
