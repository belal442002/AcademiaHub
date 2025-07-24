using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto
{
    public class ResetPasswordByUserIdRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
