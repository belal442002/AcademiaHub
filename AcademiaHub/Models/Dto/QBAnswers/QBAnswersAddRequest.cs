using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.QBAnswers
{
    public class QBAnswersAddRequest
    {
        public string? Text { get; set; }
        [Required]
        public bool Answer_TF { get; set; }
    }
}
