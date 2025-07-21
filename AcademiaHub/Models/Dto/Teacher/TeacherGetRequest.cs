using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Teacher
{
    public class TeacherGetRequest
    {
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfJoin { get; set; }
        
    }
}
