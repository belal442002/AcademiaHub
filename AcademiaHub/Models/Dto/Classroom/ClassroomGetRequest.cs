using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Classroom
{
    public class ClassroomGetRequest
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
