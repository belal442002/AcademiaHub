using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.Subject
{
    public class SubjectAddRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
