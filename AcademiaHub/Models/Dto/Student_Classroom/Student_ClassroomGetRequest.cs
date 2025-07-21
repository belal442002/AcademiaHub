using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Student_Classroom
{
    public class Student_ClassroomGetRequest
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassroomName { get; set; }
        public int ClassroomId { get; set; }
    }
}
