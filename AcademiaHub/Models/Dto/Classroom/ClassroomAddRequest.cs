using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Classroom
{
    public class ClassroomAddRequest
    {
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
