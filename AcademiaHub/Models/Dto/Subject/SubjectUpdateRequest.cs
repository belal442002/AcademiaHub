using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.Subject
{
    public class SubjectUpdateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
