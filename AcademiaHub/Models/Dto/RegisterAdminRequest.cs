using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto
{
    public class RegisterAdminRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and ConfirmPassword must be identical")]
        public string ConfirmPassword { get; set; }
    }
}
