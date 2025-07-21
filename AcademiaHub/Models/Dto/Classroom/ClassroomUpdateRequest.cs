using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Classroom
{
    public class ClassroomUpdateRequest
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
