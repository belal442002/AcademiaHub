using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.FormDetails
{
    public class FormDetailsAddRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }
}
