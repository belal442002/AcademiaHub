using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto
{
    public class ResetPasswordByEmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
