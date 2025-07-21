using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Domain
{
    public class FormDetails
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }
}
