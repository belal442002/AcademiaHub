using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password must match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
