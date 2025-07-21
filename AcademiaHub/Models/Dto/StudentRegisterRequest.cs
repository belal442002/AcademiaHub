using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto
{
    public class StudentRegisterRequest
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        
    }
}
